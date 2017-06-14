# RNGesus

[![Build status](https://ci.appveyor.com/api/projects/status/njv1rqxqxjdbef9v?svg=true)](https://ci.appveyor.com/project/lukepothier/rngesus)

Easy-to-use API wrapping .NET's (cryptographically secure) `RNGCryptoServiceProvider`. Generate random strings, numbers, booleans or byte arrays without worrying about duplicates or insecure results.

## Why?

Generating random values in .NET isn't as easy as it should be. `System.Random` provides an easy-to-use API for generating values which appear to be pseudorandom, 
but which are actually trivial to predict if one knows the seeds with which they were created. `System.Random`, even when implemented perfectly, is not considered secure.
.NET's method of generating cryptographically secure random numbers is the `RNGCryptoServiceProvider` class in `System.Security.Cryptography`, but it's API is 
significantly more convoluted than `System.Random`'s is. This library merely abstracts the implementation details and provides a simple API for generating cryptographically secure random values.

## When?

Use this library when you require genuinely unpredictable random values, and are willing to endure the performance hit in the name of security. Cryptographically secure RNG is
fundamentally slower than insecure RNG.

## How?

Install the package from [NuGet](https://www.nuget.org/packages/Luke.RNGesus): `Install-Package Luke.RNGesus`

Add the using statement: `using Luke.RNG`

RNGesus implements `IDisposable`, so it's a good idea to wrap it in a `using` statement.

**NOTE**: By default, RNGesus works using a shared buffer which is 1024 bytes long. If you require outputs larger than 1024 bytes, instantiate RNGesus using the appropriate overload, for example:

```csharp
var rngesus = new RNGesus(4096);
```

#### Booleans:

```csharp
// Generate a random boolean:
using (var rngesus = new RNGesus())
{
    var randomBool = rngesus.GenerateBool();
}

```

#### Integer types:

```csharp
// Generate a random int:
using (var rngesus = new RNGesus())
{
    var randomInt = rngesus.GenerateInt();
}

// Generate a random int below a given maximum:
using (var rngesus = new RNGesus())
{
    var randomInt = rngesus.GenerateInt(16);
}

// Generate a random int within a given range:
using (var rngesus = new RNGesus())
{
    var randomInt = rngesus.GenerateInt(16, 512);
}

// Generate a random long:
using (var rngesus = new RNGesus())
{
    var randomLong = rngesus.GenerateLong();
}

// Generate a random long below a given maximum:
using (var rngesus = new RNGesus())
{
    var randomLong = rngesus.GenerateLong(4294967296);
}

// Generate a random long within a given range:
using (var rngesus = new RNGesus())
{
    var randomLong = rngesus.GenerateLong(4294967296, 8589934592);
}
```

#### Strings:

```csharp
// Generate a random string of a given length:
using (var rngesus = new RNGesus())
{
    var randomString = rngesus.GenerateString(16);
}

// Generate a random string of a given length using a custom character set:
using (var rngesus = new RNGesus())
{
    var randomString = rngesus.GenerateString(16, "abcdef");
}

//or:
using (var rngesus = new RNGesus())
{
    var randomString = rngesus.GenerateString(16, new char[] {'a', 'b', 'c', 'd', 'e', 'f'});
}

//Generate a random string with character likelihood weightings***:
using (var rngesus = new RNGesus())
{
    var randomString = rngesus.GenerateString(16, "aaabbc", false);
}

//or***:
using (var rngesus = new RNGesus())
{
    var randomString = rngesus.GenerateString(16, new char[] {'a', 'a', 'a', 'b', 'b', 'c'}, false);
}
```
*** Only do this if you understand the ramifications - weighting the outputs lowers the entropy.

#### Floating-point types:

```csharp
// Generate a random float between 0 and 1:
using (var rngesus = new RNGesus())
{
    var randomFloat = rngesus.GenerateFloat();
}

// Generate a random double between 0 and 1:
using (var rngesus = new RNGesus())
{
    var randomDouble = rngesus.GenerateDouble();
}
```

#### Byte arrays:

```csharp
// Generate a random byte array of a given length:
using (var rngesus = new RNGesus())
{
    var randomByteArray = rngesus.GenerateByteArray(16);
}
```

#### Bonus:

```csharp
// Generate a string of length somewhere in a given range:
using (var rngesus = new RNGesus())
{
    var randomString = rngesus.GenerateString(rngesus.GenerateInt(16, 32));
}
```

## A word of warning:

Strings generated with custom character sets are only _truly_ uniform, and therefore secure, if `256` evenly divides the number of available characters. This is because there are `256` possible values 
of a single byte, and RNGesus uses modulo arithmetic to select the output character from the character set. For example, the character set

```csharp
"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
```

contains `62` distinct characters, which is not a value that `256` evenly divides (although `64` is). Thus, some of the characters in the input set are more or less likely to be any given output 
byte than one another. In this example, `abcdefgh` are 25% more likely to appear at a given output byte than their counterparts. A more appropriate character set would be:

```csharp
"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_"
```

which is `64` characters long - and also happens to be what RNGesus uses when no custom character set is specified.

## TODO:

Currently RNGesus only supports .NET v4.6 (`net46`). Version 1.2 will add support for other versions of .NET.

## Acknowledgements:

* [Markus Olsson (niik)](https://github.com/niik), whose [CryptoRandom](https://gist.github.com/niik/1017834) RNGesus borrows from.

For any comments, questions, complaints, suggestions or requests, please don't hesitate to [create an issue](https://github.com/lukepothier/rngesus/issues/new) or contact me at [lukepothier@gmail.com](mailto:lukepothier@gmail.com).
