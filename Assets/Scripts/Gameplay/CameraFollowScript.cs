using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] private float offsetY = 60;
    [SerializeField] private float offsetZ = -20;
    public static GameObject target; //Object camera will follow

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + offsetY, target.transform.position.z + offsetZ);
    }
}
