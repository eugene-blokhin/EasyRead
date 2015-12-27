namespace EnglishEasyRead.CoreApi

open System
open ServiceStack.Redis
open ServiceStack.Redis.Generic

[<CLIMutableAttribute>]
type TextData = 
    { Id : Guid
      Name : string
      Body : string }

type ITextService =
    abstract SaveText : name:string -> body:string -> lemmas:string array -> Guid
    abstract GetLemmasByTextId : textId:Guid -> string array

type TextService(redisHost : string) =
    let getTextLemmasId id = sprintf "urn:textDataLemmas:%O" id 

    interface ITextService with
        member this.SaveText name body lemmas = 
            System.Diagnostics.Debugger.Break()
            
            let textId = Guid.NewGuid()
            let textEntry = {
                Id = textId
                Name = name
                Body = body
            }
            
            let redisClient = new RedisClient()
            let redisTextData = redisClient.As<TextData>()
            let redisLemmas = redisClient.Sets.[getTextLemmasId textId]

            redisTextData.Store(textEntry) |> ignore
            lemmas |> Array.iter redisLemmas.Add
            
            textId
        
        member this.GetLemmasByTextId id = 
            let redisClient = new RedisClient(redisHost)
            redisClient.Sets.[getTextLemmasId id].GetAll()
            |> Array.ofSeq
