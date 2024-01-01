namespace Contracts.Test;

using System;
using System.Diagnostics;
using NUnit.Framework;

[TestFixture]
public class TestAssertNoThrow
{
    [Test]
    public void TestActionSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.AssertNoThrow(NotThrowing);

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrow(() => Contract.AssertNoThrow(NotThrowing));
#endif
    }

    private static void NotThrowing()
    {
    }

    [Test]
    public void TestActionFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.AssertNoThrow(Throwing);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<BrokenContractException>(() => Contract.AssertNoThrow(Throwing));
#endif
    }

    private static void Throwing()
    {
        throw new InvalidOperationException();
    }

    [Test]
    public void TestInvalidAction()
    {
        const Action NullAction = null!;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.AssertNoThrow(NullAction);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<ArgumentNullException>(() => Contract.AssertNoThrow(NullAction));
#endif
    }

    [Test]
    public void TestFunctionSuccess()
    {
        const string TestResult = "All good";
        string Result;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Result = Contract.AssertNoThrow(() => NotThrowing(TestResult));

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Result = Contract.AssertNoThrow(() => NotThrowing(TestResult));
#endif

        Assert.That(Result, Is.EqualTo(TestResult));
    }

    private static string NotThrowing(string value)
    {
        return value;
    }

    [Test]
    public void TestFunctionFailure()
    {
        const string TestResult = "Failed";

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        _ = Contract.AssertNoThrow(() => Throwing(TestResult));

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<BrokenContractException>(() => _ = Contract.AssertNoThrow(() => Throwing(TestResult)));
#endif
    }

    private static string Throwing(string value)
    {
        throw new ArgumentNullException(nameof(value));
    }

    [Test]
    public void TestInvalidFunction()
    {
        const Func<string> NullFunction = null!;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        _ = Contract.AssertNoThrow(NullFunction);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<ArgumentNullException>(() => _ = Contract.AssertNoThrow(NullFunction));
#endif
    }
}
