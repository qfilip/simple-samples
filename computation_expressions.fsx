module Funky =
    let map f xOpt =
        match xOpt with
        | Some x -> Some <| f x
        | _ -> printfn "Failed"; None


    let apply fOpt xOpt =
        match fOpt, xOpt with
        | Some f, Some x -> Some <| f x
        | _ -> printfn "Failed"; None


    let bind f xOpt =
        match xOpt with
        | Some x -> f x
        | _ -> printfn "Failed"; None


    let (<*>) = apply
    let (>>=) x f = bind f x
    

    let zip x y =
        let toTuple x1 x2 = (x1, x2)
        Some toTuple <*> x <*> y

    // using 'apply' instead of '<*>' can compile ??? 


type CompexBuilder() =
    member _.MergeSources(x, y) =
        Funky.zip x y


    member _.Bind(x, f) =
        Funky.bind f x

   
    member _.Return(x) = Some x


let builder = CompexBuilder()

let addMonad a b c =
    builder {
        let! r1 = a
        let! r2 = b
        let! r3 = c

        return r1 + r2 + r3
    }


let addApplicative a b c =
    builder {
        let! r1 = a
        and! r2 = b
        and! r3 = c

        return r1 + r2 + r3
    }


// this will fail at first None
let monadResult = addMonad (Some 2) (None) (Some 3)

// this will go through every param
let applicativeResult = addApplicative (Some 2) (None) (Some 3)

