using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float _speed = 30.0f;
    public GameObject _owner; //Who fired the projectile
    //public float fireRate;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (_speed >= 0.0f)
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Wall")
        {
            DestroyProjectile();
        }

        if (other.gameObject.tag == "Enemy" && _owner.tag == "Player")
        {
            //Decrease enemy health
            DestroyProjectile();
        }

        if (other.gameObject.tag == "Player" && _owner.tag == "Enemy")
        {
            //Decrease player health
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        _speed = 0.0f;
        Destroy(gameObject);
    }
}
