using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneMovement : MonoBehaviour
{
    [SerializeField] GameObject trolley;
    private float trolleyPosition;
    private const float MIN_T_DISTANCE = -30f;
    private const float MAX_T_DISTANCE = 90f;
    private ReadData data;

    private void Start()
    {
        data = FindObjectOfType<ReadData>();
        
    }

    private void Update()
    {
        trolleyPosition = data.trolleyPos;
        Debug.Log(trolleyPosition);

       Vector3 trolleyPositionVector = trolley.transform.position;

        trolleyPositionVector.z = trolleyPosition;

        trolley.transform.position = trolleyPositionVector;
    }
}
