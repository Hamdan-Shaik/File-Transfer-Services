-- FXFER schema baseline (database-per-tenant; no tenant_id columns)

-- T-009: Versioned migrations are written to be re-runnable (idempotent) per tenant.
CREATE TABLE IF NOT EXISTS ftp_server (
  ftp_server_id uuid PRIMARY KEY,
  ftp_server_name varchar(50) NOT NULL,
  ftp_server_host varchar(50) NOT NULL,
  ftp_transfer_mode varchar(15) NOT NULL,
  ftp_user varchar(50) NOT NULL,
  ftp_password_secret_ref text NOT NULL,
  ftp_description varchar(100) NOT NULL,
  ftp_port smallint NOT NULL,
  ftp_proxy_address varchar(200) NULL,
  ftp_proxy_port smallint NULL,
  is_valid boolean NOT NULL,
  use_ssh_auth boolean NOT NULL,
  operator_id varchar(200) NOT NULL,
  created_ts timestamp with time zone NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS fts_source_location (
  fts_source_location_id uuid PRIMARY KEY,
  config_id integer NOT NULL,
  config_name varchar(50) NOT NULL,
  source_protocol_cd varchar(4) NOT NULL,
  source_address varchar(248) NOT NULL,
  source_file_mask varchar(50) NOT NULL,
  ftp_server_id uuid NULL REFERENCES ftp_server(ftp_server_id),
  send_notifications_ind boolean NOT NULL,
  owner_name varchar(50) NOT NULL,
  owner_module uuid NOT NULL,
  owner_phone varchar(50) NULL,
  owner_email varchar(50) NULL,
  description varchar(200) NULL,
  run_on_monday_ind boolean NOT NULL,
  run_on_tuesday_ind boolean NOT NULL,
  run_on_wednesday_ind boolean NOT NULL,
  run_on_thursday_ind boolean NOT NULL,
  run_on_friday_ind boolean NOT NULL,
  run_on_saturday_ind boolean NOT NULL,
  run_on_sunday_ind boolean NOT NULL,
  run_on_first_day_of_month_ind boolean NOT NULL,
  run_on_last_day_of_month_ind boolean NOT NULL,
  run_on_this_day_of_month smallint NULL,
  start_time_24hhmm varchar(4) NOT NULL,
  end_time_24hhmm varchar(4) NOT NULL,
  file_direction varchar(50) NULL,
  trading_partner varchar(10) NULL,
  is_valid boolean NOT NULL,
  is_suspended boolean NOT NULL,
  is_suspended_changed_ts timestamp with time zone NULL,
  operator_id varchar(200) NOT NULL,
  created_ts timestamp with time zone NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS fts_destination (
  destination_id uuid PRIMARY KEY,
  fts_source_location_id uuid NOT NULL REFERENCES fts_source_location(fts_source_location_id),
  destination_address varchar(248) NOT NULL,
  dest_protocol_cd varchar(4) NOT NULL,
  overwrite_at_dest_ind boolean NOT NULL,
  compress_at_dest_ind boolean NOT NULL,
  rename_at_dest_ind boolean NOT NULL,
  dest_is_archive_ind boolean NOT NULL,
  is_valid boolean NOT NULL,
  ftp_server_id uuid NULL REFERENCES ftp_server(ftp_server_id),
  rename_pattern varchar(50) NULL,
  contact_name varchar(50) NULL,
  contact_phone varchar(15) NULL,
  contact_email varchar(150) NULL,
  operator_id varchar(200) NOT NULL,
  created_ts timestamp with time zone NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL,
  cde_encryption varchar(8) NULL,
  encryption_public_key text NULL,
  encryption_private_key_secret_ref text NULL,
  private_key_password_secret_ref text NULL,
  external_file_defn_id uuid NULL,
  destination_type varchar(200) NULL,
  contract_name varchar(200) NULL
);

CREATE TABLE IF NOT EXISTS fts_file_management (
  file_id uuid PRIMARY KEY,
  file_source_location_id uuid NOT NULL REFERENCES fts_source_location(fts_source_location_id),
  file_name varchar(500) NOT NULL,
  file_status varchar(50) NOT NULL,
  file_status_ts timestamp with time zone NOT NULL,
  sak_file_track integer NULL,
  file_size numeric NOT NULL,
  checksum_cd varchar(50) NULL,
  operator_id varchar(200) NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL,
  created_ts timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS fts_file_event (
  event_id uuid PRIMARY KEY,
  file_id uuid NOT NULL REFERENCES fts_file_management(file_id),
  destination_id uuid NOT NULL REFERENCES fts_destination(destination_id),
  event_cd varchar(50) NOT NULL,
  event_track_description varchar(500) NULL,
  created_ts timestamp with time zone NOT NULL,
  operator_id varchar(200) NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS fts_file_delivery (
  delivery_id uuid PRIMARY KEY,
  file_id uuid NOT NULL REFERENCES fts_file_management(file_id),
  destination_id uuid NOT NULL REFERENCES fts_destination(destination_id),
  delivery_ts timestamp with time zone NOT NULL,
  destination_file_name varchar(500) NULL,
  operator_id varchar(200) NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL,
  created_ts timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS fts_file_exception (
  exception_id uuid PRIMARY KEY,
  file_id uuid NOT NULL REFERENCES fts_file_management(file_id),
  destination_id uuid NOT NULL REFERENCES fts_destination(destination_id),
  event_id uuid NOT NULL REFERENCES fts_file_event(event_id),
  exception_type varchar(200) NOT NULL,
  method_name varchar(200) NULL,
  message text NOT NULL,
  stack_trace text NULL,
  created_ts timestamp with time zone NOT NULL,
  operator_id varchar(200) NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS fts_suspension (
  suspension_id uuid PRIMARY KEY,
  fts_source_location_id uuid NOT NULL REFERENCES fts_source_location(fts_source_location_id),
  error_text varchar(250) NOT NULL,
  stack_trace varchar(1000) NOT NULL,
  operator_id varchar(200) NOT NULL,
  created_ts timestamp with time zone NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS fts_notification (
  notification_id uuid PRIMARY KEY,
  fts_source_location_id uuid NOT NULL REFERENCES fts_source_location(fts_source_location_id),
  email_address varchar(254) NOT NULL,
  contact_name varchar(50) NULL,
  contact_phone varchar(15) NULL,
  notify_on_error_ind boolean NOT NULL,
  notify_on_success_ind boolean NOT NULL,
  operator_id varchar(200) NOT NULL,
  created_ts timestamp with time zone NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS fts_inbound_target (
  fts_inbound_target_id uuid PRIMARY KEY,
  fte_metadata_id uuid NOT NULL,
  import_job_name varchar(200) NOT NULL,
  contract_name varchar(200) NOT NULL,
  operator_id varchar(200) NOT NULL,
  is_valid boolean NOT NULL,
  created_ts timestamp with time zone NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL
);

CREATE TABLE IF NOT EXISTS fts_integration_file (
  fts_integration_file_id uuid PRIMARY KEY,
  group_id uuid NULL,
  config_id integer NULL,
  file_name varchar(500) NOT NULL,
  etf_outbound_metadata_defn_id uuid NOT NULL,
  is_processed boolean NOT NULL,
  operator_id varchar(200) NOT NULL,
  is_valid boolean NOT NULL,
  created_ts timestamp with time zone NOT NULL,
  last_modified_ts timestamp with time zone NOT NULL
);
