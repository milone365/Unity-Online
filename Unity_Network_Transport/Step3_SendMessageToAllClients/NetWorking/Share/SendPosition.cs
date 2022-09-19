using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPosition : MonoBehaviour
{
    float lastSend;
    Client client;

    private void Start()
    {
        client = FindObjectOfType<Client>();
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal")*10*Time.deltaTime;
        float z = Input.GetAxis("Vertical")*10*Time.deltaTime;
        transform.Translate(x, 0, z);
        if(Time.time-lastSend>1)
        {
            NetPlayerPosition net = new NetPlayerPosition(337, transform.position.x,transform.position.y,transform.position.z);
            client.SendToServer(net);
            lastSend = Time.time;
        }
    }
}
