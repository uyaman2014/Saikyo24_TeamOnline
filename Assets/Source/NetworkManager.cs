using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using R3;

public class NetworkManager : Singleton<NetworkManager>
{
    public class PlayerInfo
    {
        public bool bIsSelf;
        public int Count;
        public bool bIsReady;
    }

    private Dictionary<string, PlayerInfo> _playerInfos = new Dictionary<string, PlayerInfo>();
    public Dictionary<string, PlayerInfo> PlayerInfos { get { return new Dictionary<string, PlayerInfo>(_playerInfos); } } //非同期にplayerinfoが追加されるので、MonoBehaviourとかから参照する際はコピーを渡す
    public bool IsOnline { get; protected set; }
    private WebSocket ws;
    public string RoomName = "";

    public bool IsJoinedRoom { get; protected set; }

    public NetworkManager()
    {
        IsOnline = false;
        ws = new WebSocket("ws://localhost:3000/");
        Subscribe();
    }

    public void Subscribe()
    {
        ws.OnOpen += (sender, e) =>
        {
            IsOnline = true;
        };

        ws.OnMessage += (sender, e) =>
        {
            string[] str_array = e.Data.Split(':');
            string type = str_array[0];
            string pID = str_array[1];
            switch (type)
            {
                case "Join":
                    {
                        switch (pID)
                        {
                            case "Success":
                                {
                                    var pInfo = new PlayerInfo() { bIsSelf = true, Count = 0, bIsReady = false };
                                    _playerInfos[str_array[2]] = pInfo;
                                    IsJoinedRoom = true;
                                    break;
                                }
                            case "Other":
                                {
                                    var pInfo = new PlayerInfo() { bIsSelf = false, Count = 0, bIsReady = false };
                                    _playerInfos[str_array[2]] = pInfo;
                                    break;
                                }
                            case "FullRoom":
                                {
                                    _playerInfos.Clear();
                                    IsJoinedRoom = false;
                                    break;
                                }
                            default:
                                break;
                        }
                        break;
                    }
                case "Count":
                    {
                        int count = int.Parse(str_array[2]);
                        _playerInfos[pID].Count = count;
                        break;
                    }
                case "Ready":
                    {
                        bool bIsReady = bool.Parse(str_array[2]);
                        _playerInfos[pID].bIsReady = bIsReady;
                        break;
                    }
                case "Close":
                    {
                        _playerInfos.Remove(pID);
                        break;
                    }
                default:
                    break;
            }

        };

        ws.OnError += (sender, e) =>
        {
        };

        ws.OnClose += (sender, e) =>
        {
            IsOnline = false;
        };
        ws.ConnectAsync();
    }

    public void ResetGame()
    {
        ws.Send("Clear");
        IsJoinedRoom = false;
    }

    public void Close()
    {
        ws.CloseAsync();
    }

    public void Open()
    {
        ws.ConnectAsync();
    }

    public void SendClickedCount(int ClickCount)
    {
        ws.SendAsync("Count:" + ClickCount.ToString(), (b) => { });
    }

    public void JoinRoom()
    {
        ws.SendAsync("CreateRoom:" + RoomName, (b) => { });
        _playerInfos.Clear();
    }

    public void Ready()
    {
        ws.SendAsync("Ready", (b) => { });
    }
}
