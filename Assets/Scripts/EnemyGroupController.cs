using UnityEngine;

public class EnemyGroupController : MonoBehaviour
{
    [SerializeField] float
        _speed,
        _leapDown,
        _cooldownTime,
        _speedIncreasPerDeath,
        _enemyShootCooldown;

    [SerializeField] GameObject
        _enemyPrefab,
        _shotPrefab;

    [SerializeField] int
        _rows,
        _columns;

    [SerializeField] float
        _horizontalSpacing,
        _verticalSpacing;

    float _lastTimeDirChanged,
        _lastTimeEnemyShot;
    int _enemyNum;

    bool _goingRight = true;

    private void Awake()
    {
        FillEnemies();
        _enemyNum = _rows * _columns;
    }

    void FillEnemies()
    {
        // Fill the enemies based on the number of rows and columns, and the spacing between them:
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                Vector3 pos = transform.position + new Vector3(j * _horizontalSpacing, -i * _verticalSpacing, 0);
                pos.x -= (_columns - 1) * _horizontalSpacing / 2; // Centering the group of enemies in the x axis
                pos.y += (_rows - 1) * _verticalSpacing / 2; // Centering the group of enemies in the y axis
                Instantiate(_enemyPrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    void Update()
    {
        // Moving the group of enemies in the current vertical direction:
        transform.position += (_goingRight ? Vector3.right : Vector3.left) * _speed * Time.deltaTime;

        // Making an enemy shoot when the timer has passed and the game isn't over:
        if (Time.time > _lastTimeEnemyShot + _enemyShootCooldown && !GameManager.instance.GameOver) EnemyShoot();
    }

    public void EnemyShoot()
    {
        // Choose a random enemy to shoot and getting its position, in front of the enemies:
        Vector3 randomEnemyPos = transform.GetChild(Random.Range(0, _enemyNum)).position + Vector3.back;
        Instantiate(_shotPrefab, randomEnemyPos, Quaternion.identity); // Shoot from the position of the chosen enemy

        _lastTimeEnemyShot = Time.time; // Restart cooldown
    }

    public void DirChange()
    {
        // If the cooldown hasn't finished, don't change:
        if (_lastTimeDirChanged + _cooldownTime > Time.time) return;
        _goingRight = !_goingRight; // Change direction
        transform.position += Vector3.down * _leapDown; // Move enemies down
        _lastTimeDirChanged = Time.time; // Restart cooldown
    }

    public void EnemyDied()
    {
        _speed += _speedIncreasPerDeath;
        _enemyNum--;
        if (_enemyNum <= 0) EnemiesDefeated();
    }

    void EnemiesDefeated()
    {
        GameManager.instance.EnemiesDefeated();
        Debug.Log("You Win!");
    }
}
