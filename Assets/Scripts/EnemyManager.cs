using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    float
        _speed,
        _leapDown,
        _cooldownTime,
        _speedIncreasPerDeath,
        _enemyShootCooldown,
        _enemyRowDelayTime;

    [SerializeField] GameObject
        _shotPrefab,
        _enemyRow;

    [SerializeField] List<GameObject> _enemiesPerRow = new();

    [SerializeField] int _columns;

    [SerializeField] float
        _horizontalSpacing,
        _verticalSpacing;

    float _lastTimeEnemyShot;
    int _enemyNum,
        _rows;

    bool _goingRight = true;

    private void Awake()
    {
        // Assigning basic variables
        _rows = _enemiesPerRow.Count;
        _enemyNum = _rows * _columns;
        FillEnemies(); // Creating all the enemies
    }

    void FillEnemies()
    {
        // Fill the enemies based on the number of rows and columns, and the spacing between them:
        for (int rowIdx = 0; rowIdx < _rows; rowIdx++)
        {
            Transform newGroup = Instantiate(_enemyRow, transform).transform;
            {
                EnemyGroupController newGroupScript = newGroup.GetComponent<EnemyGroupController>();
                if (newGroupScript != null)
                    newGroup.GetComponent<EnemyGroupController>().Speed = _speed;
            }

            for (int colIdx = 0; colIdx < _columns; colIdx++)
            {
                Vector3 pos = transform.position + new Vector3(colIdx * _horizontalSpacing, -rowIdx * _verticalSpacing, 0);
                pos.x -= (_columns - 1) * _horizontalSpacing / 2; // Centering the group of enemies in the x axis
                // Adding a delay to each row so that the offset will be exactly the same in both directions:
                pos.x += _enemyRowDelayTime / _speed / 2 * rowIdx;
                pos.y += (_rows - 1) * _verticalSpacing / 2; // Centering the group of enemies in the y axis
                // Creating the enemy itself and saving its script in a variable:
                EnemyController newEnemyScript =
                    Instantiate(_enemiesPerRow[rowIdx], pos, Quaternion.identity, newGroup).GetComponent<EnemyController>();
                // Assigning this script to its reference so that the enemy can communicate:
                if (newEnemyScript != null)
                {
                    newEnemyScript.ManagerScript = this;
                    newEnemyScript.ColumnPosition = colIdx;
                }
            }
        }
    }

    private void Update()
    {
        // Making an enemy shoot when the timer has passed and the game isn't over:
        if (Time.time > _lastTimeEnemyShot + _enemyShootCooldown && !GameManager.instance.GameOver) EnemyShoot();
    }

    public void EnemyShoot()
    {
        // Choose a random enemy to shoot and getting its position, in front of the enemies:
        Transform randomRow;
        do
        {
            randomRow = transform.GetChild(Random.Range(0, transform.childCount));
        }
        while (randomRow.childCount <= 0); // Choosing a row until one with an enemy inside is found.
        // Choosing a random enemy from the chosen row:
        Vector3 randomEnemyPos = randomRow.GetChild(Random.Range(0, randomRow.childCount)).position + Vector3.back;

        Instantiate(_shotPrefab, randomEnemyPos, Quaternion.identity); // Shoot from the position of the chosen enemy
        _lastTimeEnemyShot = Time.time; // Restart cooldown
    }

    public void DirChange(bool isRightCorner)
    {
        // If the corner touched isn't the one we're heading, don't change:
        if (_goingRight != isRightCorner) return;
        _goingRight = !_goingRight;
        StartCoroutine(ChangeSlowly());
    }

    IEnumerator ChangeSlowly()
    {
        // One for one, the rows will move down:
        for (int childIdx = transform.childCount - 1; childIdx >= 0; childIdx--)
        {
            Transform row = transform.GetChild(childIdx);
            // If this row doesn't contain any enemies, we don't want the delayed effect on it:
            if (row.childCount == 0) continue;
            row.position += Vector3.down * _leapDown; // Move enemies down

            // Thelling the enemy group to change direction:
            EnemyGroupController rowScript = transform.GetChild(childIdx).GetComponent<EnemyGroupController>();
            if (rowScript != null) rowScript.GoingRight = _goingRight;

            yield return new WaitForSeconds(_enemyRowDelayTime / _speed); // Wait the time
        }
    }

    public void EnemyDied()
    {
        _speed += _speedIncreasPerDeath; // Increase all the enemies' speed.
        // Updating the number of enemies and checking if they all died:
        _enemyNum--;
        if (_enemyNum <= 0)
        {
            EnemiesDefeated();
            return;
        }
        // Telling each group to increase their speed:
        for (int childIdx = transform.childCount - 1; childIdx >= 0; childIdx--)
        {
            EnemyGroupController rowScript = transform.GetChild(childIdx).GetComponent<EnemyGroupController>();
            if (rowScript != null) rowScript.Speed = _speed;
        }
    }

    void EnemiesDefeated()
    {
        // Make the player win:
        GameManager.instance.EnemiesDefeated();
    }
}
