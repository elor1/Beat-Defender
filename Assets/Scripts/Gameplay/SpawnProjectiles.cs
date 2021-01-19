using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    [SerializeField] private GameObject _firePoint; //Place to spawn projectile
    [SerializeField] private GameObject _particleToSpawn; //Choose particle to spawn in editor
    
    /// <summary>
    /// Instantiates a particle at a give position
    /// </summary>
    /// <param name="colour">Colour of particle</param>
    public void SpawnParticle(Color colour)
    {
        GameObject particle;
        particle = Instantiate(_particleToSpawn, _firePoint.transform.position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponentInChildren<ParticleSystem>();
        TrailRenderer trail = particle.GetComponentInChildren<TrailRenderer>();

        //Set particle colour
        if (particleSystem)
        {
            var main = particleSystem.main;
            main.startColor = colour;
        }

        //Set trail colour
        if (trail)
        {
            trail.startColor = colour;
            trail.endColor = new Color(colour.r, colour.g, colour.b, 0.0f);
        }

        //Set owner of particle to object that fired it
        particle.GetComponent<ProjectileMovement>().Owner = gameObject;
        
        particle.transform.forward = gameObject.transform.forward;
    }
}
