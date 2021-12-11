type Message =
    | Urgent of string
    | Normal of string

let scanAgent = MailboxProcessor<Message>.Start(fun mbox ->
    let rec loop () = async {
        // prioritize urgent messages if queue is larger than 8
        while mbox.CurrentQueueLength > 8 do
            do! mbox.Scan(fun m ->
                match m with
                | Urgent x -> Some(async { printfn "URGENT MESSAGE: %s" x })
                | Normal x ->
                    // return message to queue, or it gets lost
                    Some(async { mbox.Post(Normal x) })
            )
        
        // resume normal workflow
        let! msg = mbox.Receive()
        let printmsg x = printfn "Got message: %s" x
        
        match msg with
        | Urgent x -> printmsg x
        | Normal x -> printmsg x
        
        return! loop ()
    }

    loop ()
)

let messages = [
    Normal "Today on radio..."
    Normal "Flute Funk"
    Normal "Steam dream"
    Normal "Football finale"
    Urgent "Stuck in the middle with you..."
    Normal "Meow of the Mountain"
    Normal "My blood is gasoline!!!"
    Normal "Plants on Planets"
    Normal "Heeeey... Macarena..."
    Urgent "Winter is coming"
    Urgent "Black Holes in space"
]

messages |> List.map scanAgent.Post