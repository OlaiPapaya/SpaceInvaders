using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField] TMP_Text
        _lifesText,
        _pointsText,
        _losePanelSubtitle;

    [SerializeField]
    GameObject
        _winPanel,
        _losePanel,
        _specialAbilityMarker;

    [SerializeField] RectTransform
        _specialAbilityCounter;

    [SerializeField] int
        _pointsToSpecialAttack;

    [SerializeField] float
        _abilityFillerMinValue;

    [SerializeField] AudioSource
        _loseAudioSource,
        _winAudioSource;

    private bool _gameOver;
    public bool GameOver => _gameOver;

    float _abilityFillerMaxValue;

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
        if (_lifesText != null) _lifesText.text = _lifes.ToString();
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
        if (_pointsText != null) _pointsText.text = _points.ToString();
    }

    public void AbilityUsed()
    {
        _abilityPoints = 0;
        UpdateAbilityCounter();
    }

    void UpdateAbilityCounter()
    {
        if (_specialAbilityCounter == null) return;
        float lerpVal = Mathf.Clamp01((float)_abilityPoints / _pointsToSpecialAttack);
        float sliderValue = Mathf.Lerp(-_abilityFillerMinValue, _abilityFillerMaxValue, lerpVal);
        Debug.Log(sliderValue);
        _specialAbilityCounter.offsetMax = new Vector2(_specialAbilityCounter.offsetMax.x, sliderValue);
        if (_specialAbilityMarker == null) return;
        _specialAbilityMarker.SetActive(lerpVal >= 1);
    }

    // Win conditions:
    public void EnemiesDefeated()
    {
        if (_gameOver) return;
        _gameOver = true;
        if (_winPanel != null) _winPanel.SetActive(true);
        if (_winAudioSource != null) _winAudioSource.Play();
    }

    // Lose conditions:

    public void EnemiesArrived()
    {
        if (_gameOver) return;
        _gameOver = true;
        if (_losePanel != null) _losePanel.SetActive(true);
        if (_loseAudioSource != null) _loseAudioSource.Play();
    }

    private void PlayerDied()
    {
        if (_gameOver) return;
        _gameOver = true;
        if (_losePanel != null) _losePanel.SetActive(true);
        if (_losePanelSubtitle != null) _losePanelSubtitle.text = "You lost all lifes!";
        if (_loseAudioSource != null) _loseAudioSource.Play();
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
