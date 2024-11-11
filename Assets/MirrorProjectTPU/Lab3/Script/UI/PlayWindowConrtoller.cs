using TMPro;
using UnityEngine;

namespace MirrorProjectTPU.Lab3.Script.NetworkMessage
{
    public class PlayWindowConrtoller : MonoBehaviour
    {
        [SerializeField] private TMP_Text textNickname;

        public void SetNickname(string nickname)
        {
            textNickname.text = $"You nickname: {nickname}";
        }
    }
}