using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _bridgeMaxLength = 5;
    [SerializeField] private float _minDistanceBehindPlatforms = 5;
    [SerializeField] private float _buildSpeed = 1;
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private float _bridgeRotationTime = 2;
    [SerializeField] private float _restartGameTime;
    [SerializeField] private float _leftBoundary;

    public static event Action OnGameStopped;
    public static event Action OnScoreChanged;
    public float BridgeMaxLength => _bridgeMaxLength;
    public float MinDistanceBehindPlatforms => _minDistanceBehindPlatforms;
    public float BuildSpeed => _buildSpeed;
    public float MoveSpeed => _moveSpeed;
    public float BridgeRotationTime => _bridgeRotationTime;
    public bool IsGameStarted => _isGameStarted;
    public float RestartGameTime => _restartGameTime;
    public int ScorePoints => _scorePoints;
    public float LeftBoundary => _leftBoundary;

    public static GameManager SharedInstance;

    private float _timeScale;
    private int _scorePoints = -1;
    private bool _isGameStarted;

    void Awake()
    {
        SharedInstance = this;
        _timeScale = Time.timeScale;
        //Time.timeScale = 0;
    }

    public void AddScorePoints(int pointsToAdd)
    {
        _scorePoints += pointsToAdd;
        OnScoreChanged?.Invoke();
    }

    public void StopGame()
    {
        //Time.timeScale = 0;
        _isGameStarted = false;
        OnGameStopped?.Invoke();
        StartCoroutine(StartRestartingTimer());
    }

    public void ContinueGame()
    {
        Time.timeScale = _timeScale;
        _isGameStarted = true;
        PlayerController.SharedInstance.PlayMoveAnimation(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator StartRestartingTimer()
    {
        yield return new WaitForSeconds(RestartGameTime);
        RestartGame();
    }
}
