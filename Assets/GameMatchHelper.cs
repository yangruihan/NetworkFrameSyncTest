using UnityEngine;
using System.Threading;
using System.Collections;
using Prototype.NetworkLobby;

namespace Ruihanyang.Game
{

    public class GameMatchHelper : MonoBehaviour
    {
        [SerializeField]
        private LobbyManager lobbyManager;
        [SerializeField]
        private LobbyMainMenu lobbyMainMenu;

        [SerializeField]
        private GameObject showInfoPanel;

        public void OnStartMatchButtonClick()
        {
            showInfoPanel.SetActive(true);

            GameMatchUtil.SendLocalIpMessageToServer();
            Thread thread = new Thread(GameMatchUtil.AcceptHostIpMessageFromServer);
            thread.Start();

            StartCoroutine(WaitForHostIp());
        }

        IEnumerator WaitForHostIp()
        {
            while (string.IsNullOrEmpty(GameMatchUtil.HostIp))
                yield return new WaitForSeconds(0.1f);

            if (GameMatchUtil.HostIp == GameMatchUtil.LocalIp)
            {
                lobbyMainMenu.OnClickHost();
            }
            else
            {
                lobbyMainMenu.OnClickJoin();
            }

            showInfoPanel.SetActive(false);
        }
    }
}
