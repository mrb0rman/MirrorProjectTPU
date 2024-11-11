
namespace MirrorProjectTPU.Lab3.Script.NetworkMessage
{
    public struct ServerMessageToPlayer : Mirror.NetworkMessage
    {
        public StateHub StateHub;
        public string Nickname;
    }
}
