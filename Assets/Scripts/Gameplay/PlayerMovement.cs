using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb; //Player's rigidbody component
    private SpawnProjectiles _projectileSpawner;

    public float _movementSpeed = 7000.0f; //Speed multiplier for player movement

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>(); //Get rigidbody component
        _projectileSpawner = GetComponent<SpawnProjectiles>();

    }

    // Update is called once per frame
    private void Update()
    {
        if (_rb != null)
        {
            MovePlayer();
            RotateToMouse.GetMousePosition(gameObject);
        }

       
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            _projectileSpawner.SpawnParticle();
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

        _rb.AddForce(movementDirection.normalized * _movementSpeed * Time.deltaTime);
    }
}
