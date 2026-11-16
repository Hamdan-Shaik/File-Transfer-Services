using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FxferService.Infrastructure.Persistence.Migrations;

[DbContext(typeof(FxferDbContext))]
[Migration("202611060001_Initial")]
public sealed partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "FTP_SERVER",
            columns: table => new
            {
                FTP_SERVER_ID = table.Column<Guid>(type: "uuid", nullable: false),
                FTP_SERVER_NAME = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                FTP_SERVER_IP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                FTP_TRANSFER_MODE = table.Column<string>(type: "text", nullable: false),
                FTP_USER = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                FTP_PASSWORD = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                FTP_DESCRIPTION = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                FTP_PORT = table.Column<short>(type: "smallint", nullable: false),
                FTP_PROXY_ADR = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                FTP_PROXY_PORT = table.Column<short>(type: "smallint", nullable: true),
                SSH_AUTHENTICATION = table.Column<bool>(type: "boolean", nullable: false),
                IS_VALID = table.Column<bool>(type: "boolean", nullable: false),
                OPERATOR_ID = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTP_SERVER", x => x.FTP_SERVER_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_SOURCE_LOCATION",
            columns: table => new
            {
                FTS_SOURCE_LOCATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                CONFIG_ID = table.Column<int>(type: "integer", nullable: false),
                CONFIG_NAME = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                SOURCE_PROTOCOL_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                SOURCE_ADDRESS = table.Column<string>(type: "character varying(248)", maxLength: 248, nullable: false),
                SOURCE_FILE_MASK = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                FTP_SERVER_ID = table.Column<Guid>(type: "uuid", nullable: true),
                SEND_NOTIFICATIONS_IND = table.Column<bool>(type: "boolean", nullable: false),
                OWNER_NAME = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                MODULE_ID = table.Column<Guid>(type: "uuid", nullable: false),
                OWNER_PHONE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                OWNER_EMAIL = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                CONFIG_DESCRIPTION = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                RUN_ON_MONDAY_IND = table.Column<bool>(type: "boolean", nullable: false),
                RUN_ON_TUESDAY_IND = table.Column<bool>(type: "boolean", nullable: false),
                RUN_ON_WED_IND = table.Column<bool>(type: "boolean", nullable: false),
                RUN_ON_THURS_IND = table.Column<bool>(type: "boolean", nullable: false),
                RUN_ON_FRI_IND = table.Column<bool>(type: "boolean", nullable: false),
                RUN_ON_SAT_IND = table.Column<bool>(type: "boolean", nullable: false),
                RUN_ON_SUN_IND = table.Column<bool>(type: "boolean", nullable: false),
                RUN_ON_FRST_DAY_OF_MONTH_IND = table.Column<bool>(type: "boolean", nullable: false),
                RUN_ON_LAST_DAY_OF_MONTH_IND = table.Column<bool>(type: "boolean", nullable: false),
                RUN_ON_THIS_DAY_OF_MONTH = table.Column<short>(type: "smallint", nullable: false),
                START_TIME_24HHMM = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                END_TIME_24HHMM = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                FILE_DIRECTION = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                TRADING_PARTNER = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                IS_VALID = table.Column<bool>(type: "boolean", nullable: false),
                IS_SUSPENDED = table.Column<bool>(type: "boolean", nullable: false),
                IS_SUSPENDED_CHANGED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                OPERATOR_ID = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_SOURCE_LOCATION", x => x.FTS_SOURCE_LOCATION_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_DESTINATION",
            columns: table => new
            {
                DESTINATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                FTS_SOURCE_LOCATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                DESTINATION_ADDRESS = table.Column<string>(type: "character varying(248)", maxLength: 248, nullable: false),
                DEST_PROTOCOL_CD = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                OVERWRITE_AT_DEST_IND = table.Column<bool>(type: "boolean", nullable: false),
                COMPRESS_AT_DEST_IND = table.Column<bool>(type: "boolean", nullable: false),
                RENAME_AT_DEST_IND = table.Column<bool>(type: "boolean", nullable: false),
                DEST_IS_ARCHIVE_IND = table.Column<bool>(type: "boolean", nullable: false),
                IS_VALID = table.Column<bool>(type: "boolean", nullable: false),
                FTP_SERVER_ID = table.Column<Guid>(type: "uuid", nullable: false),
                RENAME_PATTERN = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                CONTACT_NAME = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                CONTACT_PHONE = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                CONTACT_EMAIL = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                OPERATOR_ID = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                CREATION_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CDE_ENCRYPTION = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                ENCRYPTION_PUBLICKEY = table.Column<string>(type: "text", nullable: true),
                ENCRYPTION_PRIVATEKEY = table.Column<string>(type: "text", nullable: true),
                PRIVATEKEY_PASSWORD = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                EXTERNAL_FILE_DEFN_ID = table.Column<Guid>(type: "uuid", nullable: true),
                DESTINATION_TYPE = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                CONTRACT_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_DESTINATION", x => x.DESTINATION_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_INBOUND_TARGET",
            columns: table => new
            {
                FTS_INBOUND_TARGET_ID = table.Column<Guid>(type: "uuid", nullable: false),
                EXTERNAL_FILE_DEFN_ID = table.Column<Guid>(type: "uuid", nullable: false),
                IMPORT_JOB_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                CONTRACT_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                OPERATOR_ID = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                IS_VALID = table.Column<bool>(type: "boolean", nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_INBOUND_TARGET", x => x.FTS_INBOUND_TARGET_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_INTEGRATION_FILE",
            columns: table => new
            {
                FTS_INTEGRATION_FILE_ID = table.Column<Guid>(type: "uuid", nullable: false),
                GROUP_ID = table.Column<Guid>(type: "uuid", nullable: true),
                CONFIG_ID = table.Column<int>(type: "integer", nullable: true),
                FILE_NAME = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                ETF_OUTBOUND_METADATA_DEFN_ID = table.Column<Guid>(type: "uuid", nullable: false),
                IS_PROCESSED = table.Column<bool>(type: "boolean", nullable: false),
                OPERATOR_ID = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                IS_VALID = table.Column<bool>(type: "boolean", nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_INTEGRATION_FILE", x => x.FTS_INTEGRATION_FILE_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_SUSPENSION",
            columns: table => new
            {
                SUSPENSION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                FTS_SOURCE_LOCATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                ERROR_TEXT = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                STACK_TRACE = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                OPERATOR_ID = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_SUSPENSION", x => x.SUSPENSION_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_FILE_MANAGEMENT",
            columns: table => new
            {
                FILE_ID = table.Column<Guid>(type: "uuid", nullable: false),
                FTS_SOURCE_LOCATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                FILE_NAME = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                FILE_STATUS = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                FILE_STATUS_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                SAK_FILE_TRACK = table.Column<int>(type: "integer", nullable: true),
                FILE_SIZE = table.Column<decimal>(type: "numeric", nullable: false),
                CHECKSUM_CD = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                OPERATOR_ID = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_FILE_MANAGEMENT", x => x.FILE_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_FILE_DELIVERY",
            columns: table => new
            {
                DELIVERY_ID = table.Column<Guid>(type: "uuid", nullable: false),
                FILE_ID = table.Column<Guid>(type: "uuid", nullable: false),
                DESTINATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                DELIVERY_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                DESTINATION_FILE_NAME = table.Column<string>(type: "text", nullable: false),
                OPERATOR_ID = table.Column<string>(type: "text", nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_FILE_DELIVERY", x => x.DELIVERY_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_FILE_EVENT",
            columns: table => new
            {
                EVENT_ID = table.Column<Guid>(type: "uuid", nullable: false),
                FILE_ID = table.Column<Guid>(type: "uuid", nullable: false),
                DESTINATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                EVENT_CD = table.Column<string>(type: "text", nullable: false),
                EVENT_TRACK_DESCRIPTION = table.Column<string>(type: "text", nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                OPERATOR_ID = table.Column<string>(type: "text", nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_FILE_EVENT", x => x.EVENT_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_FILE_EXCEPTION",
            columns: table => new
            {
                EXCEPTION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                FILE_ID = table.Column<Guid>(type: "uuid", nullable: false),
                DESTINATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                EVENT_ID = table.Column<Guid>(type: "uuid", nullable: false),
                EXCEPTION_TYPE = table.Column<string>(type: "text", nullable: false),
                METHOD_NAME = table.Column<string>(type: "text", nullable: false),
                MESSAGE = table.Column<string>(type: "text", nullable: false),
                STACK_TRACE = table.Column<string>(type: "text", nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                OPERATOR_ID = table.Column<string>(type: "text", nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_FILE_EXCEPTION", x => x.EXCEPTION_ID);
            });

        migrationBuilder.CreateTable(
            name: "FTS_NOTIFICATION",
            columns: table => new
            {
                NOTIFICATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                FTS_SOURCE_LOCATION_ID = table.Column<Guid>(type: "uuid", nullable: false),
                EMAIL_ADDRESS = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                CONTACT_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                CONTACT_PHONE = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                NOTIFY_ON_ERROR_IND = table.Column<bool>(type: "boolean", nullable: false),
                NOTIFY_ON_SUCCESS_IND = table.Column<bool>(type: "boolean", nullable: false),
                OPERATOR_ID = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                CREATED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                LAST_MODIFIED_TS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FTS_NOTIFICATION", x => x.NOTIFICATION_ID);
            });

        migrationBuilder.CreateTable(
            name: "FXFER_OUTBOX",
            columns: table => new
            {
                OUTBOX_MESSAGE_ID = table.Column<Guid>(type: "uuid", nullable: false),
                TENANT_ID = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                EVENT_NAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                EVENT_VERSION = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                OCCURRED_AT = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                PAYLOAD_JSON = table.Column<string>(type: "text", nullable: false),
                DISPATCHED_AT = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FXFER_OUTBOX", x => x.OUTBOX_MESSAGE_ID);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "FXFER_OUTBOX");
        migrationBuilder.DropTable(name: "FTS_NOTIFICATION");
        migrationBuilder.DropTable(name: "FTS_FILE_EXCEPTION");
        migrationBuilder.DropTable(name: "FTS_FILE_EVENT");
        migrationBuilder.DropTable(name: "FTS_FILE_DELIVERY");
        migrationBuilder.DropTable(name: "FTS_FILE_MANAGEMENT");
        migrationBuilder.DropTable(name: "FTS_SUSPENSION");
        migrationBuilder.DropTable(name: "FTS_INTEGRATION_FILE");
        migrationBuilder.DropTable(name: "FTS_INBOUND_TARGET");
        migrationBuilder.DropTable(name: "FTS_DESTINATION");
        migrationBuilder.DropTable(name: "FTS_SOURCE_LOCATION");
        migrationBuilder.DropTable(name: "FTP_SERVER");
    }
}



