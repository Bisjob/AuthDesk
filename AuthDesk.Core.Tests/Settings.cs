using VerifyTests.DiffPlex;
using DiffEngine;
using System.Runtime.CompilerServices;

namespace AuthDesk.Core.Tests;

public class Settings
{
    [ModuleInitializer]
    public static void Init()
    {
        // In test output (e.g. from `dotnet test`), only show diffs, no complete
        // snapshots.
        VerifyDiffPlex.Initialize(OutputType.Compact);

        // Disable the diff runner (e.g. it would run Neovim on Linux, which is doesn't
        // interact well with `dotnet test`).
        DiffRunner.Disabled = true;
    }
}
