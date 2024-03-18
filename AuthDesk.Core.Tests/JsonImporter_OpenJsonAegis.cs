using AuthDesk.Core.Services;

namespace AuthDesk.Core.Tests;

public class JsonImporter_OpenJsonAegis
{
    [Fact]
    public async Task FileNotFound()
    {
        var importer = new JsonImporter();

        var result = await importer.OpenJsonAegis("non-existing");

        Assert.Empty(result);
    }

    [Fact]
    public async Task AegisPlain()
    {
        var importer = new JsonImporter();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "aegis_plain.json");

        var result = await importer.OpenJsonAegis(path);

        await Verify(result);
    }

    [Fact]
    public async Task AegisEncrypted()
    {
        var importer = new JsonImporter();

        importer.OnAskForPassword += (obj, e) => { e.Password = "test"; return Task.CompletedTask; };

        var path = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "aegis_encrypted.json");

        var result = await importer.OpenJsonAegis(path);

        await Verify(result);
    }
}
