
using System;
using UnityEngine;
using UnityEngine.UI;

public class Register : NetworkObject
{
    public static Register Instance { get; private set; }

    [SerializeField] InputField username;
    [SerializeField] InputField password;
    [SerializeField] InputField email;
    [SerializeField] InputField dispaly_name;
    [SerializeField] Text errorText;
    [SerializeField] GameObject Panel;

    private void Awake()
    {
        Instance = this;
    }

    public void Excute()
    {

        var data = new RegisterRequestData
        {
            username = username.text,
            password = password.text,
            display_name = dispaly_name.text,
            Email = email.text,
        };

        RegisterNewUser(data); 
    }

    public void RegisterNewUser(RegisterRequestData request)
    {
        string url = BASE_URL + "users/register";
        SendRequest_Post(url, request, Response, Error);
    }

    private void Response(string text)
    {
        SaveDataIntoClient(JsonUtility.FromJson<RegisterResultData>(text));

        Login.Instance.LoginUser(new RequestloginData
        {
            username = username.text,
            password = password.text
        });
        Panel.SetActive(false);
    }

    private void Error(string text)
    {
        print("Register =>   " + text);
        errorText.text = text;
    }

    private void SaveDataIntoClient(RegisterResultData result)
    {
        PlayerPrefs.SetInt(Keys.ID_KEY,result.id);
        PlayerPrefs.SetString(Keys.USERNAME_KEY, result.username);
        PlayerPrefs.SetString(Keys.NAME_KEY, result.display_name);
    }
}



[System.Serializable]
public class RegisterRequestData
{
    public string username;
    public string password;
    public string display_name;
    public string Email;
}

[System.Serializable]
public class RegisterResultData
{
    public int id;
    public string username;
    public string display_name;
    public string email;
}