namespace Contracts.Test;

#if DEBUG
using System.Diagnostics;
#endif
using System.IO;
using NUnit.Framework;

[TestFixture]
internal class TestRequireNotNull
{
    [Test]
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

    [Test]
    public void TestNullReferenceFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NullString = null;
        Contract.RequireNotNull<string>(NullString, out _);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        const string? NullString = null;
        Assert.Throws<System.ArgumentNullException>(() => Contract.RequireNotNull<string>(NullString, out _));
#endif
    }

    [Test]
    public void TestWrongTypeFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string TestString = "test";
        Contract.RequireNotNull<Stream>(TestString, out _);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        const string TestString = "test";
        Assert.Throws<System.ArgumentException>(() => Contract.RequireNotNull<Stream>(TestString, out _));
#endif
    }

    [Test]
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

    [Test]
    public void TestDisposableFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const DebugTraceListener? TestListener = null;
        _ = Contract.RequireNotNull(TestListener);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        const DebugTraceListener? TestListener = null;
        Assert.Throws<System.ArgumentNullException>(() => _ = Contract.RequireNotNull(TestListener));
#endif
    }
}
