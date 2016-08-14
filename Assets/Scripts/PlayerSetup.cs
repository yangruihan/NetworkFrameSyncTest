using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Ruihanyang.Game
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerSetup : NetworkBehaviour
    {
        [SerializeField]
        private Behaviour[] componentsToDisable;

        void Start()
        {
            if (!isLocalPlayer)
            {
                DisableComponents();
            }
            else
            {
                Player _player = GetComponent<Player>();
                GameManager.LocalPlayer = _player;
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            string _netID = GetComponent<NetworkIdentity>().netId.ToString();
            Player _player = GetComponent<Player>();

            GameManager.RegisterPlayer(_netID, _player);
        }

        void OnDisable()
        {
            GameManager.UnRegisterPlayer(transform.name);
        }

        void DisableComponents()
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
    }
}
