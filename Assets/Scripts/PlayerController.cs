using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Entity
{
    private InputSystem _input;

    [SerializeField] GameObject _shotPrefab;

    [SerializeField] float
        _moveSpeed,
        _acceleration;

    Vector3 _startingPos;

    float _currentMovement;

    bool _inLeftCorner, _inRightCorner;

    InputFrame _inputFrame;

    void Awake()
    {
        _input = new();
        _input.Player.Enable();
        _startingPos = transform.position;
    }

    void Update()
    {
        if (GameManager.instance.GameOver) return;
        GetInput();
        ApplyMovement();
        Shoot();
    }

    void GetInput()
    {
        _inputFrame = new();
        _inputFrame.horizontal = _input.Player.Move.ReadValue<Vector2>().x;
        _inputFrame.shootJustPressed = _input.Player.Jump.WasPressedThisFrame();
    }

    void ApplyMovement()
    {
        float xMovement = Mathf.MoveTowards(
            _currentMovement,
            _inputFrame.horizontal * _moveSpeed,
            _acceleration * Time.deltaTime
        );

        if (xMovement < 0 && _inLeftCorner || xMovement > 0 && _inRightCorner) xMovement = 0;
        _currentMovement = xMovement;

        transform.position += new Vector3(_currentMovement * Time.deltaTime, 0, 0);
    }

    void Shoot()
    {
        if (!_inputFrame.shootJustPressed) return;

        Instantiate(_shotPrefab, transform.position + Vector3.forward, Quaternion.identity);
    }

    void Entity.Damage()
    {
        GameManager.instance.LoseLife();
        if (!GameManager.instance.GameOver) transform.position = _startingPos;
        else Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LeftCorner")) _inLeftCorner = true;
        if (other.CompareTag("RightCorner")) _inRightCorner = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LeftCorner")) _inLeftCorner = false;
        if (other.CompareTag("RightCorner")) _inRightCorner = false;
    }

    struct InputFrame
    {
        public float horizontal;
        public bool shootJustPressed;
    }
}
