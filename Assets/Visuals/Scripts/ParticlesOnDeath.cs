using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesOnDeath : MonoBehaviour
{
    public ParticleSystem deathParticles;
    
    private void OnDestroy()
    {
        var position = transform.position;
        var gameObj = Instantiate(deathParticles, position, transform.rotation);
        gameObj.transform.position = position;
    }
}
