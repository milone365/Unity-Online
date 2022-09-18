using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
public class Server : MonoBehaviour
{
    public NetworkDriver driver;
    protected NativeList<NetworkConnection> connections;

#if UNITY_EDITOR
    private void Start() { Init(); }
    private void Update() { UpdateServer(); }
    private void OnDestroy() { Shutdown(); }
#endif
    public virtual void Init()
    {
        driver = NetworkDriver.Create();
        var endpoint = NetworkEndPoint.AnyIpv4; //who can connect to us
        endpoint.Port = 5522;
        if (driver.Bind(endpoint) != 0)
            Debug.Log("error binding to port " + endpoint.Port);
        else
            driver.Listen();

        //init list
        connections = new NativeList<NetworkConnection>(4, Allocator.Persistent);
    }
    public virtual void Shutdown()
    {
        driver.Dispose();
        connections.Dispose();
    }
    public virtual void UpdateServer()
    {
        driver.ScheduleUpdate().Complete();
        //update logout 
        CleanUpConnection();
        //login
        AcceptNewConnections();

        UpdateMessagePump();
    }
    void CleanUpConnection()
    {
        for(int i =0;i<connections.Length;i++)
        {
            if(!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }
        }
    }
    void AcceptNewConnections()
    {
        NetworkConnection c;
        while((c=driver.Accept())!=default(NetworkConnection))
        {
            connections.Add(c);
            Debug.Log("accepted a connection");
        }
    }
    protected virtual void UpdateMessagePump()
    {
        DataStreamReader stream;
        for(int i=0;i<connections.Length;i++)
        {
            NetworkEvent.Type cmd;
            while((cmd=driver.PopEventForConnection(connections[i],out stream))!=NetworkEvent.Type.Empty)
            {
                if(cmd==NetworkEvent.Type.Data)
                {
                    uint number = stream.ReadByte();
                    Debug.Log("Got " + number + "from the Client");
                }
                else if(cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    connections[i] = default(NetworkConnection);
                }
            }
        }
    }
}