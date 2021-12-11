#load "validation0_Base.fsx"

open Validation0_Base
open System.Text.RegularExpressions

type Password = Password of string

let toResult x bool errmsg =
    if bool then Validation0_Base.Success x
    else Validation0_Base.Fail [errmsg]

let hasDigits (x: string) = toResult x (Regex.IsMatch(x, "\d")) "Digits required"
let hasSymbol (x: string) = toResult x (Regex.IsMatch(x, "\W")) "Symbol required"
let hasLength (x: string) = toResult x (x.Length >= 8) "Too short"
let hasUppercase (x: string) = toResult x (Regex.IsMatch(x, "[A-Z]")) "No uppercases"

let (<!>) = Validation0_Base.bind
let (<*>) = Validation0_Base.apply

let createPassword password = (Password password)

let validatePassword (str: string): ValidationResult<Password, string list> =
    let liftError result =
        match result with
        | Success x -> Success x
        | Fail e -> Fail [ e ]
    
    Success (createPassword)
    |> apply (str |> hasDigits |> liftError)
    |> apply (str |> hasSymbol |> liftError)

let vpass = "MyStupidP@ssword1"
let ipass = "passwd"



let result = validator ipass


