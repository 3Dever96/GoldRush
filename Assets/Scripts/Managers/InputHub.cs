using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHub : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public bool Jump {  get; private set; }

    public static InputHub instance;
    private PlayerInput input;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        input = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        input.onActionTriggered += OnAction;
    }

    private void OnDisable()
    {
        input.onActionTriggered -= OnAction;
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case "Move": Move = context.ReadValue<Vector2>(); break;
            case "Jump": Jump = context.ReadValue<float>() > 0.5f; break;
        }
    }
}
