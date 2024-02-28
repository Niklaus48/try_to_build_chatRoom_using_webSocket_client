
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkObject : MonoBehaviour
{
    public delegate void NetworkAction(string massage);
    public delegate void NetworkAction_DownloadHandler(DownloadHandler downloadHandler);
    public bool debug;
    public string BASE_URL = "https://chatroom-app.liara.run/";

    protected void LogError(object massage)
    {
        if (debug)
        {
            Debug.LogError(massage);
        }
    }
    protected void Log(object massage)
    {
        if (debug)
        {
            Debug.Log(massage);
        }
    }
    public void SendRequest_Post(string url, object data, NetworkAction response, NetworkAction error)
    {
        StartCoroutine(_sendRequest_Post(url, data, response, error));

    }

    IEnumerator _sendRequest_Post(string url, object data, NetworkAction response, NetworkAction error)
    {

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        if (data != null)
        {
            var rawbody = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(rawbody);
            request.SetRequestHeader("Content-Type", "application/json");
        }

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                LogError(url + ": Error: " + request.error);
                error("connect error : " + request.error);
                error(request.downloadHandler.text);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                LogError(url + ": Error: " + request.error);
                error(": Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                LogError(url + ": HTTP Error: " + request.error);
                error(request.downloadHandler.text);
                break;
            case UnityWebRequest.Result.Success:
                Log(url + ":\nReceived: " + request.downloadHandler.text);
                response(request.downloadHandler.text);
                request.Dispose();
                break;
        }
    }

    public void SendRequest_Put(string url, object data, NetworkAction response, NetworkAction error)
    {
        StartCoroutine(_SendRequest_Put(url, data, response, error));
    }

    IEnumerator _SendRequest_Put(string url, object data, NetworkAction response, NetworkAction error)
    {

        var rawbody = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));

        UnityWebRequest request = UnityWebRequest.Put(url, rawbody);

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();


        switch (request.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                LogError(url + ": Error: " + request.error);
                error("connect error : " + request.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                LogError(url + ": Error: " + request.error);
                error(": Error: " + request.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                LogError(url + ": HTTP Error: " + request.error);
                //error(":HTTP Error: " + request.error);
                error(request.downloadHandler.text);
                break;
            case UnityWebRequest.Result.Success:
                Log(url + ":\nReceived: " + request.downloadHandler.text);
                response(request.downloadHandler.text);
                request.Dispose();
                break;

        }

    }

    public void SendRequest_Get(string url, NetworkAction response, NetworkAction error)
    {

        StartCoroutine(_sendRequest_Get(url, response, error));

    }

    IEnumerator _sendRequest_Get(string url, NetworkAction response, NetworkAction error)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            switch (www.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    LogError(url + ": Error: " + www.error);
                    error("connect error : " + www.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    LogError(url + ": Error: " + www.error);
                    error(": Error: " + www.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    LogError(url + ": HTTP Error: " + www.error);
                    error(www.downloadHandler.text);
                    break;
                case UnityWebRequest.Result.Success:
                    Log(url + ":\nReceived: " + www.downloadHandler.text);
                    response(www.downloadHandler.text);
                    www.Dispose();
                    break;
            }
        }
    }


    public void SendRequest_Get_DownloadHandler(string url, NetworkAction_DownloadHandler response, NetworkAction error)
    {
        StartCoroutine(_sendRequest_Get_DownloadHandler(url, response, error));

    }

    IEnumerator _sendRequest_Get_DownloadHandler(string url, NetworkAction_DownloadHandler response, NetworkAction error)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            switch (www.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    LogError(url + ": Error: " + www.error);
                    error("connect error : " + www.error);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    LogError(url + ": Error: " + www.error);
                    error(": Error: " + www.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    LogError(url + ": HTTP Error: " + www.error);
                    error(":HTTP Error: " + www.error);
                    break;
                case UnityWebRequest.Result.Success:
                    //Log(url + ":\nReceived: " + www.downloadHandler.text);
                    response(www.downloadHandler);
                    break;
            }
        }
    }
}
