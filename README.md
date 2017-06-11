# RNGesus

Easy-to-use API wrapping .NET's (cryptographically secure) `RNGCryptoServiceProvider`. Generate random strings, numbers, booleans or byte arrays without worrying about duplicates or insecure results.

## Why?

Generating random values in .NET isn't as easy as it should be. `System.Random` provides an easy-to-use API for generating numbers which may _appear_ to be pseudorandom, 
but which are actually trivial to predict if one knows the seed with which the value was created. `System.Random`, even when implemented perfectly, is not considered secure.
.NET's method of generating cryptographically secure random numbers is the `RNGCryptoServiceProvider` class in using `System.Security.Cryptography`, but it's API is 
significantly more convoluted than `System.Random`'s. This library merely abstracts the implementation details and provided a simple, static API.

## When?

Use this library when you require genuinely unpredictable random values, and are willing to endure the performance hit in the name of entropy. Cryptographically secure RNG is
fundamentally slower than insecure RNG.

## How?

Install the package from [NuGet](http://todo.com): `Install-Package Luke.RNGesus`

#### Booleans:

```csharp
// Generate a random boolean:
var randomBool = RNGesus.GenerateBool();
```

#### Integer types:

```csharp
// Generate a random int:
var randomInt = RNGesus.GenerateInt();

// Generate a random int below a given maximum:
var randomInt = RNGesus.GenerateInt(16);

// Generate a random int within a given range:
var randomInt = RNGesus.GenerateInt(16, 512);

// Generate a random long:
var randomLong = RNGesus.GenerateLong();

// Generate a random long below a given maximum:
var randomLong = RNGesus.GenerateLong(4294967296);

// Generate a random long within a given range:
var randomLong = RNGesus.GenerateLong(4294967296, 8589934592);
```

#### Strings:

```csharp
// Generate a random string of a given length:
var randomString = RNGesus.GenerateString(16);

// Generate a random string of a given length using a custom character set:
var randomString = RNGesus.GenerateString(16, "abcdef");

//or:
var randomString = RNGesus.GenerateString(16, new char[] {'a', 'b', 'c', 'd', 'e', 'f'});

//Generate a random string with character likelihood weightings***:
var randomString = RNGesus.GenerateString(16, "aaabbc", false);

//or***:
var randomString = RNGesus.GenerateString(16, new char[] {'a', 'a', 'a', 'b', 'b', 'c'}, false);
```
*** Only do this if you understand the ramifications - weighting the outputs lowers the entropy.

#### Floating-point types:

```csharp
// Generate a random float between 0 and 1:
var randomFloat = RNGesus.GenerateFloat();

// Generate a random double between 0 and 1:
var randomDouble = RNGesus.GenerateDouble();
```

#### Byte arrays:

```csharp
// Generate a random byte array of a given length:
var randomByteArray = RNGesus.GenerateByteArray(16);
```

#### Bonus:

```csharp
// Generate a string of length somewhere in a given range:
var randomString = RNGesus.GenerateByteArray(RNGesus.GenerateInt(16, 32));
```

## A word of warning:

Generating strings with custom character sets is only **truly** uniform if `256` evenly divides the number of available characters. This is because there are `256` possible values of a single byte,
and RNGesus uses modulo arithmetic to select the output character from the character set. For example, the character set

```csharp
"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
```

contains `62` distinct characters, which is not a value that `256` evenly divides (although `64` is). Thus, some of the characters in the input set are _fractionally_ more or less likely to be any 
given output byte than one another. A more appropriate character set would be:

```csharp
"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_"
```

which is `64` characters long - and also happens to be what RNGesus uses when no character set is specified.

## Acknowledgements

* GitHub user [Markus Olsson (niik)](https://github.com/niik), whose [implementation](https://gist.github.com/niik/1017834) RNGesus borrows from.

For any comments, questions, complaints, suggestions or requests, please don't hesitate to [create an issue](https://github.com/lukepothier/rngesus/issues/new) or contact me at [lukepothier@gmail.com](mailto:lukepothier@gmail.com).
