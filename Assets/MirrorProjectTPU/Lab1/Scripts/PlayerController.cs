using Mirror;
using UnityEngine;

namespace MirrorProjectTPU.Lab1.Scripts
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float speed = 0.5f;
        private Vector3 _movedVector;
        
        void Update()
        {
            if (!isLocalPlayer)
                return;
            _movedVector.x = Input.GetAxis("Horizontal");
            _movedVector.z = Input.GetAxis("Vertical");
            transform.Translate(Vector3.right * _movedVector.x * (speed * Time.deltaTime));
            transform.Translate(Vector3.up * _movedVector.z * (speed * Time.deltaTime));

            if (Input.GetKeyDown(KeyCode.F))
            {
                CmdSpawnObject();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                CmdSpawnObjectClient();
            }
        }
        
        [Command(requiresAuthority = false)]
        private void CmdSpawnObject()
        {
            var obj = Instantiate(NetworkManager.singleton.spawnPrefabs[Random.Range(0, NetworkManager.singleton.spawnPrefabs.Count)]);
            NetworkServer.Spawn(obj);
            Destroy(obj, 5f);
        }

        [Command(requiresAuthority = false)]
        private void CmdSpawnObjectClient()
        {
            RpcSpawnObject();
        }

        [ClientRpc]
        private void RpcSpawnObject()
        {
            var obj = Instantiate(NetworkManager.singleton.spawnPrefabs[Random.Range(0, NetworkManager.singleton.spawnPrefabs.Count)]);
            Destroy(obj, Random.Range(1, 5));
        }
    }
}
