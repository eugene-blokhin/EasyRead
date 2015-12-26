namespace EnglishEasyRead.DictionaryApi

open EnglishEasyRead.WordNet
open Nancy
open System.IO

type ITextAnalysisService =
    abstract ExtractLemmas : text:string -> string array

type TextAnalysisService(wordNetFacade : IWordNetFacade, morpher : Morpher) =
    interface ITextAnalysisService with
        member this.ExtractLemmas text =
            let lemmaExists word = (wordNetFacade.IndexSeek word).Length > 0

            Tokenizer.tokenize text
            |> Seq.map morpher.GetPossibleLemmas
            |> Seq.concat
            |> Seq.distinct
            |> Seq.where lemmaExists
            |> Array.ofSeq

type DictionaryApi(textAnalysisService : ITextAnalysisService) as this =
    inherit NancyModule("dictionary")

    do
        this.Post.["/get-lemmas"] <- fun _ ->
            use reader = new StreamReader(this.Request.Body)
            reader.ReadToEnd() 
            |> textAnalysisService.ExtractLemmas
            :> obj
