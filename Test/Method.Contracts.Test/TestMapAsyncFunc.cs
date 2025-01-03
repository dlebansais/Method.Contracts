namespace Contracts.Test;

using System;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
internal class TestMapAsyncFunc
{
    private enum TestEnum
    {
        None = 0,
        Some = 1,
        More = 2,
    }

    [TestCase(TestName = "Map success (function)")]
    public async Task TestSuccess()
    {
        const int NoneValue = 10;
        Dictionary<TestEnum, Func<Task<int>>> Dictionary = new()
        {
            { TestEnum.None, async () => await Task.Run(() => NoneValue).ConfigureAwait(false) },
            { TestEnum.Some, async () => await Task.Run(() => 20).ConfigureAwait(false) },
            { TestEnum.More, async () => await Task.Run(() => 30).ConfigureAwait(false) },
        };
        int Result = 0;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        Result = await Contract.MapAsync(TestEnum.None, Dictionary).ConfigureAwait(false);

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrowAsync(async () => { Result = await Contract.MapAsync(TestEnum.None, Dictionary).ConfigureAwait(false); });
#endif
        Assert.That(Result, Is.EqualTo(NoneValue));

        await Task.CompletedTask.ConfigureAwait(false);
    }

    [TestCase(TestName = "Map failure with bad value (function)")]
    public async Task TestFailureBadValue()
    {
        Dictionary<TestEnum, Func<Task<int>>> Dictionary = new()
        {
            { TestEnum.None, async () => await Task.Run(() => 10).ConfigureAwait(false) },
            { TestEnum.Some, async () => await Task.Run(() => 20).ConfigureAwait(false) },
            { TestEnum.More, async () => await Task.Run(() => 30).ConfigureAwait(false) },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        _ = await Contract.MapAsync((TestEnum)int.MaxValue, Dictionary).ConfigureAwait(false); int lineNumber = DebugTraceListener.LineNumber(); const string expressionText = "(TestEnum)int.MaxValue"; const int IntValue = int.MaxValue;

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Enum '{expressionText}' with value {IntValue} not in dictionary, line {lineNumber}"));
#else
        Assert.ThrowsAsync<BrokenContractException>(async () => { _ = await Contract.MapAsync((TestEnum)int.MaxValue, Dictionary).ConfigureAwait(false); });
#endif

        await Task.CompletedTask.ConfigureAwait(false);
    }

    [TestCase(TestName = "Map failure with bad dictionary (function)")]
    public async Task TestFailureBadDictionary()
    {
        Dictionary<TestEnum, Func<Task<int>>> Dictionary = new()
        {
            { TestEnum.Some, async () => await Task.Run(() => 20).ConfigureAwait(false) },
            { TestEnum.More, async () => await Task.Run(() => 30).ConfigureAwait(false) },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        _ = await Contract.MapAsync(TestEnum.More, Dictionary).ConfigureAwait(false); int lineNumber = DebugTraceListener.LineNumber(); const string dictionaryText = "Dictionary";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Invalid dictionary, line {lineNumber}: {dictionaryText}"));
#else
        Assert.ThrowsAsync<BrokenContractException>(async () => { _ = await Contract.MapAsync(TestEnum.More, Dictionary).ConfigureAwait(false); });
#endif

        await Task.CompletedTask.ConfigureAwait(false);
    }

    [TestCase(TestName = "Map failure with null dictionary (function)")]
    public async Task TestNullDictionary()
    {
        const Dictionary<TestEnum, Func<Task<int>>> Dictionary = null!;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        _ = await Contract.MapAsync(TestEnum.None, Dictionary).ConfigureAwait(false); int lineNumber = DebugTraceListener.LineNumber(); const string dictionaryText = "Dictionary";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Invalid null dictionary, line {lineNumber}: {dictionaryText}"));
#else
        Assert.ThrowsAsync<ArgumentNullException>(async () => { _ = await Contract.MapAsync(TestEnum.None, Dictionary).ConfigureAwait(false); });
#endif

        await Task.CompletedTask.ConfigureAwait(false);
    }
}
