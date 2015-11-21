namespace EnglishEasyRead.WordNet

module WordNetProvider = 
    open System.IO
    
    [<CompiledNameAttribute("LoadExceptions")>]
    let loadExceptions databasePath syntacticCategory = 
        let filename = 
            match syntacticCategory with
            | Noun -> "noun.exc"
            | Verb -> "verb.exc"
            | Adverb -> "adv.exc"
            | Adjective -> "adj.exc"
        ExceptionFileParser.parseExceptionFile <| Path.Combine(databasePath, filename)
    
    [<CompiledNameAttribute("LoadIndex")>]
    let loadIndex databasePath syntacticCategory = 
        let filename = 
            match syntacticCategory with
            | Noun -> "index.noun"
            | Verb -> "index.verb"
            | Adverb -> "index.adv"
            | Adjective -> "index.adj"
        IndexFileParser.parseIndexFile <| Path.Combine(databasePath, filename)
    
    let loadData databasePath syntacticCategory = 
        let filename = 
            match syntacticCategory with
            | Noun -> "data.noun"
            | Verb -> "data.verb"
            | Adverb -> "data.adv"
            | Adjective -> "data.adj"
        DataFileParser.parseDataFile <| Path.Combine(databasePath, filename)
