using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatField : MonoBehaviour
{
    InputField field;
    Client client;
    // Start is called before the first frame update
    void Start()
    {
        field = GetComponent<InputField>();
        client = FindObjectOfType<Client>();
    }

    public void OnSubmitEvent()
    {
        NetworkChatMessage msg = new NetworkChatMessage(field.text);
        client.SendToServer(msg);
    }
}
