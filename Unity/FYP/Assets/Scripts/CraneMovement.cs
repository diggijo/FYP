using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneMovement : MonoBehaviour
{
    [SerializeField] GameObject trolley;
    [SerializeField] GameObject hoist;
    private float trolleyPosition;
    private float hoistPosition;
    private const float MIN_CRANE_DIST = 3.5f;
    private const float MAX_CRANE_DIST = 85f;
    private const float MIN_T_DISTANCE = -35f;
    private const float MAX_T_DISTANCE = 75f;
    private ReadData data;
    internal float trolley_percent;
    internal float hoist_percent;

    

    private void Start()
    {
        data = FindObjectOfType<ReadData>();       
    }

    private void Update()
    {
        trolleyPosition = data.trolleyPos;
        hoistPosition = data.hoistPos;

        Vector3 trolleyPositionVector = trolley.transform.position;

        float unityTrolleyPos = calculateUnityPosition(trolleyPosition);

        trolleyPositionVector.z = unityTrolleyPos;

        trolley.transform.position = trolleyPositionVector;
        hoist.transform.position = new Vector3(hoist.transform.position.x, hoistPosition, hoist.transform.position.z);

        // Map trolley movement value to a relative percentage
        trolley_percent = trolleyPosition / (MIN_CRANE_DIST + MAX_CRANE_DIST);

        // Calculate relative hoist position based on provided YMin and YMax
        float range = Mathf.Abs(-18) + Mathf.Abs(32);
        float relativePosition = Mathf.Abs(hoistPosition) / range;
        hoist_percent = relativePosition;
    }

    private float calculateUnityPosition(float trolleyPosition)
    {
        float topLine = ((trolleyPosition - MIN_CRANE_DIST) * (MAX_T_DISTANCE - MIN_T_DISTANCE));
        float bottomLine = MAX_CRANE_DIST - MIN_CRANE_DIST;

        float unityPos = (topLine / bottomLine) + MIN_T_DISTANCE;

        return unityPos;
    }
}
