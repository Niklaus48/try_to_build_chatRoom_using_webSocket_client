
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    [SerializeField] Text roomName;

    private string id;

    public void init(string name, string id)
    {
        this.id = id;
        this.roomName.text = name;
    }

    public void OnClick()
    {
        ChatRoom.instance.OpenChat(id);
    }
}
