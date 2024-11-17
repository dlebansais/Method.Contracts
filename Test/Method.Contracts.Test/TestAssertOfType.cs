namespace Contracts.Test;

#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestAssertOfType
{
    [Test]
    public void TestSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        object? NotNullString = "Not null";
        string Result = Contract.AssertOfType<string>(NotNullString);

        Assert.That(Listener.IsAssertTriggered, Is.False);
        Assert.That(Result, Is.EqualTo(NotNullString));
#else
        object? NotNullString = "Not null";
        string Result = Contract.AssertOfType<string>(NotNullString);

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

        object? NotString = 0;
        _ = Contract.AssertOfType<string>(NotString);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        object? NotString = 0;
        Assert.Throws<BrokenContractException>(() => _ = Contract.AssertOfType<string>(NotString));
#endif
    }

    [Test]
    public void TestNullFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NullString = null;
        _ = Contract.AssertOfType<string>(NullString);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        const string? NullString = null;
        Assert.Throws<BrokenContractException>(() => _ = Contract.AssertOfType<string>(NullString));
#endif
    }
}
