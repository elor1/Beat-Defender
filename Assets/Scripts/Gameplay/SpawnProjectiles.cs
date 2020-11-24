using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{

    public GameObject _firePoint;

    public float _fireRate = 0.002f;
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

    public void SpawnParticle()
    {
        GameObject particle;
        if (_firePoint != null && _timePassed >= _fireRate)
        {
            particle = Instantiate(particleToSpawn, _firePoint.transform.position, Quaternion.identity);
            particle.GetComponent<ProjectileMovement>()._owner = gameObject;
            RotateToMouse.GetMousePosition(particle);

            _timePassed = 0.0f;
        }
        _timePassed += Time.deltaTime;
    }
}
