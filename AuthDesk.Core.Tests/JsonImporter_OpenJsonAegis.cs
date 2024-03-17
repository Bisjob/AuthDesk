using AuthDesk.Core.Services;

namespace AuthDesk.Core.Tests;

public class JsonImporter_OpenJsonAegis
{
    [Fact]
    public void FileNotFound()
    {
        var importer = new JsonImporter();

        var result = importer.OpenJsonAegis("non-existing", password: null);

        Assert.Null(result);
    }

    [Fact]
    public Task AegisPlain()
    {
        var importer = new JsonImporter();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "aegis_plain.json");

        var result = importer.OpenJsonAegis(path, password: null);

        return Verify(result);
    }

    [Fact]
    public Task AegisEncrypted()
    {
        var importer = new JsonImporter();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "aegis_encrypted.json");

        var result = importer.OpenJsonAegis(path, password: "test");

        return Verify(result);
    }
}
