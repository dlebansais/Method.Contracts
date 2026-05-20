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

        Contract.AssertNoThrow(new Action(Throwing)); int lineNumber = DebugTraceListener.LineNumber();

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Unexpected exception, line {lineNumber}"));
        Assert.That(Listener.IsExceptionMessage, Is.True);
#else
        BrokenContractException Exception = Assert.Throws<BrokenContractException>(() => Contract.AssertNoThrow(Throwing)); int lineNumber = DebugTraceListener.LineNumber();

        Assert.That(Exception.Message, Is.EqualTo($"Unexpected exception, line {lineNumber}"));
#endif
    }

    private static void Throwing() => throw new InvalidOperationException();

    [TestCase(TestName = "AssertNoThrow(Action) with null action")]
    public void TestActionNullFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const Action NullAction = null!;
        Contract.AssertNoThrow(NullAction); int lineNumber = DebugTraceListener.LineNumber(); const string text = "NullAction";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Unexpected null value, line {lineNumber}: {text}"));
        Assert.That(Listener.IsExceptionMessage, Is.False);
#else
        const Action NullAction = null!;
        BrokenContractException Exception = Assert.Throws<BrokenContractException>(() => Contract.AssertNoThrow(NullAction)); int lineNumber = DebugTraceListener.LineNumber(); const string text = "NullAction";

        Assert.That(Exception.Message, Is.EqualTo($"Unexpected null value, line {lineNumber}: {text}"));
#endif
    }

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

        _ = Contract.AssertNoThrow(() => Throwing(TestResult)); int lineNumber = DebugTraceListener.LineNumber();

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Unexpected exception, line {lineNumber}"));
        Assert.That(Listener.IsExceptionMessage, Is.True);
#else
        BrokenContractException Exception = Assert.Throws<BrokenContractException>(() => _ = Contract.AssertNoThrow(() => Throwing(TestResult))); int lineNumber = DebugTraceListener.LineNumber();

        Assert.That(Exception.Message, Is.EqualTo($"Unexpected exception, line {lineNumber}"));
#endif
    }

    private static string Throwing(string value) => throw new ArgumentNullException(nameof(value));

    [TestCase(TestName = "AssertNoThrow(Func) failure with null function")]
    public void TestFunctionNullFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const Func<string> NullFunction = null!;
        _ = Contract.AssertNoThrow(NullFunction); int lineNumber = DebugTraceListener.LineNumber(); const string text = "NullFunction";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Unexpected null value, line {lineNumber}: {text}"));
        Assert.That(Listener.IsExceptionMessage, Is.False);
#else
        const Func<string> NullFunction = null!;
        BrokenContractException Exception = Assert.Throws<BrokenContractException>(() => _ = Contract.AssertNoThrow(NullFunction)); int lineNumber = DebugTraceListener.LineNumber(); const string text = "NullFunction";

        Assert.That(Exception.Message, Is.EqualTo($"Unexpected null value, line {lineNumber}: {text}"));
#endif
    }
}
