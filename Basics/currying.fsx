open System

// 2 param fn
let colorPrint (color: ConsoleColor) (msg: string) =
    Console.ForegroundColor <- color
    printfn "%s" msg


// currying example 1, return inner function
let getPrintFn (color: ConsoleColor) =
    let colorPrint (msg: string) =
        Console.ForegroundColor <- color
        printfn "%s" msg

    colorPrint


// using returned function
let bluePrint = getPrintFn ConsoleColor.Blue

// currying example 2, same effect but faster (just choppin' the original 2 param fn)
let bluePrint' = colorPrint ConsoleColor.Blue