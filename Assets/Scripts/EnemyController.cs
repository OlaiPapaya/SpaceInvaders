using UnityEngine;

public class EnemyController : MonoBehaviour, Entity
{
    [SerializeField] GameObject _explosionPrefab;

    [SerializeField]
    float
        _waveMovementAmplitude,
        _waveMovementSpeed,
        _waveColumnOffset,
        _explosionDuration;

    [SerializeField] int _pointsWhenKilled;
    int _columnPosition;
    public int ColumnPosition { set { _columnPosition = value; } }

    EnemyManager _enemyManagerScript;
    public EnemyManager ManagerScript { set { _enemyManagerScript = value; } }

    private Transform mySprite;
    private Color myColor = Color.white;

    private void Start()
    {
        // Assigning basic variables
        mySprite = transform.GetChild(0).transform;
        SpriteRenderer mySpriteRenderer = mySprite.GetComponent<SpriteRenderer>();
        if (mySpriteRenderer != null ) myColor = mySpriteRenderer.color;
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
        if (transform.position.y <= GameManager.instance._enemyWinYposition &&
            !GameManager.instance.GameOver) GameManager.instance.EnemiesArrived();

        // Just for a cool effect:
        VerticalWaveMovement();
    }

    void VerticalWaveMovement()
    {
        if (mySprite == null) return;
        // Calculating an y position that moves up and down in time and has an offset based on the column:
        float yPos = Mathf.Sin(_waveMovementSpeed * (Time.time + _waveColumnOffset * _columnPosition)) * _waveMovementAmplitude;
        mySprite.localPosition = new Vector3(0, yPos, 0);
    }

    void Entity.Damage()
    {
        _enemyManagerScript.EnemyDied(); // Increase the speed of the group of enemies
        GameManager.instance.AddPoints(_pointsWhenKilled);
        // Creating the explosion object and saving it:
        Transform explosion =
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().transform;
        SpriteRenderer explosionSprite = explosion.GetComponent<SpriteRenderer>();
        if (explosionSprite != null) explosionSprite.color = myColor; // Changing its color to the enemy color
        Destroy(explosionSprite, _explosionDuration); // And destroying it after some time
        Destroy(gameObject);
    }
}
