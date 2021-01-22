using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private float _speed = GameManager.ProjectileSpeed; //Speed projectile should travel

    public GameObject Owner; //Who fired the projectile
    
    // Update is called once per frame
    private void Update()
    {
        //Move projectile forwards
        if (_speed >= 0.0f)
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //If projectile hits a wall, destroy it
        if (other.gameObject.tag == "Wall")
        {
            DestroyProjectile();
        }

        if (other != null && Owner != null)
        {
            if (other.gameObject.tag == "Enemy" && Owner.tag == "Player")
            {
                //If player hits an enemy, decrease enemy health
                EnemyMovement enemyMovement = other.gameObject.GetComponent<EnemyMovement>();
                if (enemyMovement)
                {
                    ProjectileDamage.DecreaseHealth(ref enemyMovement.Health, GameManager.PlayerDamage);
                }
                
             DestroyProjectile();
            }
            else if (other.gameObject.tag == "Player" && Owner.tag == "Enemy")
            {
                //If enemy hits player, decrease player health
                EnemyMovement enemyMovement = Owner.gameObject.GetComponent<EnemyMovement>();
                if (enemyMovement)
                {
                    ProjectileDamage.DecreaseHealth(ref GameManager.PlayerHealth, enemyMovement.Damage);
                }

                DestroyProjectile();
            }
        }
        
    }

    /// <summary>
    /// Destroys the projectile game object
    /// </summary>
    private void DestroyProjectile()
    {
        _speed = 0.0f;
        Destroy(gameObject);
    }
}
