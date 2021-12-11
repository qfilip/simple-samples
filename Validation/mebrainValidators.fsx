open System
open System.Text.RegularExpressions

type UnvalidatedUser =
    { Name: string
      Email: string
      DateOfBirth: string }
type ValidatedUser =
    { Name: string
      Email: string
      DateOfBirth: DateTime }
type ValidationFailure =
    | NameIsInvalidFailure
    | EmailIsInvalidFailure
    | DateOfBirthIsInvalidFailure

let (|ParseRegex|_|) regex str =
    let m = Regex(regex).Match(str)
    if m.Success
    then Some(List.tail [ for x in m.Groups -> x.Value ])
    else None


let (|IsValidName|_|) input =
    if input <> String.Empty then Some() else None


let (|IsValidEmail|_|) input =
    match input with
    | ParseRegex ".*?@(.*)" [ _ ] -> Some input
    | _ -> None


let (|IsValidDate|_|) input =
    let (success, value) = DateTime.TryParse(input)
    if success then Some value else None


let validateName input =
    match input with
    | IsValidName -> Ok input
    | _ -> Error [ NameIsInvalidFailure ]


let validateEmail input =
    match input with
    | IsValidEmail email -> Ok email
    | _ -> Error [ EmailIsInvalidFailure ]

    
let validateDateOfBirth input =
    match input with
    | IsValidDate dob -> Ok dob //Add logic for DOB
    | _ -> Error [ DateOfBirthIsInvalidFailure ]


let apply fResult xResult =
    match fResult, xResult with
    | Ok f, Ok x -> Ok(f x)
    | Error ex, Ok _ -> Error ex
    | Ok _, Error ex -> Error ex
    | Error ex1, Error ex2 -> Error(List.concat [ ex1; ex2 ])


let (<!>) = Result.map


let (<*>) = apply


let create name email dateOfBirth =
    { Name = name
      Email = email
      DateOfBirth = dateOfBirth }


let validate (input: UnvalidatedUser): Result<ValidatedUser, ValidationFailure list> =
    let validatedName = input.Name |> validateName
    let validatedEmail = input.Email |> validateEmail
    let validatedDateOfBirth = input.DateOfBirth |> validateDateOfBirth
    create
    <!> validatedName
    <*> validatedEmail
    <*> validatedDateOfBirth


let validTest =
    let actual =
        validate
            { Name = "Ian"
              Email = "hello@test.com"
              DateOfBirth = "2000-02-03" }
    let expected =
        Ok
            { Name = "Ian"
              Email = "hello@test.com"
              DateOfBirth = DateTime(2000, 2, 3) }
    expected = actual


let notValidTest =
    let actual =
        validate
            { Name = ""
              Email = "hello"
              DateOfBirth = "" }
    let expected =
        Error [ NameIsInvalidFailure
                EmailIsInvalidFailure
                DateOfBirthIsInvalidFailure ]
    expected = actual