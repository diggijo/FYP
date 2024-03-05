using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;          
    [SerializeField] private Transform perspectiveView;
    private float smoothSpeed = 0.125f;                 
    private Quaternion originalRotation;
    private Vector3 offset;                             
    internal bool followOn = true;

    private void Start()
    {
        originalRotation = transform.rotation;
        offset = transform.position - target.position;
    }
    void LateUpdate()
    {
        if (followOn)
        {
            followSpreader();
        }

        else
        {
            perspectiveCamera();
        }
    }

    private void perspectiveCamera()
    {
        Vector3 desiredPosition = perspectiveView.position;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
        transform.rotation = perspectiveView.rotation;
    }

    private void followSpreader()
    {
        Vector3 desiredPosition = target.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
        transform.rotation = originalRotation;
    }
}