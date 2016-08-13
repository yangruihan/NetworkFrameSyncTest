using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

    public PlayerController controller;
    public PlayerMotor motor;

    [Server]
    public void SendServerCommand()
    {
        RpcAcceptServerCommand();
    }

    [ClientRpc]
    public void RpcAcceptServerCommand()
    {
        if (!isLocalPlayer) return;

        controller.HandleUserInput();
    }
}
