using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody _rb; //Player's rigidbody component

    public float _movementSpeed = 7000.0f; //Speed multiplier for player movement

    [SerializeField] private float maximumRayLength = 40.0f;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>(); //Get rigidbody component

    }

    // Update is called once per frame
    private void Update()
    {
        if (_rb != null)
        {
            MovePlayer();
            GetMousePosition();
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

    /// <summary>
    /// Uses a raycast to get the position of the mouse
    /// </summary>
    private void GetMousePosition()
    {
        RaycastHit hit;
        Vector3 mousePosition = Input.mousePosition;
        Ray rayMouse = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, maximumRayLength))
        {
            RotatePlayer(hit.point);
        }
        else
        {
            RotatePlayer(rayMouse.GetPoint(maximumRayLength));
        }
    }

    /// <summary>
    /// Rotates the player in the Y axis to look at a given world point
    /// </summary>
    /// <param name="destination">Point to look at</param>
    private void RotatePlayer(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.Set(0.0f, rotation.y, 0.0f, rotation.w);
        transform.localRotation = Quaternion.Lerp(transform.rotation, rotation, 1);
    }
}
