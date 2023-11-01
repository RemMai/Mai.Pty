using System.Diagnostics;
using Xunit.Abstractions;

namespace Pty.Test;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test1()
    {
    }

    [Fact]
    public void run()
    {
    }
}