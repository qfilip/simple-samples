#load "validation0_Base.fsx"

open Validation0_Base
open System.Text.RegularExpressions

type ValidationExpression() = 

    member _.Bind(x, f) = Validation0_Base.bind f x

    member _.BindReturn(x, f) = Validation0_Base.map f x

    member _.MergeSources(x, y) = Validation0_Base.zip x y

    member _.Return(x) = Success x


type Id = Id of int
type Email = Email of string
type User = { Id: Id; Email: Email }


let createId id =
    match (id > 0) with
    | true -> Success (Id id)
    | _ -> Fail ["Id must be above 0"]


let validateEmail str = 
    match Regex.IsMatch(str, "@") with
    | true -> Success (Email str)
    | _ -> Fail ["Bad email pattern"]
    

let createEmail str =
    if isNull str then Fail ["Email cannot be null"]
    else validateEmail str


let createUser id email =
    { Id = id; Email = email }


let valexp = new ValidationExpression()


let validateUser id email =
    valexp {
        let! vId = createId id
        and! vEmail = email

        let user = createUser vId vEmail
        return user
    }