namespace Contracts.Test;

using System;
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

    [Test]
    public void TestConstructorWithMessage()
    {
        const string TestMessage = "Test message";
        BrokenContractException TestException = new(TestMessage);
        Assert.That(TestException.Message, Is.EqualTo(TestMessage));
    }

    [Test]
    public void TestConstructorWithMessageAndInnerException()
    {
        const string TestMessage = "Test message";
        InvalidOperationException TestInnerException = new(TestMessage);
        BrokenContractException TestException = new(TestMessage, TestInnerException);
        Assert.That(TestException.Message, Is.EqualTo(TestMessage));
        Assert.That(TestException.InnerException, Is.TypeOf<InvalidOperationException>());
        Assert.That(TestException.InnerException.Message, Is.EqualTo(TestMessage));
    }
}
