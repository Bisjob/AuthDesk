using System.Diagnostics;
using System.Reflection;

using AuthDesk.Contracts.Services;

using OSVersionHelper;


namespace AuthDesk.Services;

public class ApplicationInfoService : IApplicationInfoService
{
    public ApplicationInfoService()
    {
    }

    public Version GetVersion()
    {
        // Try to get version from assembly attributes if packaged approach does not work
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName();

        Version version = assemblyName.Version;
        if (version != null)
        {
            return version;
        }

        // Fallback to file version info if AssemblyName.Version is not set
        string assemblyLocation = assembly.Location;
        var fileVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
        return new Version(fileVersion);
    }
}
