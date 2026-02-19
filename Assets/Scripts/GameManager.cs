using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] TMP_Text
        _lifesText,
        _pointsText;

    private bool _gameOver;
    public bool GameOver { get => _gameOver; }

    public float _enemyWinYposition;

    [SerializeField] private ushort _startLifes;
    private ushort _lifes;
    private int _points;

    void Awake()
    {
        // Set this script as the global game manager, if there is not one already:
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        // Initializing player's lifes:
        _lifes = _startLifes;
    }

    // Basic Game Functions:

    public void LoseLife()
    {
        _lifes--;
        _lifesText.text = _lifes.ToString();
        if (_lifes <= 0) PlayerDied();
    }

    public void AddPoints(int addedPoints)
    {
        _points += addedPoints;
        _pointsText.text = _points.ToString();
    }

    // Win conditions:
    public void EnemiesDefeated()
    {
        _gameOver = true;
        Debug.Log("You win!");
    }

    // Lose conditions:

    public void EnemiesArrived()
    {
        _gameOver = true;
        Debug.Log("You lose! Enemies arrived!");
    }

    private void PlayerDied()
    {
        _gameOver = true;
        Debug.Log("You lose! Player died!");
    }
}
