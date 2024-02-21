using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Massage : MonoBehaviour
{
    [SerializeField] Text Sender;
    [SerializeField] Text Body;
    [SerializeField] Text Date;

    public void Init(string sender,string body, string date)
    {
        Sender.text = sender;
        Body.text = body;
        Date.text = date;
    }

}
