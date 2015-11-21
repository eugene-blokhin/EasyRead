namespace EasyRead.WordNet.Tests

open NUnit.Framework

module CustomAsserts = 
    let areEqual (expected, actual) = 
        if expected <> actual then 
            let message = sprintf "Expected: %A\nBut was:  %A" expected actual
            raise (new AssertionException(message))
