namespace Contracts.Test;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

[TestFixture]
public class TestRequireNotNull
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
        Contract.RequireNotNull<string>(NotNullString, out string Result);

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
        Assert.Throws<ArgumentNullException>(() => Contract.RequireNotNull<string>(NullString, out _));
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
        Assert.Throws<ArgumentException>(() => Contract.RequireNotNull<Stream>(TestString, out _));
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
        Assert.Throws<ArgumentNullException>(() => _ = Contract.RequireNotNull(TestListener));
#endif
    }
}
