using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed;
    //public float fireRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (speed >= 0.0f)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ENTER");
        if (other.gameObject.tag == "Enemy")
        {
            //Decrease enemy health
        }

        speed = 0.0f;
        Destroy(gameObject);
    }
}
