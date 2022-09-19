using UnityEngine;
using Unity.Networking.Transport;
using System;

public class NetMessage 
{
   public OpCode Code { set; get; }

    public virtual void Serialize(ref DataStreamWriter writer)
    {

    }

    public virtual void Deserialize(DataStreamReader reader)
    {

    }

    public virtual void RecieveOnServer(Server server)
    {
        server.BroadCast(this);
    }
    public virtual void RecieveOnClient()
    {

    }
}
