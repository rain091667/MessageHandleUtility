# MessageHandleUtility

    C# Socket Message Module
	
## Chat Message Client Server DEMO

![](https://raw.githubusercontent.com/rain091667/MessageHandleUtility/master/ScreenDemo/SampleScreen.gif)

![](https://raw.githubusercontent.com/rain091667/MessageHandleUtility/master/ScreenDemo/SampleScreenServer.png)


# Usage

## DLL Reference
	
```csharp
using MessageHandleUtility;
```

### Implement IMessageSocketSession
```csharp
class ConnectionHandle : IMessageSocketSession
{
	void OnNewConnection(string TargetID, IMessageSocketInformation info, out bool AcceptFlag)
	{
		// accept connection
		AcceptFlag = true;
		// reject connection
		AcceptFlag = false;
		
		// 檢查連線的對象 TargetID (連線 ID), info (對方連線的基本資訊)
		// 在此進行連線的決策，拒絕或接受該連線。
	}
	void OnConnectionDisconnected(string TargetID, IMessageSocketInformation info)
	{
		// TargetID (連線 ID), info (對方連線的基本資訊)
		// 在連線列表中，有斷線發生時通知。
	}
	void OnConnectFailed(string TargetID, IMessageSocketInformation info)
	{
		// TargetID (連線 ID), info (對方連線的基本資訊)
		// 進行連接時失敗，通知連線失敗。
	}
}
```


### Implement IMessageHandler
```csharp
class MessageControler : IMessageHandler
{
	public override void OnMessageReceive(IMessageContent msg)
	{
		// 當有訊息接收時，通知該實體。
	}
}
```

### init

```csharp
	string LocalID = "yourID";
	string ListenDeviceID = "ListenDeviceID";
	ConnectionHandle ConnectionHandleManager = new ConnectionHandle();
	MessageSocketManager manager = new MessageSocketManager(LocalID, ConnectionHandleManager);
	MessageControler mMessageControler = new MessageControler();
	manager.addMessageHandler(ListenDeviceID, mMessageControler);
```

### Establish Connection

```csharp
	string Server_IP = "IP"; // Ex: 127.0.0.1, 192.168.1.2
	ushort Server_Port = 38500;
	string[] Descriptions = new string[] { "message", "message2" };
	manager.Connect(new IPHostEndPoint(Server_IP, Server_Port), Descriptions); 
```

### Send Message
```csharp
	if (mMessageControler.IsSendAvailable)
	{
		// Example:
		mMessageControler.SendMessage("Message");
	}
```

### Receive Message
```csharp
class MessageControler : IMessageHandler
{
	public override void OnMessageReceive(IMessageContent msg)
	{
		TimeSpan? time = msg.Data_TimeSpanInstance; // Time
		string time_String = msg.Data_TimeTick_String; // Time
		string MessageContent = msg.Data_MessageContent; // Message
	}
}
```


# Structure Message

### JSON Message - Structure
```csharp
using System.Runtime.Serialization;
using MessageHandleUtility.Utility;
```

```csharp
[DataContract]
class MessageContract
{
	[DataMember(Name = "Name")]
	public string Name { get; set; }

	[DataMember(Name = "PhoneNumber")]
	public string PhoneNumber { get; set; }
	
	[DataMember(Name = "Message")]
	public string Message { get; set; }

	public MessageContract()
	{
		Name = string.Empty;
		PhoneNumber = string.Empty;
		Message = string.Empty;
	}
}
```

### JSON Message - Send
```csharp
	MessageContract mContract = new MessageContract();
	mContract.Name = "Name";
	mContract.PhoneNumber = "123456789";
	mContract.Message = "Your Message.";
	
	string JSONString = SerializeUtility.Serialize(mContract);
	if (mMessageControler.IsSendAvailable)
	{
		mMessageControler.SendMessage(JSONString);
	}
```

### JSON Message - Receive
```csharp
class MessageControler : IMessageHandler
{
	public override void OnMessageReceive(IMessageContent msg)
	{
		MessageContract data = SerializeUtility.DeSerialize<MessageContract>(msg.Data_MessageContent);
		string Name = data.Name; // receive: Name
		string PhoneNumber = data.PhoneNumber; // receive: 123456789
		string Message = data.Message; // receive: Your Message.
	}
}
```