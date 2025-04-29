using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainController : MonoBehaviour
{

    public ParticleSystem rainParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRain()
    {
        if (!rainParticle.isPlaying)
            rainParticle.Play();
    }

    public void StopRain()
    {
        if (rainParticle.isPlaying)
            rainParticle.Stop();
    }

}
