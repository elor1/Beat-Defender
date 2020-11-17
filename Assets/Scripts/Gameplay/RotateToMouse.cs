using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse
{
    [SerializeField] private static float _maximumRayLength = 40.0f;

    /// <summary>
    /// Uses a raycast to get the position of the mouse
    /// </summary>
    public static void GetMousePosition(GameObject obj)
    {
        RaycastHit hit;
        Vector3 mousePosition = Input.mousePosition;
        Ray rayMouse = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(rayMouse.origin, rayMouse.direction, out hit, _maximumRayLength))
        {
            RotatePlayer(obj, hit.point);
        }
        else
        {
            RotatePlayer(obj, rayMouse.GetPoint(_maximumRayLength));
        }
    }

    /// <summary>
    /// Rotates the player in the Y axis to look at a given world point
    /// </summary>
    /// <param name="destination">Point to look at</param>
    private static void RotatePlayer(GameObject obj, Vector3 destination)
    {
        Vector3 direction = destination - obj.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation.Set(0.0f, rotation.y, 0.0f, rotation.w);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }
}
