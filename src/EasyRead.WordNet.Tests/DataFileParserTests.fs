namespace EasyRead.WordNet.Tests

open NUnit.Framework
open EnglishEasyRead.WordNet

[<TestFixture>]
type DataFileParserTests() = 
    
    [<Test>]
    member this.``Ensure parseLine returns None if it is a comment line``() = 
        let line = "  1 This software and database is being provided to you, the LICENSEE, by  "
        let result = DataFileParser.parseLine line
        Assert.AreEqual(None, result)
    
    [<Test>]
    member this.``Test paseLine with one word in a synset``() = 
        let line = 
            "00001930 03 n 01 physical_entity 0 007 @ 00001740 n 0000 ~ 00002452 n 0000 ~ 00002684 n 0000 " 
            + "~ 00007347 n 0000 ~ 00020827 n 0000 ~ 00029677 n 0000 ~ 14580597 n 0000 | an entity that has physical existence  "
        
        let expectedResult = 
            { SynsetOffset = 1930
              Words = [| { Word = "physical_entity" } |]
              Gloss = "an entity that has physical existence" }
        
        let result = DataFileParser.parseLine line
        Assert.True(result.IsSome)
        CustomAsserts.areEqual (expectedResult, result.Value)
    
    [<Test>]
    member this.``Test paseLine with several words in a synset``() = 
        let line = 
            "03748162 06 n 04 mercantile_establishment 0 retail_store 0 sales_outlet 0 outlet 1 009 @ 03953020 n 0000 " 
            + "~ 03119203 n 0000 ~ 03176763 n 0000 ~ 03206405 n 0000 ~ 03722288 n 0000 %p 03748886 n 0000 ~ 03965456 n 0000 " 
            + "~ 04202417 n 0000 ~ 04340019 n 0000 | a place of business for retailing goods  "
        
        let expectedResult = 
            { SynsetOffset = 3748162
              Words = 
                  [| { Word = "mercantile_establishment" }
                     { Word = "retail_store" }
                     { Word = "sales_outlet" }
                     { Word = "outlet" } |]
              Gloss = "a place of business for retailing goods" }
        
        let result = DataFileParser.parseLine line
        Assert.True(result.IsSome)
        CustomAsserts.areEqual (expectedResult, result.Value)
