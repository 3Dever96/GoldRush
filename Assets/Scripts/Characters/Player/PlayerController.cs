using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public CharacterController Controller { get; private set; }
    
    public PlayerState CurrentState { get; private set; }

    public float CurrentSpeed { get; set; }
    public float VerticalSpeed { get; set; }
    public Vector3 MoveDirection { get; set; }
    public Vector3 LookDirection { get; set; }
    public Vector3 Position { get; private set; }
    public Vector3 GroundCheckPosition { get; private set; }
    public Vector3 CeilingCheckPosition { get; private set; }

    [field: SerializeField] public PlayerGroundState GroundState { get; private set; } = new PlayerGroundState();
    [field: SerializeField] public PlayerAirState AirState { get; private set; } = new PlayerAirState();

    [field: SerializeField] public float Radius { get; private set; }
    [SerializeField] private float groundCheckOffset;
    [SerializeField] private float ceilingCheckOffset;

    private void Start()
    {
        Controller = GetComponent<CharacterController>();

        SetState(GroundState);
    }

    private void Update()
    {
        Position = transform.position;
        GroundCheckPosition = Position + Vector3.up * groundCheckOffset;
        CeilingCheckPosition = Position + Vector3.up * ceilingCheckOffset;

        if (CurrentState != null)
        {
            CurrentState.UpdateState(this);
            CurrentState.ChangeState(this);
        }

        ApplyMovement();
    }

    public void SetState(PlayerState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.ExitState(this);
        }

        CurrentState = newState;

        if (CurrentState != null)
        {
            CurrentState.StartState(this);
        }
    }

    private void ApplyMovement()
    {
        Vector3 velocity = MoveDirection * CurrentSpeed;
        velocity.y = VerticalSpeed;

        Controller.Move(velocity * Time.deltaTime);
    }

    public void FaceDirection(float turnSpeed = 500f)
    {
        if (LookDirection == Vector3.zero) return;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(LookDirection), turnSpeed * Time.deltaTime);
    }
}
