namespace Contracts.Test;

#if !DEBUG
using System;
#endif
#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestRequire
{
    [TestCase(TestName = "Require success")]
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

    [TestCase(TestName = "Require failure")]
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
