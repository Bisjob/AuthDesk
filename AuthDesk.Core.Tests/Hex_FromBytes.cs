using AuthDesk.Core.Tools;

namespace AuthDesk.Core.Tests;

public class Hex_FromBytes
{
    [Fact]
    public void Empty()
    {
        var result = Hex.FromBytes(new byte[] {});

        Assert.Equal("", result);
    }

    [Fact]
    public void Simple()
    {
        var result = Hex.FromBytes(new byte[] {0x00, 0x01, 0xff});

        Assert.Equal("0001ff", result);
    }
}
