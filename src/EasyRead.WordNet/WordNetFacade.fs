namespace EasyRead.WordNet

type IWordNetFacade = 
    abstract CategoryIndexSeek : syntacticCategory:SyntacticCategory -> lemma:string -> IndexFileRecord option
    abstract IndexSeek : lemma:string -> IndexFileRecord array
    abstract GetData : syntacticCategory:SyntacticCategory -> synsetOffset:int -> DataFileRecord option

type WordNetFacade(databasePath : string) = 
    
    let loadIndex category = 
        lazy (WordNetProvider.loadIndex databasePath category
              |> Seq.map (fun record -> (record.Lemma, record))
              |> Map)
    
    let nounIndex = loadIndex Noun
    let verbIndex = loadIndex Verb
    let adverbIndex = loadIndex Adverb
    let adjectiveIndex = loadIndex Adjective

    let loadData category = 
        lazy (WordNetProvider.loadData databasePath category
              |> Seq.map (fun record -> (record.SynsetOffset, record))
              |> Map)
    
    let nounData = loadData Noun
    let verbData = loadData Verb
    let adverbData = loadData Adverb
    let adjectiveData = loadData Adjective

    interface IWordNetFacade with
        member this.GetData (syntacticCategory: SyntacticCategory) (synsetOffset: int) = 
            let categoryData =
                match syntacticCategory with
                | Noun -> nounData
                | Verb -> verbData
                | Adjective -> adjectiveData
                | Adverb -> adverbData
            categoryData.Value.TryFind synsetOffset

        member this.IndexSeek(lemma: string) = 
            [|Noun; Verb; Adverb; Adjective|] 
            |> Array.choose (fun c -> (this :> IWordNetFacade).CategoryIndexSeek c lemma)
        
        member this.CategoryIndexSeek (syntacticCategory:SyntacticCategory) (lemma:string) =
            let categoryIndex =
                match syntacticCategory with
                | Noun -> nounIndex
                | Verb -> verbIndex
                | Adjective -> adjectiveIndex
                | Adverb -> adverbIndex
            categoryIndex.Value.TryFind lemma

    member this.GetData (syntacticCategory: SyntacticCategory) (synsetOffset: int) = (this :> IWordNetFacade).GetData syntacticCategory synsetOffset
    member this.IndexSeek (lemma: string) = (this :> IWordNetFacade).IndexSeek lemma
    member this.CategoryIndexSeek (syntacticCategory:SyntacticCategory) (lemma:string) = (this :> IWordNetFacade).CategoryIndexSeek syntacticCategory lemma
