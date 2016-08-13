using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Player player;

    public void HandleUserInput()
    {
        // 处理位移输入
        HandleMoveInput();
    }

    private void HandleMoveInput()
    {
        float _x = Input.GetAxis("Horizontal");
        float _y = Input.GetAxis("Vertical");

        Vector3 _moveDirection = new Vector3(_x, _y, 0f);

        player.motor.Move(_moveDirection);
    }
}
