using System;
using UnityEngine;

public class Ball : MonoBehaviour{
    
    #region Events   
    
    public static event Action<int> BallDestroyed;
    
    #endregion

    #region Variables

    [SerializeField] GameObject EffectPrefab;

    float _speed;                                                // Ball speed
    int _scoreCost;                                              // Ball score cost  

    MeshRenderer _meshRenderer;

    #endregion

    #region Unity Messages

    void Awake (){
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }
    
    void Update () {
        Move();
    }

    void OnEnable(){
        GameController.GameEnded += OnGameEnded;
    }

    void OnDisable(){
        GameController.GameEnded -= OnGameEnded;
    }

    #endregion

    #region Event Handlers

    void OnGameEnded(){
        DestroyBall();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Sets color to ball
    /// </summary>
    /// <param name="color">Color</param>
    public void SetColor(Color color){
        _meshRenderer.material.color = color;
    }

    /// <summary>
    /// Sets speed to ball
    /// </summary>
    /// <param name="speed">Speed</param>
    public void SetSpeed(float speed){
        _speed = speed;
    }

    /// <summary>
    /// Sets score cost to ball
    /// </summary>
    /// <param name="cost">Score cost</param>
    public void SetScoreCost(int cost){
        _scoreCost = cost;
    }
    
    /// <summary>
    /// Destoys ball with score
    /// </summary>
    public void ScoreBall(){
        BallDestroyed?.Invoke(_scoreCost);
        Instantiate(EffectPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Destoys ball with score
    /// </summary>
    public void DestroyBall(){
        Destroy(gameObject);
    }

    #endregion

    #region Private Methods

    void Move(){
        transform.Translate(transform.up * _speed * Time.deltaTime);
        if (transform.position.y > SpawnController.ScreenBounds.VerticalBounds.y)
            DestroyBall();
    }

    #endregion

}
