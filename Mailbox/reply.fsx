type Message = string * AsyncReplyChannel<string>

let replyAgent = MailboxProcessor<Message>.Start(fun mbox ->
    let rec loop () = async {
        let! (msg, replyChannel) = mbox.Receive()
        match msg with
        | "pineapple" -> replyChannel.Reply("pina")
        | "password" -> replyChannel.Reply("contrasena")
        | _ -> replyChannel.Reply("failed to translate")
        
        return! loop ()
    }

    loop ()
)

replyAgent.PostAndReply(fun rc -> "pineapple", rc) |> printfn "%s"
replyAgent.PostAndReply(fun rc -> "corruption", rc) |> printfn "%s"