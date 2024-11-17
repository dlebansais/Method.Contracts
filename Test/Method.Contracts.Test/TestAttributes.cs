namespace Contracts.Test;

using System.Linq;
using NUnit.Framework;

[TestFixture]
internal class TestAttributes
{
    [Test]
    public void TestAccessAttribute()
    {
        AccessAttribute Access1 = new("public");
        Assert.That(Access1.Specifiers.Count, Is.EqualTo(1));

        AccessAttribute Access2 = new("public", "virtual");
        Assert.That(Access2.Specifiers.Count, Is.EqualTo(2));

        AccessAttribute Access3 = new("public", "virtual", "unchecked");
        Assert.That(Access3.Specifiers.Count, Is.EqualTo(3));

        AccessAttribute Access4 = new("public", "virtual", "unchecked", "partial");
        Assert.That(Access4.Specifiers.Count, Is.EqualTo(4));

        AccessAttribute Access5 = new("private", "protected", "virtual", "unchecked", "partial");
        Assert.That(Access5.Specifiers.Count, Is.EqualTo(5));
    }

    [Test]
    public void TestRequireNotNullAttribute()
    {
        RequireNotNullAttribute RequireNotNull = new("text");
        Assert.That(RequireNotNull.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNull.Type, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNull.Name, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNull.AliasName, Is.EqualTo(string.Empty));

        const string Type = "Type";
        const string Name = "Name";
        const string AliasName = "AliasName";

        RequireNotNullAttribute RequireNotNullWithType = new("text") { Type = Type };
        Assert.That(RequireNotNullWithType.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithType.Type, Is.EqualTo(Type));
        Assert.That(RequireNotNullWithType.Name, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNullWithType.AliasName, Is.EqualTo(string.Empty));

        RequireNotNullAttribute RequireNotNullWithName = new("text") { Name = Name };
        Assert.That(RequireNotNullWithName.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithName.Type, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNullWithName.Name, Is.EqualTo(Name));
        Assert.That(RequireNotNullWithName.AliasName, Is.EqualTo(string.Empty));

        RequireNotNullAttribute RequireNotNullWithAliasName = new("text") { AliasName = AliasName };
        Assert.That(RequireNotNullWithAliasName.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithAliasName.Type, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNullWithAliasName.Name, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNullWithAliasName.AliasName, Is.EqualTo(AliasName));

        RequireNotNullAttribute RequireNotNullWithAliasTypeAndName = new("text") { Type = Type, Name = Name, AliasName = AliasName };
        Assert.That(RequireNotNullWithAliasTypeAndName.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithAliasTypeAndName.Type, Is.EqualTo(Type));
        Assert.That(RequireNotNullWithAliasTypeAndName.Name, Is.EqualTo(Name));
        Assert.That(RequireNotNullWithAliasTypeAndName.AliasName, Is.EqualTo(AliasName));

        RequireNotNullAttribute RequireNotNull2 = new("text1", "text2");
        Assert.That(RequireNotNull2.ArgumentNames.Count, Is.EqualTo(2));
        Assert.That(RequireNotNull2.Type, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNull2.Name, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNull2.AliasName, Is.EqualTo(string.Empty));
    }

    [Test]
    public void TestRequireAttribute()
    {
        RequireAttribute Require = new("true");
        Assert.That(Require.Expressions.Count, Is.EqualTo(1));
        Assert.That(Require.DebugOnly, Is.False);

        RequireAttribute RequireDebugOnly = new("true") { DebugOnly = true };
        Assert.That(RequireDebugOnly.Expressions.Count, Is.EqualTo(1));
        Assert.That(RequireDebugOnly.DebugOnly, Is.True);

        RequireAttribute Require2 = new("true", "true");
        Assert.That(Require2.Expressions.Count, Is.EqualTo(2));
        Assert.That(Require2.DebugOnly, Is.False);
    }

    [Test]
    public void TestEnsureAttribute()
    {
        EnsureAttribute Ensure = new("true");
        Assert.That(Ensure.Expressions.Count, Is.EqualTo(1));
        Assert.That(Ensure.DebugOnly, Is.False);

        EnsureAttribute EnsureDebugOnly = new("true") { DebugOnly = true };
        Assert.That(EnsureDebugOnly.Expressions.Count, Is.EqualTo(1));
        Assert.That(EnsureDebugOnly.DebugOnly, Is.True);

        EnsureAttribute Ensure2 = new("true", "true");
        Assert.That(Ensure2.Expressions.Count, Is.EqualTo(2));
        Assert.That(Ensure2.DebugOnly, Is.False);
    }

    [Test]
    public void TestInitializeWithAttribute()
    {
        InitializeWithAttribute InitializeWith = new("text");
        Assert.That(InitializeWith.MethodName, Is.EqualTo("text"));
    }
}
