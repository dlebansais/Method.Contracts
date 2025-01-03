namespace Contracts.Test;

using System;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestMapAction
{
    private enum TestEnum
    {
        None = 0,
        Some = 1,
        More = 2,
    }

    [TestCase(TestName = "Map success (action)")]
    public void TestSuccess()
    {
        const int NoneValue = 10;
        int Result = 0;

        Dictionary<TestEnum, Action> Dictionary = new()
        {
            { TestEnum.None, () => Result = NoneValue },
            { TestEnum.Some, () => Result = 20 },
            { TestEnum.More, () => Result = 30 },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Map(TestEnum.None, Dictionary);

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrow(() => Contract.Map(TestEnum.None, Dictionary));
#endif
        Assert.That(Result, Is.EqualTo(NoneValue));
    }

    [TestCase(TestName = "Map failure with bad value (action)")]
    public void TestFailureBadValue()
    {
        int Result = 0;
        Dictionary<TestEnum, Action> Dictionary = new()
        {
            { TestEnum.None, () => Result = 10 },
            { TestEnum.Some, () => Result = 20 },
            { TestEnum.More, () => Result = 30 },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Map((TestEnum)int.MaxValue, Dictionary); int lineNumber = DebugTraceListener.LineNumber(); const string expressionText = "(TestEnum)int.MaxValue"; const int IntValue = int.MaxValue;

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Enum '{expressionText}' with value {IntValue} not in dictionary, line {lineNumber}"));
#else
        Assert.Throws<BrokenContractException>(() => Contract.Map((TestEnum)int.MaxValue, Dictionary));
#endif

        Assert.That(Result, Is.EqualTo(0));
    }

    [TestCase(TestName = "Map failure with bad dictionary (action)")]
    public void TestFailureBadDictionary()
    {
        int Result = 0;
        Dictionary<TestEnum, Action> Dictionary = new()
        {
            { TestEnum.Some, () => Result = 20 },
            { TestEnum.More, () => Result = 30 },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Map(TestEnum.More, Dictionary); int lineNumber = DebugTraceListener.LineNumber(); const string dictionaryText = "Dictionary";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Invalid dictionary, line {lineNumber}: {dictionaryText}"));
#else
        Assert.Throws<BrokenContractException>(() => Contract.Map(TestEnum.More, Dictionary));
#endif

        Assert.That(Result, Is.EqualTo(0));
    }

    [TestCase(TestName = "Map failure with null dictionary (action)")]
    public void TestNullDictionary()
    {
        const Dictionary<TestEnum, Action> Dictionary = null!;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Contract.Map(TestEnum.None, Dictionary); int lineNumber = DebugTraceListener.LineNumber(); const string dictionaryText = "Dictionary";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Invalid null dictionary, line {lineNumber}: {dictionaryText}"));
#else
        Assert.Throws<ArgumentNullException>(() => Contract.Map(TestEnum.None, Dictionary));
#endif
    }
}
