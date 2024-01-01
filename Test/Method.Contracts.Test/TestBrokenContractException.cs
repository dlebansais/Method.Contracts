namespace Contracts.Test;

using System.Text.Json;
using NUnit.Framework;

[TestFixture]
public class TestBrokenContractException
{
    [Test]
    public void TestSerializable()
    {
        BrokenContractException TestException = new();

        string SerializedException = JsonSerializer.Serialize(TestException);
        BrokenContractException? DeserializedException = JsonSerializer.Deserialize<BrokenContractException>(SerializedException);

        Assert.That(DeserializedException, Is.EqualTo(TestException));
    }
}
