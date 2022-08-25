using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _timeToRestartText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Image _buildIcon;

    public static InterfaceManager SharedInstance;

    private float _remainingRestartTime;

    void Awake()
    {
        SharedInstance = this;
    }

    void OnEnable()
    {
        GameManager.OnGameStopped += RestartGame;
        GameManager.OnScoreChanged += OnScoreChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameStopped -= RestartGame;
        GameManager.OnScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged()
    {
        _scoreText.text = $"SCORE: {GameManager.SharedInstance.ScorePoints}";
    }

    public void OnStartButtonPress()
    {
        GameManager.SharedInstance.ContinueGame();
        _startButton.gameObject.SetActive(false);
    }

    private void RestartGame()
    {
        _timeToRestartText.gameObject.SetActive(true);
        _remainingRestartTime = GameManager.SharedInstance.RestartGameTime;
        StartCoroutine(ShowTimeToRestart());
    }

    public void ShowOrHideHelpImage(bool show)
    {
        _buildIcon.gameObject.SetActive(show);
    }

    private IEnumerator ShowTimeToRestart()
    {
        while (_remainingRestartTime > 0)
        {
            _timeToRestartText.text = $"GameOver!\n Restarted after {_remainingRestartTime}!";
            yield return new WaitForSeconds(1);
            _remainingRestartTime--;
        }
    }
}
