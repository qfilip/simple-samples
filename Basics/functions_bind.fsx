// Main Source: https://fsharpforfunandprofit.com/posts/elevated-world-2/

type E<'a> =
    | Is of 'a
    | Isnt


// description: Allows you to compose world-crossing ("monadic") functions
// note: use List.collect for lists
let bind f xElev =
    match xElev with
    | Is x -> f x
    | _ -> Isnt


let (>>=) = bind


// Sample usage
type Id = Id of string
type Age = Age of int
type Email = Email of string

type User = { Id: Id; Age: Age; Email: Email }

let createId (x: string) = if x.Length = 5 then Is (Id x) else Isnt
let createAge (x: int) = if x > 0 && x < 120 then Is (Age x) else Isnt
let createEmail (x: string) = if x.Contains('@') then Is (Email x) else Isnt

let createUser id age email = { Id = id; Age = age; Email = email }

let createUserUgly id age email =
    match (createId id) with
    | Isnt -> Isnt
    | Is isId ->
        match (createAge age) with
        | Isnt -> Isnt
        | Is isAge ->
            match (createEmail email) with
            | Isnt -> Isnt
            | Is isEmail -> Is (createUser isId isAge isEmail)


let createUserUgly2 id age email =
    let eId = createId id
    let eAge = createAge age
    let eEmail = createEmail email

    eId
    |> bind (fun isId -> 
        eAge 
        |> bind (fun isAge ->
            eEmail
            |> bind (fun isEmail -> 
                (createUser isId isAge isEmail) |> Is
            )
        )
    )


type UserBuilder() = 
    member _.Bind(x, f) = bind f x
    member _.Return(x) = Is x


let createUserPretty id age email =
    let builder = UserBuilder()
    builder {
        let! isId = createId id
        let! isAge = createAge age
        let! isEmail = createEmail email
        
        return (createUser isId isAge isEmail)
    }