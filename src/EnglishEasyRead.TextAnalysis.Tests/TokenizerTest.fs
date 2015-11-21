namespace EnglishEasyRead.TextAnalysis.Tests

open EnglishEasyRead.TextAnalysis.Tokenizer
open Xunit

type TokenizerTest() = 

    [<Fact>]
    let `` can parse text `` () =
        let result = tokenize "hello world"
        Assert.Equal(result.[0],"hello")
        Assert.Equal (result.[1], "world")


    [<Fact>]
    let `` converts words to lowercase `` () =
        let result = tokenize "HeLlo"
        Assert.Equal (result.[0], "hello")

    [<Fact>]
    let `` removes digits from text `` () = 
        let result = tokenize "hello 123 world"
        Assert.Equal(2, result.Length)
        Assert.DoesNotContain(result, fun s -> s.Equals "123")