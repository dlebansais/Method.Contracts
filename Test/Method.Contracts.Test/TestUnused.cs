namespace Contracts.Test;

using NUnit.Framework;

[TestFixture]
internal class TestUnused
{
    [Test]
    public void TestClass()
    {
        Contract.Unused(out string UnusedString);

        Assert.That(UnusedString, Is.Null);
    }

    [Test]
    public void TestNullable()
    {
        Contract.Unused(out int? UnusedInt);

        Assert.That(UnusedInt, Is.Null);
    }

    [Test]
    public void TestStruct()
    {
        Contract.Unused(out int UnusedInt);

        Assert.That(UnusedInt, Is.EqualTo(0));
    }
}
