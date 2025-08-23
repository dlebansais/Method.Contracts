namespace Contracts.Test;

using System;
using System.Collections.Generic;

#if DEBUG
using System.Diagnostics;
#endif

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
internal class TestMapAsyncAction
{
    private enum TestEnum
    {
        None = 0,
        Some = 1,
        More = 2,
    }

    [TestCase(TestName = "Map success (async action)")]
    public async Task TestSuccess()
    {
        const int NoneValue = 10;
        int Result = 0;

        Dictionary<TestEnum, Func<Task>> Dictionary = new()
        {
            { TestEnum.None, async () => await Task.Run(() => { Result = NoneValue; }).ConfigureAwait(false) },
            { TestEnum.Some, async () => await Task.Run(() => { Result = 20; }).ConfigureAwait(false) },
            { TestEnum.More, async () => await Task.Run(() => { Result = 30; }).ConfigureAwait(false) },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        await Contract.MapAsync(TestEnum.None, Dictionary).ConfigureAwait(false);

        Assert.That(Listener.IsAssertTriggered, Is.False);
#else
        Assert.DoesNotThrowAsync(async () => await Contract.MapAsync(TestEnum.None, Dictionary).ConfigureAwait(false));
#endif
        Assert.That(Result, Is.EqualTo(NoneValue));

        await Task.CompletedTask.ConfigureAwait(false);
    }

    [TestCase(TestName = "Map failure with bad value (async action)")]
    public async Task TestFailureBadValue()
    {
        int Result = 0;
        Dictionary<TestEnum, Func<Task>> Dictionary = new()
        {
            { TestEnum.None, async () => await Task.Run(() => { Result = 10; }).ConfigureAwait(false) },
            { TestEnum.Some, async () => await Task.Run(() => { Result = 20; }).ConfigureAwait(false) },
            { TestEnum.More, async () => await Task.Run(() => { Result = 30; }).ConfigureAwait(false) },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        await Contract.MapAsync((TestEnum)int.MaxValue, Dictionary).ConfigureAwait(false); int lineNumber = DebugTraceListener.LineNumber(); const string expressionText = "(TestEnum)int.MaxValue"; const int IntValue = int.MaxValue;

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Enum '{expressionText}' with value {IntValue} not in dictionary, line {lineNumber}"));
#else
        BrokenContractException Exception = Assert.ThrowsAsync<BrokenContractException>(async () => await Contract.MapAsync((TestEnum)int.MaxValue, Dictionary).ConfigureAwait(false)); int lineNumber = DebugTraceListener.LineNumber(); const string expressionText = "(TestEnum)int.MaxValue"; const int IntValue = int.MaxValue;

        Assert.That(Exception.Message, Is.EqualTo($"Enum '{expressionText}' with value {IntValue} not in dictionary, line {lineNumber}"));
#endif

        Assert.That(Result, Is.Zero);

        await Task.CompletedTask.ConfigureAwait(false);
    }

    [TestCase(TestName = "Map failure with bad dictionary (async action)")]
    public async Task TestFailureBadDictionary()
    {
        int Result = 0;
        Dictionary<TestEnum, Func<Task>> Dictionary = new()
        {
            { TestEnum.Some, async () => await Task.Run(() => { Result = 20; }).ConfigureAwait(false) },
            { TestEnum.More, async () => await Task.Run(() => { Result = 30; }).ConfigureAwait(false) },
        };

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        await Contract.MapAsync(TestEnum.More, Dictionary).ConfigureAwait(false); int lineNumber = DebugTraceListener.LineNumber(); const string dictionaryText = "Dictionary";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Invalid dictionary, line {lineNumber}: {dictionaryText}"));
#else
        BrokenContractException Exception = Assert.ThrowsAsync<BrokenContractException>(async () => await Contract.MapAsync(TestEnum.More, Dictionary).ConfigureAwait(false)); int lineNumber = DebugTraceListener.LineNumber(); const string dictionaryText = "Dictionary";

        Assert.That(Exception.Message, Is.EqualTo($"Invalid dictionary, line {lineNumber}: {dictionaryText}"));
#endif

        Assert.That(Result, Is.Zero);

        await Task.CompletedTask.ConfigureAwait(false);
    }

    [TestCase(TestName = "Map failure with null dictionary (async action)")]
    public async Task TestNullDictionary()
    {
        const Dictionary<TestEnum, Func<Task>> Dictionary = null!;

#if DEBUG
        DebugTraceListener Listener = new();
        Trace.Listeners.Clear();
        Trace.Listeners.Add(Listener);

        await Contract.MapAsync(TestEnum.None, Dictionary).ConfigureAwait(false); int lineNumber = DebugTraceListener.LineNumber(); const string dictionaryText = "Dictionary";

        Assert.That(Listener.IsAssertTriggered, Is.True);
        Assert.That(Listener.IsOnlyOneMessage, Is.True);
        Assert.That(Listener.LastMessage, Is.EqualTo($"Invalid null dictionary, line {lineNumber}: {dictionaryText}"));
#else
        BrokenContractException Exception = Assert.ThrowsAsync<BrokenContractException>(async () => await Contract.MapAsync(TestEnum.None, Dictionary).ConfigureAwait(false)); int lineNumber = DebugTraceListener.LineNumber(); const string dictionaryText = "Dictionary";

        Assert.That(Exception.Message, Is.EqualTo($"Invalid null dictionary, line {lineNumber}: {dictionaryText}"));
#endif

        await Task.CompletedTask.ConfigureAwait(false);
    }
}
