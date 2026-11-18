using Microsoft.Extensions.Logging;

namespace Fxfer.TransferWorker.Worker;

public static class CliEntrypoint
{
    public static bool TryHandle(string[] args, out int exitCode)
    {
        exitCode = 0;

        if (args.Length == 0)
        {
            return false;
        }

        if (args.Length > 0 && (string.Equals(args[0], "run-file-transfer-batch-basic", StringComparison.OrdinalIgnoreCase)
            || string.Equals(args[0], "basic", StringComparison.OrdinalIgnoreCase)))
        {
            return TryHandleBasic(args, out exitCode);
        }

        if (args.Length > 0 && (string.Equals(args[0], "run-file-transfer-batch-managed", StringComparison.OrdinalIgnoreCase)
            || string.Equals(args[0], "managed", StringComparison.OrdinalIgnoreCase)))
        {
            return TryHandleManaged(args, out exitCode);
        }

        if (args.Length > 0 && (string.Equals(args[0], "rerun-tracked-file", StringComparison.OrdinalIgnoreCase)
            || string.Equals(args[0], "rerun", StringComparison.OrdinalIgnoreCase)))
        {
            return TryHandleRerun(args, out exitCode);
        }

        if (LooksLikeLegacyBatchDoorInvocation(args))
        {
            return TryHandleLegacyBatchDoor(args, out exitCode);
        }

        return false;
    }

    private static bool TryHandleBasic(string[] args, out int exitCode)
    {
        var loggerFactory = CreateLoggerFactory();
        var runner = FileTransferBatchBasicRunner.CreateDefault(loggerFactory);
        exitCode = runner.Run(args.Skip(1).ToArray());
        return true;
    }

    private static bool TryHandleManaged(string[] args, out int exitCode)
    {
        exitCode = FileTransferBatchManagedRunner.CreateDefault().Run(args.Skip(1).ToArray());
        return true;
    }

    private static bool LooksLikeLegacyBatchDoorInvocation(string[] args)
    {
        return args.Length == 1
            ? Guid.TryParseExact(args[0], "D", out _)
            : args.Length == 3 && Guid.TryParseExact(args[2], "D", out _);
    }

    private static bool TryHandleRerun(string[] args, out int exitCode)
    {
        exitCode = 1;

        var tenantId = TryGetOptionValue(args, "--tenant-id");
        var fileId = TryGetOptionValue(args, "--tracked-file-id") ?? TryGetOptionValue(args, "--file-id");

        if (!Guid.TryParseExact(tenantId, "D", out var parsedTenantId) || !Guid.TryParseExact(fileId, "D", out var parsedFileId))
        {
            Console.Error.WriteLine("Invalid args. Usage: fxfer-transfer-worker rerun-tracked-file --tenant-id <uuid> --tracked-file-id <uuid>");
            return true;
        }

        exitCode = FileTransferBatchRerunCommand.CreateDefault().Run(parsedTenantId, parsedFileId);
        return true;
    }

    private static bool TryHandleLegacyBatchDoor(string[] args, out int exitCode)
    {
        exitCode = FileTransferBatchManagedRunner.CreateDefault().Run(args);
        return true;
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(logging =>
        {
            logging.AddSimpleConsole();
            logging.SetMinimumLevel(LogLevel.Information);
        });
    }

    private static string? TryGetOptionValue(string[] args, string optionName)
    {
        for (var index = 0; index < args.Length; index++)
        {
            var arg = args[index];

            if (arg.StartsWith(optionName + "=", StringComparison.OrdinalIgnoreCase))
            {
                return arg[(optionName.Length + 1)..];
            }

            if (string.Equals(arg, optionName, StringComparison.OrdinalIgnoreCase) && index + 1 < args.Length)
            {
                return args[index + 1];
            }
        }

        return null;
    }
}
