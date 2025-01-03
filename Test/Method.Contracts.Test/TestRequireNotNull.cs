namespace Contracts.Test;

#if DEBUG
using System.Diagnostics;
#endif
using System.IO;
using NUnit.Framework;

[TestFixture]
internal class TestRequireNotNull
{
    [TestCase(TestName = "RequireNotNull success")]
    public void TestSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NotNullString = "Not null";
        Contract.RequireNotNull(NotNullString, out string Result);

        Assert.That(Listener.IsAssertTriggered, Is.False);
        Assert.That(Result, Is.EqualTo(NotNullString));
#else
        const string? NotNullString = "Not null";
        Contract.RequireNotNull(NotNullString, out string Result);

        Assert.That(Result, Is.EqualTo(NotNullString));
#endif
    }

    [TestCase(TestName = "RequireNotNull failure with null reference")]
    public void TestNullReferenceFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NullString = null;
        Contract.RequireNotNull<string>(NullString, out _); int lineNumber = DebugTraceListener.LineNumber(); const string text = "NullString";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Unexpected null value, line {lineNumber}: {text}"));
#else
        const string? NullString = null;
        Assert.Throws<System.ArgumentNullException>(() => Contract.RequireNotNull<string>(NullString, out _));
#endif
    }

    [TestCase(TestName = "RequireNotNull failure with wrong type")]
    public void TestWrongTypeFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string TestString = "test";
        Contract.RequireNotNull<Stream>(TestString, out _); int lineNumber = DebugTraceListener.LineNumber();

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Invalid argument type, expected 'System.IO.Stream', got 'System.String', line {lineNumber}"));
#else
        const string TestString = "test";
        Assert.Throws<System.ArgumentException>(() => Contract.RequireNotNull<Stream>(TestString, out _));
#endif
    }

    [TestCase(TestName = "RequireNotNull success (with disposable)")]
    public void TestDisposableSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        using DebugTraceListener TestListener = new();
        DebugTraceListener Result = Contract.RequireNotNull(TestListener);

        Assert.That(Listener.IsAssertTriggered, Is.False);
        Assert.That(Result, Is.EqualTo(TestListener));
#else
        using DebugTraceListener TestListener = new();
        DebugTraceListener Result = Contract.RequireNotNull(TestListener);

        Assert.That(Result, Is.EqualTo(TestListener));
#endif
    }

    [TestCase(TestName = "RequireNotNull failure (with disposable)")]
    public void TestDisposableFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const DebugTraceListener? TestListener = null;

        _ = Contract.RequireNotNull(TestListener); int lineNumber = DebugTraceListener.LineNumber(); const string text = "TestListener";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Invalid null argument '{text}', line {lineNumber}"));
#else
        const DebugTraceListener? TestListener = null;
        Assert.Throws<System.ArgumentNullException>(() => _ = Contract.RequireNotNull(TestListener));
#endif
    }
}
