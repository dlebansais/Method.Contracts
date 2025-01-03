namespace Contracts.Test;

#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestAssert
{
    [TestCase(TestName = "Assert successful")]
    public void TestSuccess()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Assert(true);

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrow(() => Contract.Assert(true));
#endif
    }

    [TestCase(TestName = "Assert failure")]
    public void TestFailure()
    {
#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Assert(false); int lineNumber = DebugTraceListener.LineNumber(); const string text = "false";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Assert failed, line {lineNumber}: {text}"));
#else
        Assert.Throws<BrokenContractException>(() => Contract.Assert(false));
#endif
    }
}
