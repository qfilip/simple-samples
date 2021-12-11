// Main Source: https://swlaschin.gitbooks.io/fsharpforfunandprofit/content/posts/elevated-world.html

type E<'a> =
    | Is of 'a
    | Isnt


// description: unpacks a function inside eValue into a eFunction
// wtfless: lifts multiparam function to multiparam eFunction
let apply fElev xElev =
    match fElev, xElev with
    | Is f, Is x -> Is <| f x
    | _ -> Isnt


let (<*>) = apply

let add = Is (+)

let r = add <*> (Is 1) <*> (Is 2)