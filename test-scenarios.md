# Parity Environment — Provider File Transfer UI Test Flows

---

## Screen 1: File Transfer Configuration

This screen covers the end-to-end flows for managing file transfer configurations.

It supports four main activities:
1) Searching a configuration
2) Viewing a configuration
3) Creating a new configuration
4) Editing an existing configuration

A) Searching a configuration

1) Page load / initial UI
   1. Navigate to **File Transfer Configuration**.
   2. Confirm the following are present:
      - **Create New** button
      - **Search Criteria** panel

2) Search Criteria fields (verify all are available)
   1. Open the **Search Criteria** panel.
   2. Confirm you can enter/select:
      - **Config Id**
      - **Config Name**
      - **Owner Name**
      - **Owner Module** (dropdown)
      - **Protocol**

3) Execute search (known values)
   1. Enter one or more known criteria values.
   2. Click **Search**.
   3. Confirm a **results grid** appears and contains **rows** when matches exist.

4) Search variations (validate each path returns expected results)
   1. Search by **Config Id**
      - Enter a known **Config Id** -> click **Search** -> verify matching row(s).
   2. Search by **Configuration Name**
      - Enter a known **Config Name** (or partial, if supported) -> click **Search** -> verify matching row(s).
   3. Search by **Owner Name**
      - Enter a known **Owner Name** -> click **Search** -> verify matching row(s).
   4. Search by **Protocol**
      - Select a **Protocol** -> click **Search** -> verify matching configuration row(s).
   5. Search with **Include Inactives**
      - Run search with **Include Inactives** unchecked -> note results -> enable **Include Inactives** -> search again -> verify inactive configurations appear.

B) Viewing a configuration

1) Open a configuration
   1. In the results grid, locate a configuration row.
   2. **Double-click** the row.
   3. Confirm a **View Configuration** modal opens.

2) Verify source configuration details
   1. In the view modal, confirm you can see source-side details (as shown in parity), including:
      - Source configuration details (address / mask / protocol / server where applicable)
      - Owner information
      - Run schedule / scheduling information

3) Verify destination configuration list
   1. Locate the **Destinations** section/grid.
   2. Confirm destination row(s) are displayed (when configured).

4) Drill into a destination
   1. Click a destination row in the destinations grid.
   2. Confirm a destination detail panel opens/shows.
   3. Verify destination-specific information is visible (address-based vs contract/FTE metadata based on destination type).

C) Creating a new configuration

1) Open the create flow
   1. Click **Create New**.
   2. Confirm an **Add / Create Configuration** modal opens.

2) Required fields and validation (negative test)
   1. Without entering required values, click **Save**.
   2. Confirm validation messages are shown.
   3. At minimum, confirm validation behavior for:
      - Required source fields (as marked required in the UI)
      - Missing run schedule (if required by the UI)
      - No destinations / no active destinations (if the UI enforces at least one active destination)

3) Create with valid values (happy path)
   1. Fill out the **Source configuration** section.
   2. Fill out the **Run schedule** / scheduling section.
   3. Add at least one **Destination**:
      - Click destination **Create New**
      - Fill destination fields
      - Click **Add and Close** (or equivalent)
   4. Click **Save** for the configuration.
   5. Confirm a success message is shown (toast/message such as “Saved successfully”).

4) Confirm the new configuration exists
   1. Close the modal.
   2. Search by the **Config Name** (or another unique value you used).
   3. Confirm the new configuration appears in the results grid.

D) Editing an existing configuration

1) Open edit mode
   1. Search and locate a configuration.
   2. **Double-click** the row to open the view modal.
   3. Click **Edit** (often bottom-left in the modal).
   4. Confirm the **Edit Configuration** modal opens.

2) Verify editable areas are present
   1. Confirm you can edit values across:
      - Source configuration
      - Run schedule
      - Destination configuration list/grid

3) Drill into destination details while editing
   1. In the destination grid/list, click a destination row.
   2. Confirm the destination edit/detail panel shows additional destination fields.

4) Validate key edit validations
   1. Run schedule missing
      - Clear/unselect run schedule (if allowed) -> click **Save** -> confirm validation message.
   2. No active destinations
      - Remove destinations or mark all as inactive -> click **Save** -> confirm validation message.
   3. Duplicate destination address (if you can add destinations)
      - Add a destination with an address already present -> confirm duplicate validation message.
   4. Invalid FTP/SFTP address format (if protocol requires it)
      - Enter an invalid source/destination address format -> click **Save** -> confirm validation message.

5) Edit + Save updates
   1. Make a small, safe change (example: Description).
   2. Click **Save**.
   3. Confirm a success message.
   4. Re-run a search and re-open the configuration.
   5. Confirm the updated value is persisted.

---

## Screen 2: File Transfer Tracking

This screen supports two main activities:
1) Searching tracking records
2) Viewing tracking details (Information, Events, Exceptions)

A) Searching tracking records

1) Page load / criteria fields
   1. Navigate to **File Transfer Tracking**.
   2. Confirm the Search Criteria panel contains these fields:
      - **File Id**
      - **File Name**
      - **File Status**
      - **Direction**
      - **Transport Type**
      - **Source Location** (dropdown)
      - **Trading Partner Id**
      - **From Date**
      - **To Date**
      - **From Size**
      - **To Size**

2) Execute search (known information)
   1. Enter known criteria (commonly a date range and/or file name).
   2. Click **Search**.
   3. Confirm a results grid appears and shows rows when matches exist.

3) Search variations (optional, if data exists)
   1. Search by **File Id** (exact).
   2. Search by **File Name** (exact or partial).
   3. Search by **Source Location** + date range.
   4. Search by **Status** + date range.

B) Viewing a tracking record

1) Open tracking detail
   1. In the results grid, locate a tracking record.
   2. **Double-click** the row.
   3. Confirm a **Tracking Detail** modal opens.

2) Verify the tracking detail sections
   1. Information section
      - Confirm file metadata is present (id, name, status, direction, transport type, size, checksum/source, timestamps as displayed).
   2. Events section
      - Confirm an Events grid/list is present with event code/description and timestamps.
   3. Exceptions section
      - Confirm an Exceptions grid/list is present.
      - Confirm you can expand/open an exception to view message and stack trace (as displayed).

---

## Screen 3: FTP Server Credentials (SFTP/FTP Server Config)

This screen supports three main activities:
1) Searching server credentials
2) Viewing server credentials
3) Creating server credentials

A) Searching server credentials

1) Page load / initial UI
   1. Navigate to **FTP Server Credentials**.
   2. Confirm you see:
      - A **Search Criteria** panel
      - A **Server Name** field
      - A **Create New** button (may be role/rights dependent)

2) Execute search
   1. Enter a known **Server Name**.
   2. Click **Search**.
   3. Confirm a results grid appears and shows rows.

3) Include Invalids
   1. Search with **Include Invalids** unchecked.
   2. Toggle **Include Invalids**.
   3. Refresh / re-search.
   4. Confirm invalid credentials appear only when included.

B) Viewing server credentials

1) Open the view/edit modal
   1. In the results grid, locate a credential row.
   2. **Double-click** the row.
   3. Confirm a modal opens.

2) Verify information shown
   1. FTP Server Credentials section (as displayed in parity)
      - Server Name
      - Description
      - Transfer Mode
      - Server IP
      - Port
      - Proxy Address / Proxy Port (if present)
      - User
      - Password fields (may be hidden/read-only depending on mode)
   2. Audit Details section
      - Operator Id
      - Creation Date
      - Last Modified Date
      - Is Valid

3) Invalidate a credential (only if permitted)
   1. Click **Invalidate**.
   2. Confirm a warning prompt appears.
   3. Click **Yes/Confirm**.
   4. Return to the grid and verify the credential now shows as invalid.
   5. Verify it is only visible when **Include Invalids** is enabled.

C) Creating server credentials

1) Open the create flow
   1. Click **Create New**.
   2. Confirm the modal title indicates **Add FTP Server Credentials**.

2) Required fields and validation
   1. Attempt to **Save** with required fields empty.
   2. Confirm validation messages appear for required fields.
   3. Validate invalid IP format:
      - Enter a non-IPv4 value in Server IP -> click **Save** -> confirm invalid IP address format error.

3) Create with valid values
   1. Enter required values (server name, ip, port, user, password/confirmation as required).
   2. Click **Save**.
   3. Confirm success message.
   4. Search by the new **Server Name**.
   5. Confirm the new record appears in the results grid.

---

Notes to capture while executing (for anything unexpected):
1) Exact click path (what you clicked, in order)
2) Values entered/selected
3) Screenshot name(s)
4) Error message text exactly as displayed
5) Whether the action might be role/rights gated
