namespace Contracts.Test;

using System.Linq;
using NUnit.Framework;

[TestFixture]
public class TestAttributes
{
    [Test]
    public void TestAccessAttribute()
    {
        var Access1 = new AccessAttribute("public");
        Assert.That(Access1.Specifiers.Count, Is.EqualTo(1));

        var Access2 = new AccessAttribute("public", "virtual");
        Assert.That(Access2.Specifiers.Count, Is.EqualTo(2));

        var Access3 = new AccessAttribute("public", "virtual", "unchecked");
        Assert.That(Access3.Specifiers.Count, Is.EqualTo(3));

        var Access4 = new AccessAttribute("public", "virtual", "unchecked", "partial");
        Assert.That(Access4.Specifiers.Count, Is.EqualTo(4));

        var Access5 = new AccessAttribute("private", "protected", "virtual", "unchecked", "partial");
        Assert.That(Access5.Specifiers.Count, Is.EqualTo(5));
    }

    [Test]
    public void TestRequireNotNullAttribute()
    {
        var RequireNotNull = new RequireNotNullAttribute("text");
        Assert.That(RequireNotNull.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNull.Type, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNull.Name, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNull.AliasName, Is.EqualTo(string.Empty));

        const string Type = "Type";
        const string Name = "Name";
        const string AliasName = "AliasName";

        var RequireNotNullWithType = new RequireNotNullAttribute("text") { Type = Type };
        Assert.That(RequireNotNullWithType.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithType.Type, Is.EqualTo(Type));
        Assert.That(RequireNotNullWithType.Name, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNullWithType.AliasName, Is.EqualTo(string.Empty));

        var RequireNotNullWithName = new RequireNotNullAttribute("text") { Name = Name };
        Assert.That(RequireNotNullWithName.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithName.Type, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNullWithName.Name, Is.EqualTo(Name));
        Assert.That(RequireNotNullWithName.AliasName, Is.EqualTo(string.Empty));

        var RequireNotNullWithAliasName = new RequireNotNullAttribute("text") { AliasName = AliasName };
        Assert.That(RequireNotNullWithAliasName.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithAliasName.Type, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNullWithAliasName.Name, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNullWithAliasName.AliasName, Is.EqualTo(AliasName));

        var RequireNotNullWithAliasTypeAndName = new RequireNotNullAttribute("text") { Type = Type, Name = Name, AliasName = AliasName };
        Assert.That(RequireNotNullWithAliasTypeAndName.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithAliasTypeAndName.Type, Is.EqualTo(Type));
        Assert.That(RequireNotNullWithAliasTypeAndName.Name, Is.EqualTo(Name));
        Assert.That(RequireNotNullWithAliasTypeAndName.AliasName, Is.EqualTo(AliasName));

        var RequireNotNull2 = new RequireNotNullAttribute("text1", "text2");
        Assert.That(RequireNotNull2.ArgumentNames.Count, Is.EqualTo(2));
        Assert.That(RequireNotNull2.Type, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNull2.Name, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNull2.AliasName, Is.EqualTo(string.Empty));

    }

    [Test]
    public void TestRequireAttribute()
    {
        var Require = new RequireAttribute("true");
        Assert.That(Require.Expressions.Count, Is.EqualTo(1));
        Assert.That(Require.DebugOnly, Is.False);

        var RequireDebugOnly = new RequireAttribute("true") { DebugOnly = true };
        Assert.That(RequireDebugOnly.Expressions.Count, Is.EqualTo(1));
        Assert.That(RequireDebugOnly.DebugOnly, Is.True);

        var Require2 = new RequireAttribute("true", "true");
        Assert.That(Require2.Expressions.Count, Is.EqualTo(2));
        Assert.That(Require2.DebugOnly, Is.False);

    }

    [Test]
    public void TestEnsureAttribute()
    {
        var Ensure = new EnsureAttribute("true");
        Assert.That(Ensure.Expressions.Count, Is.EqualTo(1));
        Assert.That(Ensure.DebugOnly, Is.False);

        var EnsureDebugOnly = new EnsureAttribute("true") { DebugOnly = true };
        Assert.That(EnsureDebugOnly.Expressions.Count, Is.EqualTo(1));
        Assert.That(EnsureDebugOnly.DebugOnly, Is.True);

        var Ensure2 = new EnsureAttribute("true", "true");
        Assert.That(Ensure2.Expressions.Count, Is.EqualTo(2));
        Assert.That(Ensure2.DebugOnly, Is.False);
    }
}
