// Main Source: https://swlaschin.gitbooks.io/fsharpforfunandprofit/content/posts/elevated-world.html

type E<'a> =
    | Is of 'a
    | Isnt

// description: lifts a function to eWorld
let map f xElev =
    match xElev with
    | Is x -> Is (f x)
    | Isnt -> Isnt


let (<!>) = map


// operators: none
// description: lifts a single value to eWorld
let retrn x = (Is x)

// elevate a function
let add1 = (+) 1
let add1' = map add1

// use that function on elevated list
let add1ToList' = List.map add1'

// composition of a function and eFunction must match
let f = (+) 1
let g = (+) 2
let h = f >> g

let f' = map f
let g' = map g

// signatures must remain the same, even when
// you compose normal functions THEN elevate them
// check functor laws at: https://en.wikibooks.org/wiki/Haskell/The_Functor_class#The_functor_laws

let h1' = f' >> g'
let h2' = map (f >> g)
