namespace Contracts.Test;

using NUnit.Framework;

[TestFixture]
public class TestAttributes
{
    [Test]
    public void TestSampleMethod()
    {
        SampleMethod(string.Empty);
    }

    [Access("public")]
    [RequireNotNull("text")]
    [Require("text.Length >= 0")]
    [Ensure("text.Length >= 0")]
    public void SampleMethod(string text)
    {
        SampleField++;
    }

    public int SampleField { get; set; }
}
