using Unity.Collections;
using UnityEngine;
using Unity.Networking.Transport;

public class Client : MonoBehaviour
{
    public NetworkDriver driver;
    protected NetworkConnection connection;

#if UNITY_EDITOR
    private void Start() { Init(); }
    private void Update() { UpdateServer(); }
    private void OnDestroy() { Shutdown(); }
#endif
    public virtual void Init()
    {
        driver = NetworkDriver.Create();
        connection = default(NetworkConnection);
        var endpoint = NetworkEndPoint.LoopbackIpv4; 
        //same port with server
        endpoint.Port = 5522;
        //connet with driver
        connection = driver.Connect(endpoint);
    }
    public virtual void Shutdown()
    {
        driver.Dispose();
    }
    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete();
        CheckAlive();
        UpdateMessagePump();
    }
    void CheckAlive()
    {
        if(!connection.IsCreated)
        {
            Debug.Log("lost connection to server");
        }
    }
    protected virtual void UpdateMessagePump()
    {
        DataStreamReader stream;
 
            NetworkEvent.Type cmd;
            while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty)
            {
                if(cmd == NetworkEvent.Type.Connect)
                {
                    Debug.Log("Connected to server");
                 }
                else if (cmd == NetworkEvent.Type.Data)
                {
                    uint number = stream.ReadByte();
                    Debug.Log("Got " + number + "from the Client");
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    connection = default(NetworkConnection);
                }           
        }
    }

    public virtual void SendToServer(NetMessage msg)
    {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }

}
