using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterPlay : MonoBehaviour{

	ParticleSystem _particleSystem;
	
	void Awake (){
		_particleSystem = GetComponent<ParticleSystem>();
		Destroy(gameObject, _particleSystem.duration);
	}
}
