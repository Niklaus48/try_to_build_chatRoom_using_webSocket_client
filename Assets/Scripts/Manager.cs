using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject Register;
    [SerializeField] GameObject ChatRoom;

    void Start()
    {
        if (PlayerPrefs.GetInt(Keys.Authorized,0) == 0)
        {
            Register.SetActive(true);
        }
        else
        {
            ChatRoom.SetActive(true);
        }
    }
}
