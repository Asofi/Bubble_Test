using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour{
    
    #region Events   
    
    public static event Action<int> ScoreUpdated, TimerStarted, TimerUpdated;
    public static event Action GameStarted, GameEnded;
    
    #endregion

    #region Variables

    const int TimerStep = 1;

    [SerializeField] int _gameSessionTime;
    int _timeLeft;
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

    int TimeLeft{
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

    #region Private Methods

    void OnBallDestroyed(int obj){
        Score += obj;
    }

    void Init(){
        TimeLeft = _gameSessionTime;
        Score = 0;
        StartCoroutine(TimerCoroutine());
    }

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