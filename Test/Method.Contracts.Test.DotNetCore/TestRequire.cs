namespace Contracts.Test;

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

        Contract.Require(false); int lineNumber = DebugTraceListener.LineNumber(); const string text = "false";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Requirement not met, line {lineNumber}: {text}"));
#else
        BrokenContractException Exception = Assert.Throws<BrokenContractException>(() => Contract.Require(false)); int lineNumber = DebugTraceListener.LineNumber(); const string text = "false";

        Assert.That(Exception.Message, Is.EqualTo($"Requirement not met, line {lineNumber}: {text}"));
#endif
    }
}
