using UnityEngine;
using System.Collections;
using WebSocketSharp;

public class ClientExample : MonoBehaviour
{

    private WebSocket ws;

    void Start()
    {
        ws = new WebSocket("ws://localhost:3000/");
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket Open");
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Data: " + e.Data);
        };

        ws.OnError += (sender, e) =>
        {
            Debug.Log("WebSocket Error Message: " + e.Message);
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket Close");
        };

        ws.Connect();

    }

    void Update()
    {

        if (Input.GetKeyUp("s"))
        {
            ws.Send("Test Message");
        }

    }

    void OnDestroy()
    {
        ws.Close();
        ws = null;
    }
}