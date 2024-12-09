namespace Contracts.Test;

using System;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif
using NUnit.Framework;

[TestFixture]
internal class TestMapNotComputed
{
    private enum TestEnum
    {
        None = 0,
        Some = 1,
        More = 2,
    }

    [Test]
    public void TestSuccess()
    {
        const int NoneValue = 10;
        Dictionary<TestEnum, Func<int>> Dictionary = new()
        {
            { TestEnum.None, () => NoneValue },
            { TestEnum.Some, () => 20 },
            { TestEnum.More, () => 30 },
        };
        int Result;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Result = Contract.Map(TestEnum.None, Dictionary);

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrow(() => Result = Contract.Map(TestEnum.None, Dictionary));
#endif
        Assert.That(Result, Is.EqualTo(NoneValue));
    }

    [Test]
    public void TestFailureBadValue()
    {
        Dictionary<TestEnum, Func<int>> Dictionary = new()
        {
            { TestEnum.None, () => 10 },
            { TestEnum.Some, () => 20 },
            { TestEnum.More, () => 30 },
        };
        int Result;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Result = Contract.Map((TestEnum)int.MaxValue, Dictionary);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<BrokenContractException>(() => Contract.Map(TestEnum.None, Dictionary));
#endif
    }

    [Test]
    public void TestFailureBadDictionary()
    {
        Dictionary<TestEnum, Func<int>> Dictionary = new()
        {
            { TestEnum.Some, () => 20 },
            { TestEnum.More, () => 30 },
        };
        int Result;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Result = Contract.Map(TestEnum.More, Dictionary);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<BrokenContractException>(() => Contract.Map(TestEnum.More, Dictionary));
#endif
    }

    [Test]
    public void TestNullDictionary()
    {
        const Dictionary<TestEnum, Func<int>> Dictionary = null!;
        int Result;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Result = Contract.Map(TestEnum.None, Dictionary);

        Assert.That(Listener.IsAssertTriggered, Is.True);
#else
        Assert.Throws<System.ArgumentNullException>(() => Contract.Map(TestEnum.None, Dictionary));
#endif
    }
}
