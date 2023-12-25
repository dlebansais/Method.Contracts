namespace Contracts.Test;

using System;
using System.Diagnostics;
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
    public void TestFailure()
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
}
