type CreditCard =
   { Number: string
     Expiry: string
     Cvv: string }

type UserId = UserId of string
type EmailAddress = EmailAddress of string
type EmailInstance = {
    To: EmailAddress
    Body: string
}

type User =
    { Id: UserId
      CreditCard: CreditCard
      EmailAddress: EmailAddress }

type PaymentId = PaymentId of string

type ISqlConnection =
    abstract Query: string -> User

type SqlConnection(sqlConn: string) =
    interface ISqlConnection with
        member this.Query q = 
            {
                Id = UserId "1"
                CreditCard = { Number = "1"; Expiry = "2"; Cvv = "" }
                EmailAddress = EmailAddress "w@w.com"
            }

type IPaymentClient =
    abstract Charge: CreditCard -> float -> PaymentId

type PaymentClient(key: string) =
    interface IPaymentClient  with
        member this.Charge (cc: CreditCard) (amount: float) =
            PaymentId "done"

type IEmailClient = 
    abstract Send: EmailInstance -> unit

type EmailClient() =
    interface IEmailClient with
        member this.Send (email: EmailInstance) = ()

module PaymentProvider =
    let chargeCard (card: CreditCard) amount (client: #IPaymentClient) =
        client.Charge card amount 

module Database =
    let getUser (id: UserId) (connection: #ISqlConnection) : User =
        connection.Query($"SELECT * FROM User AS u WHERE u.Id = {id}")    

module Email =
    let send (email: EmailInstance) (client: #IEmailClient) =
        client.Send email

// implementation
let inject f valueThatNeedsDep =
        fun deps ->
            let value = valueThatNeedsDep deps
            f value deps

type InjectorBuilder() =
    member _.Return(x) = fun _ -> x
    member _.Bind(x, f) = inject f x
    member _.Zero() = fun _ -> ()
    member _.ReturnFrom x = x

let injector = InjectorBuilder()

let chargeUser userId amount =
    injector {
        let! user = Database.getUser userId
        let! paymentId = PaymentProvider.chargeCard user.CreditCard amount
        let email = {
            To = user.EmailAddress
            Body = $"Your payment id is {paymentId}"
        }

        return Email.send email
    }

type IDeps =
    inherit IPaymentClient
    inherit ISqlConnection
    inherit IEmailClient

let deps =
    { 
        new IDeps with
        member _.Charge card amount =
            // create PaymentClient and call it

        member _.Send address body =
            // create SMTP client and call it

        member _.Query x =
            // create sql connection and invoke it 
    }

let paymentId = chargeUser (UserId "1") 2.50 deps

