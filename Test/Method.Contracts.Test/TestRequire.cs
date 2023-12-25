namespace Contracts.Test;

using System;
using System.Diagnostics;
using NUnit.Framework;

[TestFixture]
public class TestRequire
{
    [Test]
    public void TestSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Require(true);

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrow(() => Contract.Require(true));
#endif
    }

    [Test]
    public void TestFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Require(false);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<ArgumentException>(() => Contract.Require(false));
#endif
    }
}
