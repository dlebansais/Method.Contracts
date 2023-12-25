namespace Contracts.Test;

using System;
using System.Diagnostics;
using NUnit.Framework;

[TestFixture]
public class TestNullSuppressed
{
    [Test]
    public void TestSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NotNullString = "Not null";
        string Result = Contract.NullSupressed(NotNullString);

        Assert.That(Listener.IsAssertTriggered, Is.False);
        Assert.That(Result, Is.EqualTo(NotNullString));
#else
        const string? NotNullString = "Not null";
        string Result = Contract.NullSupressed(NotNullString);

        Assert.That(Result, Is.EqualTo(NotNullString));
#endif
    }

    [Test]
    public void TestFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NullString = null;
        _ = Contract.NullSupressed(NullString);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        const string? NullString = null;
        Assert.Throws<InvalidOperationException>(() => _ = Contract.NullSupressed(NullString));
#endif
    }
}
