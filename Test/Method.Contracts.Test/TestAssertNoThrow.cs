namespace Contracts.Test;

using System;
#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestAssertNoThrow
{
    [TestCase(TestName = "AssertNoThrow(Action) success")]
    public void TestActionSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.AssertNoThrow(new Action(NotThrowing));

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrow(() => Contract.AssertNoThrow(NotThrowing));
#endif
    }

    private static void NotThrowing()
    {
    }

    [TestCase(TestName = "AssertNoThrow(Action) failure")]
    public void TestActionFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.AssertNoThrow(new Action(Throwing));

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<BrokenContractException>(() => Contract.AssertNoThrow(Throwing));
#endif
    }

    private static void Throwing() => throw new InvalidOperationException();

    [TestCase(TestName = "AssertNoThrow(Func) success")]
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

    private static string NotThrowing(string value) => value;

    [TestCase(TestName = "AssertNoThrow(Func) failure")]
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

    private static string Throwing(string value) => throw new ArgumentNullException(nameof(value));
}
