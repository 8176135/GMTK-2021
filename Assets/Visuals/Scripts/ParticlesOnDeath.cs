using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesOnDeath : MonoBehaviour
{
    public ParticleSystem deathParticles;
    
    private void OnDestroy()
    {
        var gameObj = Instantiate(deathParticles);
        gameObj.transform.position = transform.position;
    }
}
