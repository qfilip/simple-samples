open System
open System.Threading

let report (color : ConsoleColor) (message : string) =
     Console.ForegroundColor <- color
     printfn "%s (thread ID: %i)" message Thread.CurrentThread.ManagedThreadId
     Console.ResetColor()

let red = report ConsoleColor.Red
let green = report ConsoleColor.Green
let yellow = report ConsoleColor.Yellow
let cyan = report ConsoleColor.Cyan

let srcDir = __SOURCE_DIRECTORY__

red srcDir
