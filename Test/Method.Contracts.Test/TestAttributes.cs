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
    }

    [Test]
    public void TestRequireAttribute()
    {
        var Require = new RequireAttribute("true");
        Assert.That(Require.Expressions.Count, Is.EqualTo(1));
    }

    [Test]
    public void TestEnsureAttribute()
    {
        var Ensure = new EnsureAttribute("true");
        Assert.That(Ensure.Expressions.Count, Is.EqualTo(1));
    }
}
