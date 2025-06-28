# Method.Contracts

A set of tools to enforce contracts in methods.

[![Build status](https://ci.appveyor.com/api/projects/status/ex06fppm0o8d8fh1?svg=true)](https://ci.appveyor.com/project/dlebansais/method-contracts) [![codecov](https://codecov.io/gh/dlebansais/Method.Contracts/graph/badge.svg?token=9F0GWW4VVZ)](https://codecov.io/gh/dlebansais/Method.Contracts) [![CodeFactor](https://www.codefactor.io/repository/github/dlebansais/method.contracts/badge)](https://www.codefactor.io/repository/github/dlebansais/method.contracts) [![NuGet](https://img.shields.io/nuget/v/Method.Contracts.svg)](https://www.nuget.org/packages/Method.Contracts)

This assembly applies to projects using **C# 8 or higher** and with **Nullable** enabled.

## Usage

Add the assembly from the latest release as a dependency of your project. The `Contracts` namespace then becomes available.

````csharp
using Contracts;
````
    
### ``Contract.RequireNotNull``

This static method can be used to check parameters and remove [warning CA1062](https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1062).

Consider the following code:

````csharp
public bool TryParseFoo(string text, out Foo parsedFoo)
{
    if (text.Length > 0)
    {
	// ...
    }
````

The line `if (text.Length > 0)` generates warning CA1062: *Validate arguments of public methods*. The traditional way of removing this warning is to check for the `null` value, as follow.

````csharp
public bool TryParseFoo(string text, out Foo parsedFoo)
{
    if (text is null)
        throw new ArgumentNullException(nameof(text));

    if (text.Length > 0)
    {
	// ...
    }
````

You can replace this code with `RequireNotNull` instead:

````csharp
public bool TryParseFoo(string text, out Foo parsedFoo)
{
    Contract.RequireNotNull(text, out string Text);

    if (Text.Length > 0)
    {
	// ...
    }
````

Note how the new code uses `Text` with an upper case T and not the parameter anymore.

By using `RequireNotNull` you can slightly improve your code, at least from a point of view:

+ You can make the check take only one line and keep warnings about single-line statements active (see for instance [warning SA1502](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1502.md)).
+ You can make this contract easy to replace everywhere with search/replace once the corresponding feature is added to .NET, if it ever happen.
+ This check explicitly means you're declaring a code contract about your parameter.
+ The debug version of your code will generate a `Debug.Assert` failure, the release version will throw `ArgumentNullException`. This means that even if during debugging and tests you couldn't reproduce a case where the inspected value is null, in production code there will still be an exception thrown.

The drawback of using `RequireNotNull` is, of course, that you introduce a new variable.

#### Special version for ``IDisposable`` objects

If the `out` value is a `IDisposable` object, the compiler generates [warning CA2000](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca2000): *Dispose objects before losing scope*.

In this case, use the version of `RequireNotNull` that returns the non-null value:

````csharp
    Stream Stream = Contract.RequireNotNull(stream);
````

This call can be confused with `AssertNotNull` but there is no helping it.

### ``Contract.Require``

This method takes advantage of compiler features to break when an expression is false with a meaningful text that includes the expression text. Its purpose is to check arguments. This check explicitly means you're declaring a code contract about your parameter.

For example, consider this code :

````csharp
public double SquareRoot(double value)
{
    Contract.Require(value >= 0);

    // ...
}
````

If `value` is negative, the debug version will trigger a `Debug.Assert` failure, and the release version will throw `ArgumentException`, both will a meaningful message that includes the "value >= 0" text.

### ``Contract.Ensure``

This method takes advantage of compiler features to break when an expression is false with a meaningful text that includes the expression text. Its purpose is to check the value returned by a method and the object state. This check explicitly means you're declaring a code contract about the method exit state.

For example, consider this code :

````csharp
public double SquareRoot(double value)
{
    double Result;

    // ...

    Contract.Ensure(Result >= 0);
    return Result;
}
````

If `Result` is negative, the debug version will trigger a `Debug.Assert` failure, and the release version will throw `BrokenContractException`, both will a meaningful message that includes the "Result >= 0" text.

### ``Contract.Unused``

The purpose of this method is mostly to annotate your code to specify that a variable value is not used, and remove warning CS8625: *Cannot convert null literal to non-nullable reference type*.

Consider the following code:

````csharp
public bool TryParseFoo(string text, out Foo parsedFoo)
{
    if (text.Length > 0)
    {
        // ... Obtain parsedFoo
        return true;
    }

    parsedFoo = null;
    return false;
}
````

The line `parsedFoo = null;` generates warning CS8625. The traditional way of removing this warning is then to add the `!` null forgiving operator, as follow:

````csharp
parsedFoo = null!;
return false;
````

You can replace this code with `Unused` instead:

````csharp
Contract.Unused(out parsedFoo);
return false;
````

By using `Unused` you can slightly improve your code, at least from a point of view:

+ The null forgiving operator is easily missed.
+ This check explicitly means you're declaring a code contract about your output.

### ``Contract.AssertNotNull``

.NET analyzers can detect if the code checks whether some reference is null or not, and analyze the rest of the code accordingly.

For instance :

````csharp
    if (text is null)
        throw new MyUnexpectedNullException(nameof(text));

    // Here, the analyzer knows that text is not null.
````

In .NET Framework, `Debug.Assert(text is not null)` does the opposite of what is expected: the analyzer sees that `text` is compared to `null`, but does not conclude that it's non-null, and in the code that follows assumes text is of type `string?`, not `string` as declared, and therefore can be null.

To avoid using `#if` directives to compile differently for .NET Framework and .NET 6 or greater, use the following call:

````csharp
    string Text = Contract.AssertNotNull(text);
````

`text` will then be tested, and if null will trigger `Debug.Assert` in the debug version, or throw `BrokenContractException` in the release version.

### ``Contract.AssertOfType<T>``

.NET analyzers can detect if the code checks whether some reference is of a given type or not, and analyze the rest of the code accordingly.

For instance :

````csharp
    if (value is not string Text)
        throw new MyWrongTypeException(nameof(value));

    // Here, the analyzer knows that Text is a non-null string.
````

In .NET Framework, `Debug.Assert(value is string)` requires an additional operation to assign the type: `string Text = (string)value;`. But `null` is propagated then, which means that analyzers react differently depending on the .NET Framework used (see the previous section).

To avoid using `#if` directives to compile differently for .NET Framework and .NET 6 or greater, use the following call:

````csharp
    string Text = Contract.AssertOfType<string>(value);
````

`value` will then be tested, and if `null` or not a `string` will trigger `Debug.Assert` in the debug version, or throw `BrokenContractException` in the release version.

### ``Contract.AssertNoThrow``

Methods may declare that they can throw exceptions in the documentation. The caller could either let any unexpected exception go through, or catch them and fallback to a more friendly approach such as returning a failure code.
Yet, when no exception is thrown in testable cases the exception handling code is never tested.
`AssertNoThrow` offers another approach, which is to remove any exception thrown in some code, and to trigger `Debug.Assert` in the debug version, or throw `BrokenContractException` in the release version.

For example:

````csharp
    string Text = Contract.AssertNoThrow(
        () => ReadStringFromFile(filename)
    );
````

In the case above, if the code has previously taken all necessary precautions to check that the file exists and has reading access (with appropriate error reporting to the user), and can prevent the file from being deleted or moved (maybe only temporarily), then performing the read operation is safe.

### ``Contract.Assert``

This method is similar to `Debug.Assert`, with the following differences:

+ It is also compiled in release mode. If you don't want that, just use `Debug.Assert`.
+ If no assert message is specified, the default message is the expression in the first parameter converted to a string.
+ In release mode it throws `BrokenContractException` if the expression is evaluated to `false`.

### ``Contract.Map``

When using source coverage tools it is common to observe a `switch` instruction (or expression) that is not fully covered. For exemple, if all possible values are explicitly in a `case` clause, the `default` clause is unreachable. However, having a `default` clause is desirable in case the enum is extended with new values.

To cover this case the programmer typically has to pick one of the enum values and replace it with `default`. This is not only aesthetically questionable, it also contradicts some rules like [IDE0010](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0010): *Add missing cases to switch statement*.

Additionally it is hard to properly control what happens when a value is added to the enum. If, for instance, one adds a `default` case that throws an exception, this case cannot be tested, and will look bad on code coverage reports.

This assembly provides the `Map` feature that takes an expression and a dictionary of actions to perform for each value. The `Map` method will assert that the dictionary covers all possible keys and will ensure full code coverage.

It comes in different overloads:

### `TValue Map<TEnumKey, TValue>(TEnumKey expression, Dictionary<TEnumKey, TValue> dictionary)`

This method handles conversions that are already computed by the caller, such as conversion from one enum to another.

### `TValue Map<TEnumKey, TValue>(TEnumKey expression, Dictionary<TEnumKey, Func<TValue>> dictionary)`

This method handles conversions that not already computed, and will only call the function corresponding to the `expression` key.

### `void Map<TEnumKey>(TEnumKey expression, Dictionary<TEnumKey, Action> dictionary`

This method will only call the action corresponding to the matching `expression` key.

## Recommended usage

## When to use ``RequireNotNull`` (and ``Require``) vs ``AssertNotNull``

`Contract.RequireNotNull` should be used for arguments. In release mode it throws `ArgumentNullException`. To check the object state for null references when entering the method, use `Contract.Require(... is not null)`.

`Contract.AssertNotNull` should be used once the state has been validated and some action performed, to validate intermediate states. In release mode it throws `BrokenContractException`. If the null check is for the method exit state (like the returned value), prefer `Contract.Ensure(... is not null)`.

## When to use ``Require`` (or ``Ensure``) vs ``Assert``

`Contract.Require` and `Contract.Ensure` should be used to check the object state before (and after, respectively) the method is executed.

`Contract.Assert` should be used once the state has been validated and some action performed, to validate intermediate states.
