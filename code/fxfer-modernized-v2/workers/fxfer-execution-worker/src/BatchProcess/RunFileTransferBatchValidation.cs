using System.Text;

namespace Fxfer.ExecutionWorker.BatchProcess;

public static class RunFileTransferBatchValidation
{
    public static bool TryValidateRunDetails(string[] runDetails, out Guid tenantId, out bool processingAllFiles)
    {
        // F03-004: only accept exactly 1 or 3 run details.
        tenantId = default;
        processingAllFiles = false;

        var badArgFound = false;

        if (runDetails.Length != 1 && runDetails.Length != 3)
        {
            return false;
        }

        if (runDetails.Length == 1)
        {
            // F03-005: single arg is the tenant id and must be a GUID.
            processingAllFiles = true;
            if (Guid.TryParse(runDetails[0], out tenantId) == false)
            {
                badArgFound = true;
            }
        }

        int commandValue;
        Guid fileId;
        if (!processingAllFiles)
        {
            // F03-007: do not validate runDetails[0] when length is 3.
            // F03-006: second value must parse as int or guid; third value must parse as tenant guid.
            if (int.TryParse(runDetails[1], out commandValue) == false && Guid.TryParse(runDetails[1], out fileId) == false)
            {
                badArgFound = true;
            }

            if (Guid.TryParse(runDetails[2], out tenantId) == false)
            {
                badArgFound = true;
            }
        }

        if (badArgFound)
        {
            return false;
        }

        return true;
    }

    public static string GetUsageMessage()
    {
        // F03-004: legacy usage text stays byte-for-byte equivalent.
        var msg = new StringBuilder();
        msg.AppendLine("Usage: FileTransferService <command> <command_value>")
            .AppendLine("  command = sakTrack X (X is the sak_file_track to be processed)")
            .AppendLine("            configId X (X is the config_id of the location to be processed")
            .AppendLine("                     top X (X is the number of failed entries to be processed")
            .Append("  To process all files do not specify the <command> and <command_value> arguments");
        return msg.ToString();
    }
}
