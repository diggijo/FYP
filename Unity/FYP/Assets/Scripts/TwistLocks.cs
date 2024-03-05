using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwistLocks : MonoBehaviour
{
    [SerializeField] private ReadData data;
    [SerializeField] private Image[] twistLocks;

    void Update()
    {
        if (data.isLocked)
        {
            changeLocks(Color.red);
        }

        else if (data.isUnlocked)
        {
            changeLocks(Color.green);
        }
    }

    private void changeLocks(Color color)
    {
        int iterations;

        if (data.totalLoad > 45f)
        {
            iterations = 7;
        }

        else
        {
            iterations = 3;
        }

        for (int i = 0; i <= iterations; i++)
        {
            twistLocks[i].color = color;
        }
    }
}
