namespace EnglishEasyRead.DictionaryApi

module Tokenizer = 
    open System.Text.RegularExpressions
    
    let tokenize text = 
        Regex.Matches(text, @"([a-z]+)", RegexOptions.IgnoreCase)
        |> Seq.cast<Match>
        |> Seq.choose (fun m -> 
               if (m.Success) then Some(m.Groups.[1].Value)
               else None)
        |> Seq.map (fun w -> w.ToLower())