using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{

    public GameObject _firePoint;
    private float _timePassed = 0.0f;

    [SerializeField] private GameObject particleToSpawn;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        _timePassed += Time.deltaTime;
        //Debug.Log(timePassed);
        //if (Input.GetMouseButton(0))
        //{
        //    SpawnParticle();
        //}
      
    }

    public void SpawnParticle(Color colour)
    {
        GameObject particle;
        particle = Instantiate(particleToSpawn, _firePoint.transform.position, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponentInChildren<ParticleSystem>();
        TrailRenderer trail = particle.GetComponentInChildren<TrailRenderer>();
        if (particleSystem)
        {
            //Debug.Log("COLOUR CHANGED");
            var main = particleSystem.main;
            main.startColor = colour;
        }
        if (trail)
        {
            trail.startColor = colour;
            trail.endColor = new Color(colour.r, colour.g, colour.b, 0.0f);
        }
        particle.GetComponent<ProjectileMovement>()._owner = gameObject;
        //RotateToMouse.GetMousePosition(particle);
        particle.transform.forward = gameObject.transform.forward;

        _timePassed = 0.0f;
        //if (_firePoint != null && _timePassed >= fireRate)
        //{

        //}
        //_timePassed += Time.deltaTime;
    }
}
