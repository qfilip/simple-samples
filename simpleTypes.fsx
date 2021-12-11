type PositiveInt = private PositiveInt of int

module PositiveInt =
    let private validator (x: int) =
        if x < 0 then Error "Negative number"
        else Ok (PositiveInt x)
    
    let wrap x = validator x

    let unwrap (PositiveInt x) = x