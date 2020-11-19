using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    [SerializeField] private float _offsetY = 60;
    [SerializeField] private float _offsetZ = -20;
    public static GameObject _target; //Object camera will follow

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y + _offsetY, _target.transform.position.z + _offsetZ);
    }
}
