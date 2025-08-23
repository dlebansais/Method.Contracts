namespace Contracts.Test;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

internal class DebugTraceListener : TraceListener
{
    public bool IsAssertTriggered { get; private set; }

    public bool IsOnlyOneMessage => RecordedMessages.Count == 1;

    public string LastMessage => RecordedMessages[^1];

    public bool IsExceptionMessage => RecordedDetailMessages.Count == 2 &&
#if NET8_0_OR_GREATER || NETCOREAPP3_1
                                      RecordedDetailMessages[0].Contains(".cs:line", StringComparison.Ordinal);
#else
                                      RecordedDetailMessages[0].IndexOf(".cs:line", StringComparison.Ordinal) >= 0;
#endif

    public override void Write(string? message) => RecordedMessages.Add(message!);

    public override void WriteLine(string? message) => RecordedDetailMessages.Add(message!);

    public override void Fail(string? message)
    {
        IsAssertTriggered = true;

        Write(message);
    }

    public override void Fail(string? message, string? detailMessage)
    {
        Fail(message);

        WriteLine(detailMessage);
    }

    public static int LineNumber([CallerLineNumber] int lineNumber = 0) => lineNumber;

    private List<string> RecordedMessages { get; } = [];
    private List<string> RecordedDetailMessages { get; } = [];
}
