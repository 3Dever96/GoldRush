using UnityEngine;

[System.Serializable]
public class PlayerAirState : PlayerState
{
    [SerializeField] private float airSpeed;
    [SerializeField] private float jumpGravity;
    [SerializeField] private float fallGravity;
    [SerializeField] private float fallSpeed;

    private float gravity;

    public override void StartState(PlayerController player)
    {
        
    }

    public override void UpdateState(PlayerController player)
    {
        Vector3 direction = Camera.main.transform.right * InputHub.instance.Move.x + Camera.main.transform.forward * InputHub.instance.Move.y;
        direction.y = 0f;
        player.MoveDirection = direction.normalized;

        player.CurrentSpeed = airSpeed * InputHub.instance.Move.magnitude;

        gravity = player.VerticalSpeed > 0f ? jumpGravity : fallGravity;

        if (!InputHub.instance.Jump || Physics.CheckSphere(player.CeilingCheckPosition, player.Radius, LayerMask.GetMask("Solid")))
        {
            player.VerticalSpeed = Mathf.Min(0f, player.VerticalSpeed);
        }

        if (player.VerticalSpeed > fallSpeed)
        {
            player.VerticalSpeed += gravity * Time.deltaTime;
        }
    }

    public override void ChangeState(PlayerController player)
    {
        if (player.VerticalSpeed < 0f && Physics.CheckSphere(player.GroundCheckPosition, player.Radius, LayerMask.GetMask("Solid")))
        {
            player.SetState(player.GroundState);
        }
    }

    public override void ExitState(PlayerController player)
    {
        
    }
}
