using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

public class NetworkChatMessage : NetMessage
{
    public FixedString128Bytes messageText { get; set; }
    public NetworkChatMessage()
    {
        Code = OpCode.CHAT_MESSAGE;
    }
    public NetworkChatMessage(DataStreamReader reader)
    {
        Code = OpCode.CHAT_MESSAGE;
        Deserialize(reader);
    }
    public NetworkChatMessage(FixedString128Bytes msg)
    {
        Code = OpCode.CHAT_MESSAGE;
        messageText = msg;
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteFixedString128(messageText);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        //first byte is handle
        messageText = reader.ReadFixedString128();
    }

    public override void RecieveOnServer(Server server)
    {
        Debug.Log("Server:: " + messageText);
        base.RecieveOnServer(server);
    }

    public override void RecieveOnClient()
    {
        Debug.Log("Client:: " + messageText);
    }
}
