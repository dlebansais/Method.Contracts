namespace Contracts.Test;

#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestEnsure
{
    [TestCase(TestName = "Ensure success")]
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

    [TestCase(TestName = "Ensure failure")]
    public void TestFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Ensure(false); int lineNumber = DebugTraceListener.LineNumber(); const string text = "false";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Postcondition failed, line {lineNumber}: {text}"));
#else
        Assert.Throws<BrokenContractException>(() => Contract.Ensure(false));
#endif
    }
}
