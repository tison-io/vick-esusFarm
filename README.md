## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/tison-io/vick-esusFarm.git
cd vick-esusFarm
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Build the application

```bash
dotnet build
```

### 4. Run the application

```bash
dotnet run
```

The API will start on:
- HTTP: `http://localhost:5243`

## API Endpoints

### Register Farmer (USSD Flow)

**POST** `/api/ussd/register`

#### Request Body

```json
{
  "phoneNumber": "+256700123456",
  "sessionId": "unique-session-id",
  "userInput": ""
}
```

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| `phoneNumber` | string | Yes | Farmer's phone number (7-15 digits, optionally prefixed with +) |
| `sessionId` | string | Yes | Unique session identifier from USSD gateway |
| `userInput` | string | No | User's input for the current step |

#### Response

```json
{
  "sessionId": "unique-session-id",
  "message": "Welcome to eSusFarm. Please enter your District:",
  "sessionActive": true
}
```

| Field | Type | Description |
|-------|------|-------------|
| `sessionId` | string | Echo of the session ID |
| `message` | string | Text to display to the user (max 160 characters) |
| `sessionActive` | bool | `true` = session continues, `false` = session ends |

## Registration Flow Example

```bash
# Step 1: Start registration
curl -X POST http://localhost:5243/api/ussd/register \
  -H "Content-Type: application/json" \
  -d '{"phoneNumber": "+256700123456", "sessionId": "session-001", "userInput": ""}'

# Response: "Welcome to eSusFarm. Please enter your District:"

# Step 2: Enter district
curl -X POST http://localhost:5243/api/ussd/register \
  -H "Content-Type: application/json" \
  -d '{"phoneNumber": "+256700123456", "sessionId": "session-001", "userInput": "Kampala"}'

# Response: "Please enter your Farm Size in hectares:"

# Step 3: Enter farm size
curl -X POST http://localhost:5243/api/ussd/register \
  -H "Content-Type: application/json" \
  -d '{"phoneNumber": "+256700123456", "sessionId": "session-001", "userInput": "50"}'

# Response: "Registration complete. Thank you!"
```



## Notes

- Sessions are stored **in-memory**. Restarting the server clears all active sessions.


