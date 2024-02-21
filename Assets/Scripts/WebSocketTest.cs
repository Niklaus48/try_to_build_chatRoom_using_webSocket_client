using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class WebSocketTest : MonoBehaviour
{
    [SerializeField] Massage massage;
    [SerializeField] Massage massageFromOther;
    [SerializeField] Transform parent;
    [SerializeField] TMP_InputField body;
    bool isYou;
    WebSocket ws;

    List<Action> actions = new List<Action>();

    private void Start()
    {
        isYou = true;
        ws = new WebSocket("ws://127.0.0.1:8888/chat");
        ws.OnMessage += Ws_OnMessage;
        ws.Connect();
    }

    

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        actions.Add(() =>
        {

            MassageData md = JsonUtility.FromJson<MassageData>(e.Data);
            print(md.Body);
            if (isYou)
            {
                var newMassage = Instantiate(massage, parent);
                newMassage.Init(md.Sender, md.Body, md.Date);
            }
            else
            {
                var newMassage = Instantiate(massageFromOther, parent);
                newMassage.Init(md.Sender, md.Body, md.Date);
            }
            isYou = !isYou;
            print("Server Massage : " + e.Data);
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
            Sender = "Amin",
            Body = body.text,
            Date = DateTime.Now.ToString("HH:mm")
        });

        ws.Send(massage);
    }
}
