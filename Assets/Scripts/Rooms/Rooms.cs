
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rooms : NetworkObject
{
    [SerializeField] Room Room;
    [SerializeField] Transform Parent;
    [SerializeField] InputField RoomName;

    private void OnEnable()
    {
        Execute();
    }

    public void Execute()
    {
        string url = BASE_URL + "ws/getRooms";
        print("login_Token =>  " + PlayerPrefs.GetString(Keys.TOKEN_KEY));
        SendRequest_Get(url, Response, Error);
    }

    public void Response(string text)
    {
        print("Rooms =>   " + text);
        string FixedText = $"{{\"rooms\":{text}}}";

        var data = JsonUtility.FromJson<RoomsData>(FixedText);

        foreach (var room in data.rooms)
        {
            MakeRoom(room);
        }
    }

    public void Error(string text)
    {
        print(text);
    }

    public void MakeRoom(RoomData room)
    {
        Room newRoom = Instantiate(Room, Parent);
        newRoom.init(room.name, room.id);
    }

    public void CreateNewRoom()
    {
        string url = BASE_URL + "ws/createRoom";

        var newRoom = new CreateRoomRequestData
        {
            id = Guid.NewGuid().ToString(),
            name = RoomName.text
        };

        SendRequest_Post(url, newRoom, OnCreateRoomSuccess, OnCreateRoomError);
    }

    public void OnCreateRoomSuccess(string text)
    {
        MakeRoom(JsonUtility.FromJson<RoomData>(text));
    }

    public void OnCreateRoomError(string text)
    {
        print(text);
    }
}

[System.Serializable]
public class RoomsData
{
    public List<RoomData> rooms;
}

[System.Serializable]
public class RoomData
{
    public string id;
    public string name;
}

[System.Serializable]
public class CreateRoomRequestData
{
    public string id;
    public string name;
}