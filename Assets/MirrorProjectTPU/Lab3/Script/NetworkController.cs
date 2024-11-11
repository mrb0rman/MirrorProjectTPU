using Mirror;
using UnityEngine;

namespace MirrorProjectTPU.Lab3.Script
{
    public class NetworkController : MonoBehaviour
    {
        public void ConnectServer()
        {
            NetworkManager.singleton.StartClient();
        }
    }
}
