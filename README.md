# Contracts
A set of tools to enforce contracts in methods.

[![Build status](https://ci.appveyor.com/api/projects/status/i7n5qgflgtbvaj1n?svg=true)](https://ci.appveyor.com/project/dlebansais/contracts) [![CodeFactor](https://www.codefactor.io/repository/github/dlebansais/contracts/badge)](https://www.codefactor.io/repository/github/dlebansais/contracts)

This assembly applies to projects using **C# 8 or higher** and with **Nullable** enabled.

## Usage

Add the assembly from the latest release as a dependency of your project. The `Contracts` namespace then becomes available.

````csharp
using Contracts;
````
    
### Contract.RequireNotNull

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
    if (text == null)
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
+ This check explicitely means you're declaring a code contract about your parameter.

The drawback of using `RequireNotNull` is, of course, that you introduce a new variable.

### Contract.Unused

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
+ This check explicitely means you're declaring a code contract about your output.

### Contract.DisposeOfUndefined

If you use `Contract.Unused(out ...)` you may get [warning CA1062](https://docs.microsoft.com/en-us/visualstudio/code-quality/ca2000): *Use recommended dispose pattern to ensure that object created by 'out ...' is disposed on all paths*.

Rather than turning the warning off, you can instead use `DisposeOfUndefined` to explicitely dispose of the object.

````csharp
	if (!TryParseFoo(string text, out IDisposable parsedFoo))
	{
		Contract.DisposeOfUndefined(parsedFoo);
	}
````