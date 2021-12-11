#load "validation0_Base.fsx"

open Validation0_Base
open System.Text.RegularExpressions

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


let (<!>) = map
let (<*>) = apply


let tryCreateUser id email =
    let idResult = createId id
    let emailResult = createEmail email
    createUser
    <!> idResult
    <*> emailResult


// test implementation
let goodResult = tryCreateUser 1 "kwanza@notmail.com"
let badResult = tryCreateUser -1 "kwanza.com"