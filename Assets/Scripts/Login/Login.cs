
using UnityEngine;
using UnityEngine.UI;

public class Login : NetworkObject
{
    public static Login Instance { get; private set; }

    [SerializeField] InputField username;
    [SerializeField] InputField password;
    [SerializeField] Text errorText;
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Rooms;

    private void Awake()
    {
        Instance = this;
    }

    public void Excute()
    {
        var user = new RequestloginData
        {
            username = this.username.text,
            password = this.password.text
        };

        LoginUser(user);       
    }

    public void LoginUser(RequestloginData request)
    {
        string url = BASE_URL + "users/login";
        SendRequest_Post(url, request, Response, Error);
    }

    private void Response(string text)
    {
        var result = JsonUtility.FromJson<ResultLoginData>(text);

        PlayerPrefs.SetInt(Keys.Authorized,1);
        DeliverToChat();
    }
    private void Error(string text)
    {
        print("Login =>   " + text);
        errorText.text = text;
    }

    public void DeliverToChat()
    {
        Panel.SetActive(false);
        Rooms.SetActive(true);
    }

}

[System.Serializable]
public class RequestloginData
{
    public string username;
    public string password;
}

public class ResultLoginData
{
    public string session_id;
    public string access_token;
    public string access_token_expires_at;
    public string refresh_token;
    public string refresh_token_expires_at;
    public RegisterResultData user;
}