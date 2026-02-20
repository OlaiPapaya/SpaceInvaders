using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] TMP_Text
        _lifesText,
        _pointsText;

    [SerializeField]
    GameObject
        _winPanel,
        _losePanel;

    [SerializeField] RectTransform
        _specialAbilityCounter;

    [SerializeField] int
        _pointsToSpecialAttack;

    [SerializeField] float
        _abilityFillerMinValue;

    private bool _gameOver;
    public bool GameOver { get => _gameOver; }

    [SerializeField] float
        _enemyWinYposition;

    float _abilityFillerMaxValue;
    public float EnemyWinYposition { get { return _enemyWinYposition; } }

    [SerializeField] private ushort _startLifes;
    private ushort _lifes;
    private int _points, _abilityPoints;
    public int Points { get { return _points; } }
    public bool CanUseSpecialAttack => _abilityPoints >= _pointsToSpecialAttack;

    void Awake()
    {
        // Set this script as the global game manager, if there is not one already:
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        // Initializing player's lifes:
        _lifes = _startLifes;
    }

    private void Start()
    {
        if (_specialAbilityCounter != null) _abilityFillerMaxValue = _specialAbilityCounter.offsetMax.y;
        Debug.Log(_abilityFillerMaxValue);
        UpdateAbilityCounter();
    }

    // Basic Game Functions:

    public void LoseLife()
    {
        _lifes--;
        _lifesText.text = _lifes.ToString();
        if (_lifes <= 0) PlayerDied();
    }

    public void AddPoints(int addedPoints, bool pointsAffectAbility)
    {
        _points += addedPoints;
        if (pointsAffectAbility)
        {
            _abilityPoints += addedPoints;
            UpdateAbilityCounter();
        }
        _pointsText.text = _points.ToString();
    }

    public void AbilityUsed()
    {
        _abilityPoints = 0;
        UpdateAbilityCounter();
    }

    void UpdateAbilityCounter()
    {
        if (_specialAbilityCounter == null) return;
        float lerpVal = Mathf.Clamp01(_abilityPoints / _pointsToSpecialAttack);
        float sliderValue = Mathf.Lerp(-_abilityFillerMinValue, _abilityFillerMaxValue, lerpVal);
        Debug.Log(sliderValue);
        _specialAbilityCounter.offsetMax = new Vector2(_specialAbilityCounter.offsetMax.x, sliderValue);
    }

    // Win conditions:
    public void EnemiesDefeated()
    {
        _gameOver = true;
        _winPanel.SetActive(true);
    }

    // Lose conditions:

    public void EnemiesArrived()
    {
        _gameOver = true;
        _losePanel.SetActive(true);
    }

    private void PlayerDied()
    {
        _gameOver = true;
        _losePanel.SetActive(true);
        TMP_Text loseReason = _losePanel.transform.Find("Subtitle").GetComponent<TMP_Text>();
        if (loseReason != null) loseReason.text = "You lost all lifes!";
    }

    // Main Buttons:

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
