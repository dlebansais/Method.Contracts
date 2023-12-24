namespace Contracts.Test;

using System;
using System.Diagnostics;
using NUnit.Framework;

[TestFixture]
public class TestContracts
{
    [Test]
    public void TestAssertNotNull()
    {
#if DEBUG
        TestAssertNotNull_Debug();
#else
        TestAssertNotNull_Release();
#endif
    }

    private static void TestAssertNotNull_Debug()
    {
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NullString = null;
        Contract.RequireNotNull<string>(NullString, out _);

        Assert.That(Listener.IsAssertTriggered, Is.True);
    }

    private static void TestAssertNotNull_Release()
    {
        const string? NullString = null;
        Assert.Throws<ArgumentNullException>(() => Contract.RequireNotNull<string>(NullString, out _));
    }
}
