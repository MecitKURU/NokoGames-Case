using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Target;
    public Transform Camera;
    public float FollowSpeed;

    private Vector3 _offset;
    private void Start()
    {
        _offset = Camera.position - Target.position;
    }
    private void LateUpdate()
    {
        Vector3 destination = Target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(Camera.position, destination, Time.deltaTime * FollowSpeed);
        Camera.position = smoothedPosition;
    }

}
