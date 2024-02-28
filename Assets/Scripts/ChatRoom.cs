using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class ChatRoom : MonoBehaviour
{
    public static ChatRoom instance;

    [SerializeField] Massage massage;
    [SerializeField] Massage massageFromOther;
    [SerializeField] Transform parent;
    [SerializeField] TMP_InputField body;
    [SerializeField] Dropdown users;
    [SerializeField] GameObject Panel;

    WebSocket ws;

    List<Action> actions = new List<Action>();

    private void Awake()
    {
        instance = this;
    }

    public void OpenChat(string id)
    {
        Panel.SetActive(true);

        int userId = PlayerPrefs.GetInt(Keys.ID_KEY);
        string username = PlayerPrefs.GetString(Keys.USERNAME_KEY);

        EstablishServer(new ServerRequestData
        {
            roomId = id,
            userId = userId,
            username = username,
        });
    }

    private void EstablishServer(ServerRequestData request)
    {
        string url = "wss://chatroom-app.liara.run/ws/joinRoom/" + request.roomId + "?userId=" + request.userId.ToString() + "&username=" + request.username;

        ws = new WebSocket(url);

        ws.OnMessage += Ws_OnMessage;
        ws.OnOpen += Ws_OnOpen;
        ws.OnError += Ws_OnError;
        ws.Connect();
    }

    private void Ws_OnError(object sender, ErrorEventArgs e)
    {
        print(e.Message);
    }

    private void Ws_OnOpen(object sender, EventArgs e)
    {
        print("connected");
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        print(e.Data);
        
        actions.Add(() =>
        {
            try
            {
                ServerResponse data = JsonUtility.FromJson<ServerResponse>(e.Data);
                MassageData md = JsonUtility.FromJson<MassageData>(data.content);

                if (data.username != PlayerPrefs.GetString(Keys.USERNAME_KEY))
                    ShowMassage(data.username, data.content, false);
            }
            catch
            {
                print(e.Data + " is not a valid Data");
            }
        });
    }

    private void Update()
    {
        foreach (var action in actions)
        {
            action.Invoke();
        }
        actions.Clear();
    }

    public void OnSendClick()
    {
        string massage = JsonUtility.ToJson(new MassageData
        {
            Body = body.text,
            Date = DateTime.Now.ToString("HH:mm")
        });

        ShowMassage(PlayerPrefs.GetString(Keys.USERNAME_KEY), massage, true);

        print(massage);

        ws.Send(massage);
    }

    private void ShowMassage(string sender, string massageData, bool IsYou)
    {
        MassageData md = JsonUtility.FromJson<MassageData>(massageData);

        var newMassage = Instantiate(IsYou ? massage : massageFromOther, parent);
        newMassage.Init(sender, md.Body, md.Date);
    }

    public void CloseChat()
    {
        ws.Close();
        Panel.SetActive(false);
    }
}

public class ServerRequestData
{
    public string roomId;
    public string username;
    public int userId;
}