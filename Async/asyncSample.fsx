// SOURCE: 
// https://fsharpforfunandprofit.com/posts/concurrency-async-and-parallel/

open System.Threading

let longOperation =
    async {
        do printfn "Long started" 
        do! Async.Sleep 4000
        do printfn "Long ended"
    }


let wait20 = async {
    for i in [1..20] do
        do! Async.Sleep 200
        do printfn "%i" i
}   


let all =
    Async.Parallel [wait20; longOperation] |> Async.RunSynchronously