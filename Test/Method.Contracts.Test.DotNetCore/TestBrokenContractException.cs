namespace Contracts.Test;

using System;
using System.Text.Json;
using NUnit.Framework;

[TestFixture]
internal class TestBrokenContractException
{
    [TestCase(TestName = "BrokenContractException serialization")]
    public void TestSerialization()
    {
        BrokenContractException TestException = new();

        string SerializedException = JsonSerializer.Serialize(TestException);
        BrokenContractException? DeserializedException = JsonSerializer.Deserialize<BrokenContractException>(SerializedException);

        Assert.That(DeserializedException, Is.Not.Null);

        string ReserializedException = JsonSerializer.Serialize(DeserializedException);

        Assert.That(ReserializedException, Is.EqualTo(SerializedException));
    }

    [TestCase(TestName = "BrokenContractException constructor with message")]
    public void TestConstructorWithMessage()
    {
        const string TestMessage = "Test message";
        BrokenContractException TestException = new(TestMessage);
        Assert.That(TestException.Message, Is.EqualTo(TestMessage));
    }

    [TestCase(TestName = "BrokenContractException constructor with message and inner exception")]
    public void TestConstructorWithMessageAndInnerException()
    {
        const string TestMessage = "Test message";
        InvalidOperationException TestInnerException = new(TestMessage);
        BrokenContractException TestException = new(TestMessage, TestInnerException);
        Assert.That(TestException.Message, Is.EqualTo(TestMessage));
        Assert.That(TestException.InnerException, Is.TypeOf<InvalidOperationException>());

        InvalidOperationException? InnerException = TestException.InnerException as InvalidOperationException;
        Assert.That(InnerException, Is.Not.Null);

        string Message = InnerException!.Message;
        Assert.That(Message, Is.EqualTo(TestMessage));
    }
}
