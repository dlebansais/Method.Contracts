﻿namespace Contracts.Test;

#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestAssertNotNull
{
    [TestCase(TestName = "AssertNotNull success")]
    public void TestSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NotNullString = "Not null";
        string Result = Contract.AssertNotNull(NotNullString);

        Assert.That(Listener.IsAssertTriggered, Is.False);
        Assert.That(Result, Is.EqualTo(NotNullString));
#else
        const string? NotNullString = "Not null";
        string Result = Contract.AssertNotNull(NotNullString);

        Assert.That(Result, Is.EqualTo(NotNullString));
#endif
    }

    [TestCase(TestName = "AssertNotNull failure")]
    public void TestFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NullString = null;
        _ = Contract.AssertNotNull(NullString);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        const string? NullString = null;
        Assert.Throws<BrokenContractException>(() => _ = Contract.AssertNotNull(NullString));
#endif
    }
}
