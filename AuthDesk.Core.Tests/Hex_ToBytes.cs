using AuthDesk.Core.Tools;

namespace AuthDesk.Core.Tests;

public class Hex_ToBytes
{
    [Fact]
    public void Empty()
    {
        var result = Hex.ToBytes("");

        Assert.Equal(new byte[] {}, result);
    }

    [Fact]
    public void Simple()
    {
        var result = Hex.ToBytes("0001ff");

        Assert.Equal(new byte[] {0x00, 0x01, 0xff}, result);
    }

    [Fact]
    public void Capitalized()
    {
        var result = Hex.ToBytes("0001FF");

        Assert.Equal(new byte[] {0x00, 0x01, 0xff}, result);
    }
}
