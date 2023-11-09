using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneMovement : MonoBehaviour
{
    [SerializeField] GameObject trolley;
    private float trolleyPos = 0;
    private const float MAX_T_DISTANCE = 100f;
    private const float MOVE_SPEED = 10f;
    private bool reachedEnd = false;

    private void Update()
    {
        if (!reachedEnd && trolleyPos < MAX_T_DISTANCE)
        {
            trolleyPos += MOVE_SPEED * Time.deltaTime;
            trolley.transform.Translate(Vector3.right * MOVE_SPEED * Time.deltaTime);
            
        }

        else
        {
            reachedEnd = true;
        }

        if (reachedEnd && trolleyPos > 0f)
        {
            trolleyPos -= MOVE_SPEED * Time.deltaTime;
            trolley.transform.Translate(Vector3.left * MOVE_SPEED * Time.deltaTime);
        }

        else
        {
            reachedEnd = false;
        }
    }
}
