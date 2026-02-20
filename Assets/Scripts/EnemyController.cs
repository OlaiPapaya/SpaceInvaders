using UnityEngine;

public class EnemyController : MonoBehaviour, Entity
{
    [SerializeField] GameObject _explosionPrefab;

    [SerializeField]
    float
        _waveMovementAmplitude,
        _waveMovementSpeed,
        _waveColumnOffset,
        _explosionDuration,
        _rotationAmount;

    [SerializeField] int _pointsWhenKilled;
    int _columnPosition;
    public int ColumnPosition { set { _columnPosition = value; } }

    EnemyManager _enemyManagerScript;
    public EnemyManager ManagerScript { set { _enemyManagerScript = value; } }

    private Transform _mySprite;
    private Color _myColor = Color.white;

    [SerializeField] Color _altColor;
    SpriteRenderer _mySpriteRenderer;

    bool dead = false;

    private void Start()
    {
        // Assigning basic variables
        _mySprite = transform.GetChild(0).transform;
        _mySpriteRenderer = _mySprite.GetComponent<SpriteRenderer>();
        if (_mySpriteRenderer != null ) _myColor = _mySpriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If it's not a corner, we don't care:
        if (!other.CompareTag("LeftCorner") && !other.CompareTag("RightCorner")) return;

        // Tell the group of enemies to change the direction:
        _enemyManagerScript.DirChange(other.CompareTag("RightCorner"));
    }

    private void Update()
    {
        // If the enemies arrive to the end and the game isn't over yet, the player loses:
        if (transform.position.y <= GameManager.instance.EnemyWinYposition &&
            !GameManager.instance.GameOver) GameManager.instance.EnemiesArrived();

        // Just for a cool effect:
        VerticalWaveMovement();
    }

    void VerticalWaveMovement()
    {
        if (_mySprite == null) return;
        // Calculating an y position that moves up and down in time and has an offset based on the column.
        float wavePos = Mathf.Cos(_waveMovementSpeed * (Time.time + _waveColumnOffset * _columnPosition));
        // This gives me a value from 0 to 1 that moves up and down:
        float wavePos01 = Mathf.Clamp01(wavePos / 2 + 0.5f);

        // Up and down Movement:
        float yPos = wavePos01 * _waveMovementAmplitude;
        _mySprite.localPosition = new Vector3(0, yPos, 0);
        // Cool color gradient:
        if (_mySpriteRenderer == null) return;
        _mySpriteRenderer.color = Color.Lerp(_myColor, _altColor, wavePos01);
        // Rotation --- I KNOW pixel art shouldn't be rotated, but I wanted to see if I could do this:
        float sinWavePos = -Mathf.Sin(_waveMovementSpeed * (Time.time + _waveColumnOffset * _columnPosition));
        _mySprite.eulerAngles = new Vector3(0, 0, sinWavePos * _rotationAmount);
    }

    void Entity.Damage(bool addPoints)
    {
        if (dead) return;
        dead = true;
        _enemyManagerScript.EnemyDied(); // Increase the speed of the group of enemies
        GameManager.instance.AddPoints(_pointsWhenKilled, addPoints);
        // Creating the explosion object and saving it:
        Transform explosion =
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().transform;
        SpriteRenderer explosionSprite = explosion.GetComponent<SpriteRenderer>();
        if (explosionSprite != null) explosionSprite.color = _myColor; // Changing its color to the enemy color
        Destroy(explosionSprite, _explosionDuration); // And destroying it after some time
        Destroy(gameObject);
    }
}
