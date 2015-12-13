using UnityEngine;
using System.Collections;

public class SkatingParticle : MonoBehaviour {
    Rigidbody playerBody;
    ParticleSystem particle;
    ParticleSystem.EmissionModule emission;
    ParticleSystem.MinMaxCurve curve;

    float startRate;

	void Start () {
        particle = GetComponent<ParticleSystem>();
        emission = particle.emission;
        playerBody = FindObjectOfType<Player>().GetComponent<Rigidbody>();
        curve = particle.emission.rate;
        startRate = curve.constantMax;
	}
	
	void Update () {
        float rate = startRate * playerBody.velocity.magnitude;
        Debug.Log(rate);
        curve.constantMin = rate;
        curve.constantMax = rate;
        emission.rate = curve;
	}
}
