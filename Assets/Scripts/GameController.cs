using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour{

    const float TimerStep = 0.1f;
    
    #region Events   
    
    public static event Action<int> ScoreUpdated;
    public static event Action<float> TimerUpdated, TimerStarted;
    public static event Action GameStarted, GameEnded;

    #endregion

    #region Variables

    [SerializeField] float _gameSessionTime;
    float _timeLeft;
    int _score;

    #endregion

    #region Properties

    int Score{
        get{ return _score; }
        set{
            _score = value;
            ScoreUpdated?.Invoke(_score);
        }
    }

    float TimeLeft{
        get{ return _timeLeft; }
        set{
            _timeLeft = value;
            TimerUpdated?.Invoke(_timeLeft);
        }
    }

    #endregion

    #region Unity Messages

    void OnEnable(){
        Ball.BallDestroyed += OnBallDestroyed;
    }

    void OnDisable(){
        Ball.BallDestroyed -= OnBallDestroyed;
    }

    #endregion

    #region Event Handlers

    void OnBallDestroyed(int obj) {
        Score += obj;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Initialise game
    /// </summary>
    void Init(){
        TimeLeft = _gameSessionTime;
        Score = 0;
        StartCoroutine(TimerCoroutine());
    }

    /// <summary>
    /// Rises GameEnded event
    /// </summary>
    void EndGame(){
        GameEnded?.Invoke();
    }

    IEnumerator TimerCoroutine(){
        TimerStarted?.Invoke(TimeLeft);
        while (_timeLeft > 0){
            yield return new WaitForSeconds(TimerStep);
            TimeLeft -= TimerStep;
        }
        EndGame();
    }

    #endregion

    #region Button Methods

    public void GameStart(){
        Init();
        GameStarted?.Invoke();
    }

    #endregion
}