using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour {
	
	#region UI Elements

	[SerializeField] TMP_Text _currentScore, _timer;
	[SerializeField] GameObject StartPanel, RestartPanel;

	#endregion

	#region Unity Messages

	void OnEnable(){
		GameController.ScoreUpdated += OnScoreUpdated;
		GameController.TimerUpdated += OnTimerUpdated;
		GameController.GameStarted += OnGameStarted;
		GameController.GameEnded += OnGameEnded;
	}

	void OnDisable(){
		GameController.ScoreUpdated -= OnScoreUpdated;
		GameController.TimerUpdated -= OnTimerUpdated;
		GameController.GameStarted -= OnGameStarted;
		GameController.GameEnded -= OnGameEnded;
	}

	#endregion

	#region Event Handlers

	void OnGameEnded(){
		RestartPanel.SetActive(true);
	}
	
	void OnGameStarted(){
		StartPanel.SetActive(false);
		RestartPanel.SetActive(false);
	}

	void OnTimerUpdated(float obj){
		_timer.text = $"Time remains: {obj:0.#}";
	}

	void OnScoreUpdated(int obj){
		_currentScore.text = $"SCORE: {obj}";
	}

	#endregion

}
