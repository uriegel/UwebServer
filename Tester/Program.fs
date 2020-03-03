﻿open System.Runtime.Serialization
open Configuration
open Session
open Request

type Command = {
    Cmd: string
    RequestId: string
    Count: int64
}

type Input = {
    id: string
    name: string
}

// TODO:
// Testprogramm: TCPSender: send Header, dann Send Json
// Testprogramm: dann SendJson > 20000 bzw. Buffersize = 1000 Json > 4000

printfn "Starting Test Server"

let asyncRequest (requestSession: RequestSession) = 
    async {
        match requestSession.query.Value.Method with
        | "runOperation" ->
            let jason = asyncGetJson<Input> requestSession.requestData 
            let id = jason.id
            let command = {
                Cmd = "Kommando"
                RequestId = "RekwestEidie"
                Count= 45L
            }
            //System.Threading.Thread.Sleep 3
            do! requestSession.asyncSendJson (command :> obj)
            return true
        | "affe" ->
            let test = requestSession.query.Value
            let param1 = test.Query "param1" 
            let param2 = test.Query "param2"
            let param3 = test.Query "param41"

            let command = {
                Cmd = "Kommando"
                RequestId = "RekwestEidie"
                Count= 45L
            }
            //System.Threading.Thread.Sleep 3
            do! requestSession.asyncSendJson (command :> obj)
            return true
        | _ -> return false
    }

let onWebSocketClose _ =
    printfn "%s" "gekloßt"
    
let onNewWebSocket _ __ = 
    {
        id = ""
        onClose = onWebSocketClose
    }

let configuration = Configuration.create {
    Configuration.createEmpty() with 
        WebRoot = "webroot" 
        Port=20000
        AllowOrigins = Some [| "http://localhost:8080" |]
        onNewWebSocket = onNewWebSocket
        asyncRequest = asyncRequest
        favicon = "Uwe.jpg"
}




try
        // let rec getLinesBufferAt lines = 
        //     seq {
        //         match getLinesBuffer () with
        //         | Some lines -> yield! getLinesBufferAt lines
        //         | None -> yield lines
        //     }

        // getLinesBufferAt  |> Seq.concat

        
    //let seq2 = getLines ()

    // printfn "\nThe"
    // let ret = seq2 |> Seq.toArray



//    printfn "\nThe sequence fib contains Fibonacci numbers. %d" ret.Length
    // for x in ret do printf "%O " x
    // printfn ""

    let server = Server.create configuration 
    server.start ()
    stdin.ReadLine() |> ignore
    server.stop ()
with
    | ex -> printfn "Fehler: %O" ex

