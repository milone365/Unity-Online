using UnityEngine;
using Unity.Networking.Transport;
public class NetPlayerPosition : NetMessage
{
    public int PlayerID { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }

    public NetPlayerPosition()
    {
        Code = OpCode.PLAYER_POSITION;
    }
    public NetPlayerPosition(DataStreamReader reader)
    {
        Code= OpCode.PLAYER_POSITION;
        Deserialize(reader);
    }
    public NetPlayerPosition(int ID,float inx,float iny,float inz)
    {
        Code = OpCode.PLAYER_POSITION;
        x = inx;
        y = iny;
        z = inz;
        PlayerID = ID;
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(PlayerID);
        writer.WriteFloat(x);
        writer.WriteFloat(y);
        writer.WriteFloat(z);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        PlayerID = reader.ReadInt();
        x = reader.ReadFloat();
        y = reader.ReadFloat();
        z = reader.ReadFloat();
    }

    public override void RecieveOnClient()
    {
        Debug.Log("Client " + PlayerID + ":: [" + x + " / "+ y +" / "+ z + "]");
    }
    public override void RecieveOnServer()
    {
        Debug.Log("Server " + PlayerID + ":: [" + x + " / " + y + " / " + z + "]");
    }
}
