using UnityEngine;

public class ShootController : MonoBehaviour{

    #region Variables

    [SerializeField] LayerMask _shootableObjectMask;
    Camera _camera;

    #endregion

    #region Unity Messages

    void Awake(){
        _camera = Camera.main;
    }

    void Update(){
        if (Input.GetMouseButtonDown(0)){
            Shoot();
        }
    }

    #endregion

    #region Private Methods

    void Shoot(){
        RaycastHit hit;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, _shootableObjectMask)) 
            return;
        
        var hitTarget = hit.collider.GetComponent<Ball>();
        if (!hitTarget)
            return;

        hitTarget.ScoreBall();
    }

    #endregion
    
}