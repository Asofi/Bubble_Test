using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour{

    #region Variables

    public static ScreenSpaceBounds ScreenBounds;                                      // Holds struct with current screen bounds
    
    [Header("Spawn Settings"),SerializeField] Transform _ballPrefab;                   // Ball prefab to spawn
    [SerializeField] float _spawnCooldown = 1f;                                        // Time between spawns
    
    [Header("Size Settings"),SerializeField] float _minSize = 0.2f;                    // Max size of ball
    [SerializeField] float _maxSize = 2f;                                              // Min size of ball  
   

    [Header("Speed Settings"),SerializeField] float _baseSpeed = 10;                   // Start base ball speed
    [SerializeField] float _maxBaseSpeed = 20;                                         // Max base ball speed         
    [SerializeField] float _speedMultiplier = 1;                                       // Speed multiplier

    [Header("Color Settings"),SerializeField] Color[] _colorPallete;                   // Array of allowed colors. Made this instead of full random for more pleasant colors.

    float _currentSpeed;                                                               // Base ball speed at this moment
    float _maxTimer;                                                                   // Max timer in this session

    #endregion

    #region Structs
    
    public struct ScreenSpaceBounds{
        public readonly Vector2 HorizontalBounds;
        public readonly Vector2 VerticalBounds;

        public ScreenSpaceBounds(float left, float right, float bottom, float up){
            HorizontalBounds = new Vector2(left, right);
            VerticalBounds = new Vector2(bottom, up);
        }

        public override string ToString(){
            return
                $"Left corner: {HorizontalBounds.x}, Right Corner: {HorizontalBounds.y} \n Bottom Corner: {VerticalBounds.x}, UpperCorner: {VerticalBounds.y}";
        }
    }

    #endregion

    #region Unity Messages

    void Awake(){
        ScreenBounds = GetScreenSpaceBounds();
    }

    void OnEnable(){
        GameController.TimerUpdated += OnTimerUpdated;
        GameController.GameStarted += OnGameStarted;
        GameController.GameEnded += OnGameEnded;
        GameController.TimerStarted += OnTimerStarted;
    }

    void OnDisable(){
        GameController.TimerUpdated -= OnTimerUpdated;
        GameController.GameStarted -= OnGameStarted;
        GameController.GameEnded -= OnGameEnded;
        GameController.TimerStarted -= OnTimerStarted;
    }

    #endregion

    #region Event Handlers
    
    void OnTimerStarted(float obj){
        _maxTimer = obj;
        _currentSpeed = _baseSpeed;
    }

    void OnTimerUpdated(float obj){
        if(obj > 0)
            _currentSpeed = Mathf.Lerp(_baseSpeed, _maxBaseSpeed, 1 - obj / _maxTimer);
    }
    
    void OnGameEnded(){
        StopAllCoroutines();
    }

    void OnGameStarted(){
        StartCoroutine(SpawningCoroutine());
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates random colors to our ball pallete
    /// </summary>
    /// <param name="amount">Amount of desired colors</param>
    public void FillUpColors(int amount){
        _colorPallete = new Color[amount];
        for (var i = 0; i < _colorPallete.Length; i++){
            _colorPallete[i] = Random.ColorHSV(0,1,0.5f, 1, 0.8f, 1);
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Calculates current screen bounds
    /// </summary>
    /// <returns>Current screen bounds</returns>
    ScreenSpaceBounds GetScreenSpaceBounds(){
        var camera = Camera.main;

        var leftCorner = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane)).x;
        var rightCorner = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane)).x;
        var bottomCorner = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane)).y;
        var upperCorner = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane)).y;

        var bounds = new ScreenSpaceBounds(leftCorner, rightCorner, bottomCorner, upperCorner);


        return bounds;
    }

    /// <summary>
    /// Returns random color from ColorPallete
    /// </summary>
    /// <returns>Random color from ColorPallete</returns>
    Color GetColorFromArray(){
        return _colorPallete[Random.Range(0, _colorPallete.Length)];
    }

    /// <summary>
    /// Setups ball
    /// </summary>
    /// <param name="ball">Ball to setup</param>
    /// <param name="size">Ball size</param>
    void InitBall(Ball ball, float size){
        var speed = _currentSpeed * (_maxSize / size) * _speedMultiplier;
        var score = Mathf.RoundToInt(_maxSize / size);
        
        ball.transform.localScale = Vector3.one * size;
        ball.SetColor(GetColorFromArray());
        ball.SetSpeed(speed);
        ball.SetScoreCost(score);
    }
    
    /// <summary>
    /// Ball spawning coroutine
    /// </summary>
    IEnumerator SpawningCoroutine(){
        while (true){
            var size = Random.Range(_minSize, _maxSize);
            var yToSpawn = ScreenBounds.VerticalBounds.x - (size / 2f);
            var xToSpawn = Random.Range(ScreenBounds.HorizontalBounds.x + size / 2f,
                                        ScreenBounds.HorizontalBounds.y - size / 2f);
            var spawnedBall = Instantiate(_ballPrefab, new Vector2(xToSpawn, yToSpawn), Quaternion.identity);
            
            var ballScript = spawnedBall.GetComponent<Ball>();
            InitBall(ballScript, size);
            yield return new WaitForSeconds(_spawnCooldown);
        }
    }

    #endregion
    
}