using System.Collections.Generic;
using Mirror;
using MirrorProjectTPU.Lab3.Script.NetworkMessage;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MirrorProjectTPU.Lab3.Script
{
    public class HubManager : MonoBehaviour
    {
        public static HubManager Instance
        {
            get
            {
                if (_hubManager == null)
                {
                    _hubManager = FindFirstObjectByType<HubManager>();
                }
                return _hubManager;
            }
        }
        private static HubManager _hubManager;
        
        private List<Scene> _listScene = new();
        private List<string> _listNickname = new();
        private Dictionary<NetworkConnectionToClient, PlayerSetting> _dictPlayer= new();
        
        private void Start()
        {
            NetworkServer.RegisterHandler<PlayerMessageToServer>(OnPlayerMessageToServer);
        }

        public void AddVoidScene()
        {
            var newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            _listScene.Add(newScene);
            
            Debug.Log($"Create void scene");
        }

        public void AddPlayerOnRandomScene(NetworkConnectionToClient conn)
        {
            SceneManager.MoveGameObjectToScene(conn.identity.gameObject, _listScene[Random.Range(0, _listScene.Count)]);
        }
        
        private void OnPlayerMessageToServer(NetworkConnectionToClient conn, PlayerMessageToServer msg)
        {
            if (!_listNickname.Contains(msg.Nickname))
            {
                conn.Send(new ServerMessageToPlayer()
                {
                    StateHub = StateHub.Nickname,
                    Nickname = msg.Nickname
                });
                
                _listNickname.Add(msg.Nickname);
                _dictPlayer.Add(conn, new PlayerSetting(){Nickname = msg.Nickname});
                return;
            }
            conn.Send(new ServerMessageToPlayer()
            {
                StateHub = StateHub.Nickname,
                Nickname = ""
            });
        }
    }
    public struct PlayerSetting
    {
        public string Nickname;
    }
    
    public enum StateHub
    {
        Nickname,
        Password
    }
}

