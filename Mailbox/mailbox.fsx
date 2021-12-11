type PlaneTicket = { Owner: string option; Destination: string }

type TouristAgency = {
    Clients: string list
    Tickets: PlaneTicket list
}

type TicketReservationRequest = { Client: string; Destination: string }

let reserveAgent = MailboxProcessor.Start(fun inbox ->
    let reserve (agency: TouristAgency) (req: TicketReservationRequest) =
        let index = 
            agency.Tickets
                |> List.tryFindIndex (fun x -> x.Owner = None && x.Destination = req.Destination)
        
        match index with
        | None -> 
            printfn "cannot reserve ticket"
            agency
        | Some i ->
            let t = agency.Tickets.[i]
            let ts = agency.Tickets |> List.map (fun x -> if x = t then { t with Owner = Some req.Client } else x)
            let newTrack = { Clients = agency.Clients; Tickets = ts }
            
            printfn "TICKET RESERVED BY %s" req.Client
            newTrack


    let rec messageLoop tickTracker = async {
        let! req = inbox.Receive()
        let newTickTracker = reserve tickTracker req
        return! messageLoop newTickTracker
    }

    let initilState = {
        Clients = [ "xerxes"; "yirma"; "zed" ]
        Tickets = [ { Owner = None; Destination = "bali" } ]
    }

    messageLoop initilState
)

let makeReservingTask reserver dest = async {
    let req = { Client = reserver; Destination = dest }
    reserveAgent.Post(req)
}


[ "xerxes"; "yirma"; "zed" ]
    |> List.map (fun i -> makeReservingTask i "bali")
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore