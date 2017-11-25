﻿open System.Runtime.Serialization
open Configuration
open ResponseData

[<DataContract>]
type Command = {
    [<DataMember>]
    mutable cmd: string

    [<DataMember>]
    mutable requestId: string
}

printfn "Starting Test Server"

let asyncRequest (url: string) responseData = 
    async {
        match responseData.query.Value.method with
        | "affe" ->
            let test = responseData.query.Value
            let param1 = test.Query "param1" 
            let param2 = test.Query "param2"
            let param3 = test.Query "param41"

            let command = {
                cmd = "Kommando"
                requestId = "RekwestEidie"
            }
            //System.Threading.Thread.Sleep 3
            do! Response.asyncSendJson responseData command
            return true
        | _ -> return false
    }

let configuration = Configuration.create {
        Configuration.createEmpty() with 
            Port = 20000; 
            WebRoot = "/home/uwe/Projekte/Node/WebServerElectron/web/" 
            //WebRoot = "D:\Projekte\WebServerSharp\web" 
            // WebRoot = "C:\Users\urieg\Documents\Projects\Projekte\webroot" 
    }
    
try
    let server = Server.create configuration
    server.registerRequests asyncRequest
    server.start ()
    stdin.ReadLine() |> ignore
    server.stop ()
with
    | ex -> printfn "Fehler: %O" ex

