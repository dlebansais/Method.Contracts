# Contracts
A set of tools to enforce contracts in methods.

[![Build status](https://ci.appveyor.com/api/projects/status/i7n5qgflgtbvaj1n?svg=true)](https://ci.appveyor.com/project/dlebansais/contracts)

This assembly applies to projects using **C# 8 or higher** and with **Nullable** enabled.

## Usage

Add the assembly from the latest release as a dependency of your project. The `Contracts` namespace then becomes available.

    using Contracts;
    
### Contract.RequireNotNull

This static method can be used to check parameters and remove [warning CA1062](https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1062).

Consider the following code:

    public bool TryParseFoo(string text, out Foo parsedFoo)
    {
        if (text.Length > 0)
        {
			// ...
        }

		// ...

The line `if (text.Length > 0)` generates warning CA1062: *Validate arguments of public methods*. The traditional way of removing this warning is to check for the `null` value, as follow.

    public bool TryParseFoo(string text, out Foo parsedFoo)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));

        if (text.Length > 0)
        {
			// ...
        }

		// ...

You can replace this code with `RequireNotNull` instead:

    public bool TryParseFoo(string text, out Foo parsedFoo)
    {
        Contract.RequireNotNull(text, out string Text);

        if (Text.Length > 0)
        {
			// ...
        }

		// ...

Note how the new code uses `Text` with an upper case T and not the parameter anymore.

By using `RequireNotNull` you can slightly improve your code, at least from a point of view:

+ You can make the check take only one line and keep warnings about single-line statements active (see for instance [warning SA1502](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1502.md)).
+ You can make this contract easy to replace everywhere with search/replace once the corresponding feature is added to .NET, if it ever happen.
+ This check explicitely means you're declaring a code contract about your parameter.

The drawback of using `RequireNotNull` is, of course, that you introduce a new variable.

### Contract.Unused

The purpose of this method is mostly to annotate your code to specify that a variable value is not used, and remove warning CS8625:*C# Cannot convert null literal to non-nullable reference type*.

Consider the following code:

    public bool TryParseFoo(string text, out Foo parsedFoo)
    {
        if (text.Length > 0)
        {
            // ... obtain parsedFoo
            return true;
        }

        parsedFoo = null;
        return false;
    }

The line `parsedFoo = null;` generates warning CS8625. The traditional way of removing this warning is then to add the `!` null forgiving operator, as follow:

    parsedFoo = null!;
    return false;

You can replace this code with `Unused` instead:

    parsedFoo = Contract.Unused<Foo>();
    return false;

Note how the new code uses `Text` with an upper case T and not the parameter anymore.

By using `Unused` you can slightly improve your code, at least from a point of view:

+ The null forgiving operator is easily missed.
+ This check explicitely means you're declaring a code contract about your output.
 
# Certification

This program is digitally signed with a [CAcert](https://www.cacert.org/) certificate.
