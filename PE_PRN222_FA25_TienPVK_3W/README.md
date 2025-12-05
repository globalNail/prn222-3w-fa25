## Learn Web Application with Asp.Net Core Razor Pages and SignalR

### Introduction

- Razor Pages
- SignalR

### Overview

Set up a simple web application using ASP.NET Core Razor Pages and SignalR to create real-time web functionality. 

### Prerequisites

- LibMan CLI Tool (Install required libraries to run the application)
 
	Install the LibMan CLI tool globally using the .NET Core CLI with the following command:
```cmd 
		dotnet tool install -g Microsoft.Web.LibraryManager.Cli
```

### Knowledge 

- SingalR

This code defines a SignalR hub that allows clients to send messages to all connected clients in real-time.

```csharp
		public class ChatHub : Hub
		{
			public async Task SendMessage(string user, string message)
			{
				await Clients.All.SendAsync("ReceiveMessage", user, message);
			}
		}
```

connection.on Receiving messages from the SignalR hub on the client side using JavaScript. (event listener)

```javascript
		connection.on("ReceiveMessage", (user, message) => {
			console.log("Message:", user, message);
		});
```

connection.start Starting the SignalR connection on the client side using JavaScript.

```javascript
		await connection.start();
```
> connection.on always defined before connection.start to ensure event listeners are set up before the connection is established.

#### How to send messages to the SignalR hub from the client side?

```javascript
		connection.invoke("SendMessage", user, message);
```