# MessageHandleUtility

    C# Socket Message Module
	
![](https://github.com/rain091667/AndroidBloodProgressView/raw/master/ScreenDemo/screen.gif)

## Usage

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