using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;

    private Rigidbody2D _rb;
    private InputAction _moveAction;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
    }
    void Start()
    {
        _moveAction = new InputAction("Move", InputActionType.Value);
        var wasd = _moveAction.AddCompositeBinding("2DVector");
        wasd.With("Up", "<Keyboard>/w");
        wasd.With("Down", "<Keyboard>/s");
        wasd.With("Left", "<Keyboard>/a");
        wasd.With("Right", "<Keyboard>/d");

        _moveAction.Enable();
    }
    void FixedUpdate()
    {
        Vector2 input = _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
        Vector2 move = input.sqrMagnitude > 1f ? input.normalized : input;
        _rb.MovePosition(_rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

}
