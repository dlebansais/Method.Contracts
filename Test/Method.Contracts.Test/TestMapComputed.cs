namespace Contracts.Test;

using System.Collections.Generic;
#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestMapComputed
{
    private enum TestEnum
    {
        None = 0,
        Some = 1,
        More = 2,
    }

    [TestCase(TestName = "Map success (direct conversion)")]
    public void TestSuccess()
    {
        const int NoneValue = 10;
        Dictionary<TestEnum, int> Dictionary = new()
        {
            { TestEnum.None, NoneValue },
            { TestEnum.Some, 20 },
            { TestEnum.More, 30 },
        };
        int Result = 0;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Result = Contract.Map(TestEnum.None, Dictionary);

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrow(() => { Result = Contract.Map(TestEnum.None, Dictionary); });
#endif
        Assert.That(Result, Is.EqualTo(NoneValue));
    }

    [TestCase(TestName = "Map failure with bad value (direct conversion)")]
    public void TestFailureBadValue()
    {
        Dictionary<TestEnum, int> Dictionary = new()
        {
            { TestEnum.None, 10 },
            { TestEnum.Some, 20 },
            { TestEnum.More, 30 },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        _ = Contract.Map((TestEnum)int.MaxValue, Dictionary);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<BrokenContractException>(() => { _ = Contract.Map((TestEnum)int.MaxValue, Dictionary); });
#endif
    }

    [TestCase(TestName = "Map failure with bad dictionary (direct conversion)")]
    public void TestFailureBadDictionary()
    {
        Dictionary<TestEnum, int> Dictionary = new()
        {
            { TestEnum.Some, 20 },
            { TestEnum.More, 30 },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        _ = Contract.Map(TestEnum.More, Dictionary);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<BrokenContractException>(() => { _ = Contract.Map(TestEnum.More, Dictionary); });
#endif
    }

    [TestCase(TestName = "Map failure with null dictionary (direct conversion)")]
    public void TestNullDictionary()
    {
        const Dictionary<TestEnum, int> Dictionary = null!;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        _ = Contract.Map(TestEnum.None, Dictionary);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<System.ArgumentNullException>(() => { _ = Contract.Map(TestEnum.None, Dictionary); });
#endif
    }
}
