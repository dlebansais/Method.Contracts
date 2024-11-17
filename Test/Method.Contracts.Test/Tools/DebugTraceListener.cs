namespace Contracts.Test;

using System.Diagnostics;

internal class DebugTraceListener : TraceListener
{
    public bool IsAssertTriggered { get; private set; }

    public override void Write(string? message)
    {
    }

    public override void WriteLine(string? message)
    {
    }

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
}
