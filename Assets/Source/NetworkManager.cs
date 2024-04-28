using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;
using UnityEditor;
using R3;

public class NetworkManager : Singleton<NetworkManager>
{
    public bool IsOnline { get; protected set; }
    private WebSocket ws;
    public string RoomName = "";
    public Subject<bool> JoinRoomSubject = new Subject<bool>();
    public Subject<int> AllCountSubject = new Subject<int>();
    public Subject<(string, int)> PlayerCountSubject = new Subject<(string, int)>(); 
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
            Debug.Log("WebSocket Open");
            IsOnline = true;
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Data: " + e.Data);
            string[] str_array = e.Data.Split(':');
            switch (str_array[0])
            {
                case "Join":
                    {
                        if (str_array[1] == "Success")
                        {
                            IsJoinedRoom = true;
                            JoinRoomSubject.OnNext(true);
                        }
                        else
                        {
                            IsJoinedRoom = false;
                            JoinRoomSubject.OnNext(false);
                        }
                        break;
                    }
                case "Count":
                    {
                        int allCount = int.Parse(str_array[1]);
                        string id = str_array[2];
                        int count = int.Parse(str_array[3]);
                        AllCountSubject.OnNext(allCount);
                        PlayerCountSubject.OnNext((id, count));
                        break;
                    }
                default:
                    break;
            }

        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket Close");
            IsOnline = false;
        };
        ws.ConnectAsync();
    }

    public void ResetGame()
    {
        ws.Send("Clear");
        IsJoinedRoom = false;
    }

    public void SendClickedCount(int ClickCount)
    {
        ws.SendAsync("Count:" + ClickCount.ToString(), (b) => { });
    }

    public void JoinRoom()
    {
        ws.SendAsync("CreateRoom:" + RoomName, (b) => { });
    }
}
