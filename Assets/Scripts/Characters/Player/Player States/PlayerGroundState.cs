using UnityEngine;

[System.Serializable]
public class PlayerGroundState : PlayerState
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stickForce;
    [SerializeField] private float jumpSpeed;

    private bool canJump;

    public override void StartState(PlayerController player)
    {
        player.VerticalSpeed = stickForce;

        canJump = false;
    }

    public override void UpdateState(PlayerController player)
    {
        Vector3 direction = Camera.main.transform.right * InputHub.instance.Move.x + Camera.main.transform.forward * InputHub.instance.Move.y;
        direction.y = 0f;
        player.MoveDirection = direction.normalized;

        player.CurrentSpeed = moveSpeed * InputHub.instance.Move.magnitude;
        player.LookDirection = player.MoveDirection != Vector3.zero ? player.MoveDirection : player.LookDirection;

        player.FaceDirection();

        if (InputHub.instance.Jump && canJump)
        {
            player.VerticalSpeed = jumpSpeed;
        }

        canJump = !InputHub.instance.Jump;
    }

    public override void ChangeState(PlayerController player)
    {
        if (player.VerticalSpeed > 0f || !Physics.CheckSphere(player.GroundCheckPosition, player.Radius, LayerMask.GetMask("Solid")))
        {
            player.SetState(player.AirState);
        }
    }

    public override void ExitState(PlayerController player)
    {
        
    }
}
