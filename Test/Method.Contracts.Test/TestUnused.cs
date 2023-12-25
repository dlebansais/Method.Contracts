namespace Contracts.Test;

using NUnit.Framework;

[TestFixture]
public class TestUnused
{
    [Test]
    public void Test()
    {
        Contract.Unused(out string UnusedString);

        Assert.That(UnusedString, Is.Null);
    }
}
