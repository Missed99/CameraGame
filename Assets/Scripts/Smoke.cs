using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public ParticleSystem smokeParticleSystem;

    private void Start()
    {
        smokeParticleSystem.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (smokeParticleSystem != null && other.tag == "Smoke")
        {
            smokeParticleSystem.Play();
        }
    }
}
