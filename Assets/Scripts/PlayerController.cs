using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, Entity
{
    private InputSystem _input;

    [SerializeField] GameObject
        _shotPrefab,
        _explosionPrefab;

    [SerializeField] float
        _moveSpeed,
        _acceleration,
        _explosionDuration;

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
        // Si se ha acabado la partida, el jugador ya no se controla:
        if (GameManager.instance.GameOver) return;
        GetInput();
        ApplyMovement();
        Shoot();
    }

    void GetInput()
    {
        // We save all the input of this frame:
        _inputFrame = new();
        _inputFrame.horizontal = _input.Player.Move.ReadValue<Vector2>().x;
        _inputFrame.shootJustPressed = _input.Player.Jump.WasPressedThisFrame();
    }

    void ApplyMovement()
    {
        // MoveTowards to have a smooth and controlled movement:
        float xMovement = Mathf.MoveTowards(
            _currentMovement,
            _inputFrame.horizontal * _moveSpeed,
            _acceleration * Time.deltaTime
        );

        // If he's gotten to the limit and the player's still moving in that direction, we cancel its movement:
        if (xMovement < 0 && _inLeftCorner || xMovement > 0 && _inRightCorner) xMovement = 0;
        // We save the movement so that in the next frame the interpolation can happen from this point:
        _currentMovement = xMovement;

        // We apply the calculated movement:
        transform.position += new Vector3(_currentMovement * Time.deltaTime, 0, 0);
    }

    void Shoot()
    {
        // If player didn't press shoot, then we don't shoot:
        if (!_inputFrame.shootJustPressed) return;

        // We instantiate the shoot prefab in our position:
        Instantiate(_shotPrefab, transform.position + Vector3.forward, Quaternion.identity);
    }

    void Entity.Damage()
    {
        // If the player recieved damage, we create the death particles:
        Destroy(Instantiate(_explosionPrefab, transform.position, Quaternion.identity), _explosionDuration);
        // We tell the game manager we lost a life:
        GameManager.instance.LoseLife();
        // And if we're still not dead, we restart the position as if the player "reappeared":
        if (!GameManager.instance.GameOver) transform.position = _startingPos;
        // If we lost completely, there's no more player for today:
        else Destroy(gameObject);
    }

    // Just checking corners
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

    // The structure where our input will be saved
    struct InputFrame
    {
        public float horizontal;
        public bool shootJustPressed;
    }
}
