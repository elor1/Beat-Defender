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
            EnemyMovement enemyMovement = other.gameObject.GetComponent<EnemyMovement>();
            if (enemyMovement)
            {
                ProjectileDamage.DecreaseHealth(ref enemyMovement._health, GameManager._playerDamage);
            }
                
            DestroyProjectile();
        }

        if (other != null && _owner != null)
        {
            if (other.gameObject.tag == "Player" && _owner.tag == "Enemy")
            {
                //Decrease player health
                EnemyMovement enemyMovement = other.gameObject.GetComponent<EnemyMovement>();
                if (enemyMovement)
                {
                    ProjectileDamage.DecreaseHealth(ref GameManager._playerHealth, enemyMovement._enemyData._damage);
                }

                DestroyProjectile();
            }
        }
        
    }

    private void DestroyProjectile()
    {
        _speed = 0.0f;
        Destroy(gameObject);
    }
}
