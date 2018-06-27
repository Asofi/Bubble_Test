using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    float _speed = 0;

    MeshRenderer _mesh;

    public float Speed {
        get {
            return _speed;
        }

        set {
            _speed = value;
        }
    }

    // Use this for initialization
    void Awake () {
        SetRandomColor();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetRandomColor() {
        Mesh mesh = GetComponentInChildren<MeshFilter>().mesh;
        var material = GetComponentInChildren<MeshRenderer>().material;
        material.color = Random.ColorHSV();
    }

}
