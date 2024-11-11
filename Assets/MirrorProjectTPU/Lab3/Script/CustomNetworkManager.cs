using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MirrorProjectTPU.Lab3.Script
{
    public class CustomNetworkManager : NetworkManager
    {
        public static CustomNetworkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<CustomNetworkManager>();
                }
                return _instance;
            }
        }
        private static CustomNetworkManager _instance;
        
        [Header("The number of scenes during server initialization")]
        [SerializeField] private int instances = 3;

        [Scene, SerializeField] private string gameScene;
        
        public void SetupPlayer()
        {
            //NetworkClient.AddPlayer();
        }
        
        /*public override void OnClientConnect()
        {
            NetworkClient.Ready();
        }*/

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            OnServerAddPlayerDelayed(conn);
        }

        private void OnServerAddPlayerDelayed(NetworkConnectionToClient conn)
        {
            conn.Send(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.LoadAdditive });
            base.OnServerAddPlayer(conn);
            HubManager.Instance.AddPlayerOnRandomScene(conn);
        }

        public override void OnStartServer()
        {
            StartCoroutine(ServerLoadSubScenes());
        }

        public override void OnStopClient()
        {
            StartCoroutine(ClientUnloadSubScenes());
        }
        
        [ServerCallback]
        private IEnumerator ServerLoadSubScenes()
        {
            for (var index = 1; index <= instances; index++)
            {
                yield return SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });

                HubManager.Instance.AddVoidScene();
            }
        }
        
        private IEnumerator ClientUnloadSubScenes()
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
        }
    }
}
