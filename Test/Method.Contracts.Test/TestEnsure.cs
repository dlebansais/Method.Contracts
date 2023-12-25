namespace Contracts.Test;

using System;
using System.Diagnostics;
using NUnit.Framework;

[TestFixture]
public class TestEnsure
{
    [Test]
    public void Test()
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
