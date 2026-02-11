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
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        _lifes = _startLifes;
    }

    public void EnemiesDefeated()
    {
        _gameOver = true;
        Debug.Log("You win!");
    }

    public void EnemiesArrived()
    {
        _gameOver = true;
        Debug.Log("You lose! Enemies arrived!");
    }

    public void LoseLife()
    {
        _lifes--;
        _lifesText.text = _lifes.ToString();
        if (_lifes <= 0) PlayerDied();
    }

    private void PlayerDied()
    {
        _gameOver = true;
        Debug.Log("You lose! Player died!");
    }
}
