namespace Contracts.Test;

using System;
using System.Diagnostics;
using NUnit.Framework;

[TestFixture]
public class TestEnsure
{
    [Test]
    public void TestSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Ensure(true);

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrow(() => Contract.Ensure(true));
#endif
    }

    [Test]
    public void TestFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Ensure(false);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<InvalidOperationException>(() => Contract.Ensure(false));
#endif
    }
}
