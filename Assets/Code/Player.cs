using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float rotationOffset = 270f;

    public Image healthBar;
    public float healthAmount = 100f;

    private Rigidbody2D _rb;
    private InputAction _moveAction;
    private Camera _cam;
    private float _aimAngle;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
        _cam = Camera.main;
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

        _rb.MoveRotation(_aimAngle);
    }

    void Update()
    {
        // Re-acquire camera if needed (e.g., scene reload)
        if (_cam == null) _cam = Camera.main;

        // Mouse aim -> compute angle in degrees (Z axis)
        var mouse = Mouse.current;
        if (_cam != null && mouse != null)
        {
            Vector2 mouseScreen = mouse.position.ReadValue();
            Vector2 mouseWorld = _cam.ScreenToWorldPoint(mouseScreen);
            Vector2 dir = mouseWorld - _rb.position;

            if (dir.sqrMagnitude > 0.0001f)
            {
                _aimAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rotationOffset;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            TakeDamage(20);
        }
    }

    void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;

        if(healthAmount <= 0f)
        {
            SceneManager.LoadScene("Menu");
        }
    }

    //For usage later (to pick up health)
    void Heal(float healAmount)
    {
        healthAmount += healAmount;
        healthBar.fillAmount = healthAmount / 100f;
    }

}