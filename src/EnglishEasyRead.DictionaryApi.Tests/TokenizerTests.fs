namespace EnglishEasyRead.DictionaryApi.Tests

open NUnit.Framework
open EnglishEasyRead.DictionaryApi.Tokenizer

[<TestFixture>]
type TokenizerTests() = 
    
    [<Test>]
    member this.Test01() = 
        let result = tokenize "hello world" |> Array.ofSeq
        Assert.AreEqual(result.[0], "hello")
        Assert.AreEqual(result.[1], "world")
    
    [<Test>]
    member this.Test02() = 
        let result = tokenize "hello" |> Array.ofSeq
        Assert.AreEqual(result.[0], "hello")
    
    [<Test>]
    member this.Test03() = 
        let result = tokenize "hello 123 world" |> Array.ofSeq
        Assert.AreEqual(2, result.Length)
        CollectionAssert.DoesNotContain(result, "123")
