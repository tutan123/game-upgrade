namespace FrameWork;

public enum NetMessageType
{
	PlayerJoinRoom = 1,
	PlayerLeftRoom,
	JoinError,
	Information,
	Transform,
	Instantiate,
	BelongingClient,
	Rpc,
	ConnectToServer,
	DisConnectToServer,
	Destroy,
	RoomInfo,
	InstantiateEnd,
	ReLink
}
