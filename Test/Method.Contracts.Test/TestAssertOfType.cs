namespace Contracts.Test;

#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestAssertOfType
{
    [TestCase(TestName = "AssertOfType success")]
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

    [TestCase(TestName = "AssertOfType success")]
    public void TestFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        object? NotString = 0;
        _ = Contract.AssertOfType<string>(NotString); int lineNumber = DebugTraceListener.LineNumber(); const string text = "NotString";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Expected type 'System.String' for value: {text}, line {lineNumber}"));
#else
        object? NotString = 0;
        Assert.Throws<BrokenContractException>(() => _ = Contract.AssertOfType<string>(NotString));
#endif
    }

    [TestCase(TestName = "AssertOfType failure with null value")]
    public void TestNullFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        const string? NullString = null;
        _ = Contract.AssertOfType<string>(NullString); int lineNumber = DebugTraceListener.LineNumber(); const string text = "NullString";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Unexpected null value, line {lineNumber}: {text}"));
#else
        const string? NullString = null;
        Assert.Throws<BrokenContractException>(() => _ = Contract.AssertOfType<string>(NullString));
#endif
    }
}
