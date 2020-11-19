using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{

    public GameObject firePoint;

    public float fireRate = 0.002f;
    private float timePassed = 0.0f;

    [SerializeField] private GameObject particleToSpawn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        timePassed += Time.deltaTime;
        //Debug.Log(timePassed);
        if (Input.GetMouseButton(0))
        {
            SpawnParticle();
        }
    }

    private void SpawnParticle()
    {
        GameObject particle;
        if (firePoint != null && timePassed >= fireRate)
        {
            particle = Instantiate(particleToSpawn, firePoint.transform.position, Quaternion.identity);
            RotateToMouse.GetMousePosition(particle);

            timePassed = 0.0f;
        }
        timePassed += Time.deltaTime;
    }
}
