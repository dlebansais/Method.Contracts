namespace Contracts.Test;

using System;
using NUnit.Framework;

[TestFixture]
internal class TestBrokenContractException
{
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
        using (Assert.EnterMultipleScope())
        {
            Assert.That(TestException.Message, Is.EqualTo(TestMessage));
            Assert.That(TestException.InnerException, Is.TypeOf<InvalidOperationException>());
        }

        InvalidOperationException? InnerException = TestException.InnerException as InvalidOperationException;
        Assert.That(InnerException, Is.Not.Null);

        string Message = InnerException!.Message;
        Assert.That(Message, Is.EqualTo(TestMessage));
    }
}
