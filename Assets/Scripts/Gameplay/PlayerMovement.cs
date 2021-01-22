using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rigidbody; //Player's rigidbody component
    private SpawnProjectiles _projectileSpawner; //Player's projectile spawner component
    private float _timePassed; //Time since last projectile was fired
    
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); //Get rigidbody component
        _projectileSpawner = GetComponent<SpawnProjectiles>();
        _timePassed = 0.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.CurrentGameState == GameManager.State.Playing)
        {
            if (_rigidbody != null)
            {
                MovePlayer();
                RotateToMouse.GetMousePosition(gameObject);
            }
        }

        _timePassed += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (GameManager.CurrentGameState == GameManager.State.Playing)
        {
            //When left click is pressed, fire projectile
            if (Input.GetMouseButton(0))
            {
                Color particleColour = new Color(Random.Range(0.149f, 0.404f), Random.Range(0.906f, 0.945f), Random.Range(0.267f, 0.694f), 1.0f);
                if (_timePassed >= GameManager.PlayerFireRate)
                {
                    _projectileSpawner.SpawnParticle(particleColour);
                    _timePassed = 0.0f;
                }
                
            }
        }
            
    }

    /// <summary>
    /// Moves the player's position based on user input
    /// </summary>
    private void MovePlayer()
    {
        Vector3 movementDirection = new Vector3(0.0f, 0.0f, 0.0f); //Vector calculating overall direction for player this frame

        if (Input.GetKey(KeyCode.W))
        {
            movementDirection += new Vector3(0.0f, 0.0f, 1.0f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            movementDirection += new Vector3(0.0f, 0.0f, -1.0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            movementDirection += new Vector3(-1.0f, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementDirection += new Vector3(1.0f, 0.0f, 0.0f);
        }

        //Move player
        _rigidbody.AddForce(movementDirection.normalized * GameManager.PlayerSpeed * Time.deltaTime);
    }
}
