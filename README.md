# SignalForge 🚀

**SignalForge** is a professional, out-of-the-box real-time communication, chat, presence, and logging system built exclusively on .NET 10. It is designed to act as a standalone application you can instantly clone and run, providing a robust backend for any modern chat interface.

## 🚀 Getting Started

1. Clone the repository: `git clone https://github.com/jollydogn/signalr_forge.git`
2. Update the `DefaultConnection` string in `src/SignalForge.Sample/appsettings.json` to point to your PostgreSQL instance.
3. Run Entity Framework migrations:
   ```bash
   cd src/SignalForge.Sample
   dotnet ef migrations add Initial
   dotnet ef database update
   ```
4. Start the API server: `dotnet run --project src/SignalForge.Sample`

---

## 📡 SignalR WebSocket Channels & Usage

The application uses strongly-typed SignalR WebSockets. Connect your client (React, Angular, Vue, Mobile) to:
**Hub URL:** `http://localhost:<port>/hubs/chat`

Once connected, your frontend must listen to the following events dispatched by the server:

### 1. `ReceiveMessage`
Triggered whenever a new message is sent to a group you are in, or directly to you.
```javascript
// Frontend JS Example
connection.on("ReceiveMessage", (messageDto) => {
    console.log("New message received from: ", messageDto.senderId, " Content: ", messageDto.content);
});
```

### 2. `MessageRead`
Triggered when a user reads a message you sent.
```javascript
connection.on("MessageRead", (messageId, userId) => {
    // Update the UI to show double-blue ticks for this messageId
});
```

### 3. `PresenceUpdated`
Triggered when any user comes online or goes offline.
```javascript
connection.on("PresenceUpdated", (presenceDto) => {
    if(presenceDto.isOnline) {
        console.log("User is online!");
    }
});
```

### 4. Typing Indicators (`UserTyping` & `UserStoppedTyping`)
Triggered when someone starts or stops typing in a specific group.
```javascript
connection.on("UserTyping", (groupId, userDisplayName) => {
    console.log(`${userDisplayName} is typing...`);
});
```

---

## 💻 How to Perform Actions (REST API)

While the frontend listens via WebSockets, *actions* (like sending a message or joining a group) are dispatched via the REST API or Hub Methods.

### 1. How to Send a Message
To send a message, make a `POST` request to the Message endpoint. If you are sending a direct message, `GroupId` should be the ID of your 1:1 chat channel.

**Endpoint:** `POST /api/chat/messages`
**Body:**
```json
{
  "groupId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "content": "Hello everyone!"
}
```
*Behind the scenes: The server saves the message to PostgreSQL and automatically triggers the `ReceiveMessage` WebSocket event to all members of that group.*

### 2. How to Join a Group
Users can join existing groups to start receiving their messages.

**Endpoint:** `POST /api/groups/{groupId}/join`
**Body:**
```json
{
  "userDisplayName": "JohnDoe"
}
```
*Behind the scenes: The API registers the user in `ChatGroupMember` and triggers the `UserJoinedGroup` SignalR broadcast.*

### 3. How to Get a Read Receipt (Mark as Read)
When the user's screen displays a message, the frontend should notify the backend that it was read.

**Endpoint:** `POST /api/chat/messages/{messageId}/read`
*Empty Body*

*Behind the scenes: Creates a `MessageReadReceipt` record in the database preventing duplicate reads, and broadcasts the `MessageRead` socket event back to the original sender.*

### 4. Creating a 1:1 Direct Message (DM) Channel
Direct messages are just special groups with exactly 2 people under the hood.

**Endpoint:** `POST /api/groups/direct/{otherUserId}`
*Returns:* The `GroupDto` of the 1:1 channel. If one doesn't exist, it creates it automatically.

---

## 🛡️ Audit and Activity Logging

SignalForge ships with automatic logging filters. Any HTTP request will be logged into the `sf_request_logs` table automatically.

Furthermore, business methods in the Controller are tagged with `[ActivityLogging]`. For example, joining a group automatically writes a "GroupJoined" activity trail to the database (`sf_activity_logs`), making auditing incredibly easy.
