using System.Reflection;
using Fxfer.Bff.Application.Providers;

namespace Fxfer.Bff.Tests;

public sealed class T_008_ProhibitDirectDatabaseAccessTests
{
    [Fact]
    public void T_008_prohibits_direct_database_access_from_the_bff()
    {
        Program.EnsureNoDirectDatabaseDependencies(
            typeof(Program).Assembly,
            typeof(IFileTransferCommandProvider).Assembly,
            typeof(IFileTransferQueryProvider).Assembly);

        AssertConfigureServicesCallsGuard();
    }

    private static void AssertConfigureServicesCallsGuard()
    {
        var configureServices = typeof(Program).GetMethod(
            nameof(Program.ConfigureServices),
            BindingFlags.Public | BindingFlags.Static);

        Assert.NotNull(configureServices);

        var guard = typeof(Program).GetMethod(
            nameof(Program.EnsureNoDirectDatabaseDependencies),
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);

        Assert.NotNull(guard);

        var il = configureServices!.GetMethodBody()?.GetILAsByteArray();
        Assert.NotNull(il);

        var token = BitConverter.GetBytes(guard!.MetadataToken);
        var callInstruction = new[] { (byte)0x28, token[0], token[1], token[2], token[3] };

        Assert.True(ContainsSubsequence(il!, callInstruction));
    }

    private static bool ContainsSubsequence(byte[] source, byte[] subsequence)
    {
        if (subsequence.Length == 0)
        {
            return true;
        }

        for (var i = 0; i <= source.Length - subsequence.Length; i++)
        {
            var matches = true;

            for (var j = 0; j < subsequence.Length; j++)
            {
                if (source[i + j] != subsequence[j])
                {
                    matches = false;
                    break;
                }
            }

            if (matches)
            {
                return true;
            }
        }

        return false;
    }
}

