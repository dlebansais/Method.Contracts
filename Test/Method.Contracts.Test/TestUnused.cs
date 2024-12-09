namespace Contracts.Test;

using NUnit.Framework;

[TestFixture]
internal class TestUnused
{
    [TestCase(TestName = "Unused for a reference type")]
    public void TestClass()
    {
        Contract.Unused(out string UnusedString);

        Assert.That(UnusedString, Is.Null);
    }

    [TestCase(TestName = "Unused for a nullable value type")]
    public void TestNullable()
    {
        Contract.Unused(out int? UnusedInt);

        Assert.That(UnusedInt, Is.Null);
    }

    [TestCase(TestName = "Unused for a value type")]
    public void TestStruct()
    {
        Contract.Unused(out int UnusedInt);

        Assert.That(UnusedInt, Is.EqualTo(0));
    }
}
