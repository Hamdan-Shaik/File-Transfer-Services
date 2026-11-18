-- Indexes and constraints for FXFER baseline schema.

-- Case-insensitive uniqueness for active servers.
CREATE UNIQUE INDEX IF NOT EXISTS ux_ftp_server_name_active
  ON ftp_server (lower(ftp_server_name))
  WHERE is_valid;

-- Business key uniqueness per contract.
CREATE UNIQUE INDEX IF NOT EXISTS ux_fts_source_location_config_id
  ON fts_source_location (config_id);

-- FK helper indexes.
CREATE INDEX IF NOT EXISTS ix_fts_source_location_ftp_server_id
  ON fts_source_location (ftp_server_id);

CREATE INDEX IF NOT EXISTS ix_fts_destination_fts_source_location_id
  ON fts_destination (fts_source_location_id);

CREATE INDEX IF NOT EXISTS ix_fts_destination_ftp_server_id
  ON fts_destination (ftp_server_id);

CREATE INDEX IF NOT EXISTS ix_fts_file_management_file_source_location_id
  ON fts_file_management (file_source_location_id);

CREATE INDEX IF NOT EXISTS ix_fts_file_event_file_id
  ON fts_file_event (file_id);

CREATE INDEX IF NOT EXISTS ix_fts_file_event_destination_id
  ON fts_file_event (destination_id);

CREATE INDEX IF NOT EXISTS ix_fts_file_delivery_file_id
  ON fts_file_delivery (file_id);

CREATE INDEX IF NOT EXISTS ix_fts_file_delivery_destination_id
  ON fts_file_delivery (destination_id);

CREATE INDEX IF NOT EXISTS ix_fts_file_exception_file_id
  ON fts_file_exception (file_id);

CREATE INDEX IF NOT EXISTS ix_fts_file_exception_destination_id
  ON fts_file_exception (destination_id);

CREATE INDEX IF NOT EXISTS ix_fts_file_exception_event_id
  ON fts_file_exception (event_id);

CREATE INDEX IF NOT EXISTS ix_fts_suspension_fts_source_location_id
  ON fts_suspension (fts_source_location_id);

CREATE INDEX IF NOT EXISTS ix_fts_notification_fts_source_location_id
  ON fts_notification (fts_source_location_id);
