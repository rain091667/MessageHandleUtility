# MessageHandleUtility

    C# Socket Message Module
	
![](https://github.com/rain091667/AndroidBloodProgressView/raw/master/ScreenDemo/screen.gif)

## Usage

### Implement IMessageSocketSession
```c-sharp
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
		// 在進行連接的時候如果失敗，通知連線失敗。
	}
}
```


### Implement IMessageHandler
```c-sharp
class MessageControler : IMessageHandler
{
	public override void OnMessageReceive(IMessageContent msg)
	{
		// 當有訊息接收時，通知該實體。
	}
}
```