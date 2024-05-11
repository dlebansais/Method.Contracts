namespace Contracts.Test;

using System.Linq;
using NUnit.Framework;

[TestFixture]
public class TestAttributes
{
    [Test]
    public void TestAccessAttribute()
    {
        var Access = new AccessAttribute("public");
        Assert.That(Access.Specifiers.Count, Is.EqualTo(1));
    }

    [Test]
    public void TestRequireNotNullAttribute()
    {
        var RequireNotNull = new RequireNotNullAttribute("text");
        Assert.That(RequireNotNull.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNull.AliasType, Is.EqualTo(string.Empty));
        Assert.That(RequireNotNull.AliasName, Is.EqualTo(string.Empty));

        const string AliasType = "AliasType";
        const string AliasName = "AliasName";

        var RequireNotNullWithAliasType = new RequireNotNullAttribute("text") { AliasType = AliasType };
        Assert.That(RequireNotNullWithAliasType.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithAliasType.AliasType, Is.EqualTo(AliasType));
        Assert.That(RequireNotNullWithAliasType.AliasName, Is.EqualTo(string.Empty));

        var RequireNotNullWithAliasName = new RequireNotNullAttribute("text") { AliasName = AliasName };
        Assert.That(RequireNotNullWithAliasName.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithAliasName.AliasName, Is.EqualTo(AliasName));
        Assert.That(RequireNotNullWithAliasName.AliasType, Is.EqualTo(string.Empty));

        var RequireNotNullWithAliasTypeAndName = new RequireNotNullAttribute("text") { AliasType = AliasType, AliasName = AliasName };
        Assert.That(RequireNotNullWithAliasTypeAndName.ArgumentNames.Count, Is.EqualTo(1));
        Assert.That(RequireNotNullWithAliasTypeAndName.AliasType, Is.EqualTo(AliasType));
        Assert.That(RequireNotNullWithAliasTypeAndName.AliasName, Is.EqualTo(AliasName));
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
    }

    [Test]
    public void TestEnsureAttribute()
    {
        var Ensure = new EnsureAttribute("true");
        Assert.That(Ensure.Expressions.Count, Is.EqualTo(1));

        var EnsureDebugOnly = new EnsureAttribute("true") { DebugOnly = true };
        Assert.That(EnsureDebugOnly.Expressions.Count, Is.EqualTo(1));
        Assert.That(EnsureDebugOnly.DebugOnly, Is.True);
    }
}
