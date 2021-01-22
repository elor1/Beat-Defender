using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    private const float OFFSET_Y = 16.24f; //Camera's offset on the Y axis
    private const float OFFSET_Z = -10.3f;//Camera's offeset on the Z axis

    private static GameObject _target; //Object camera will follow

    public static GameObject Target { set { _target = value; } }
    
    // Update is called once per frame
    private void Update()
    {
        //Update camera's position
        transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y + OFFSET_Y, _target.transform.position.z + OFFSET_Z);
    }
}
