using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipperScript : MonoBehaviour
{
    //CHANGE THIS SCRIPT ONCE I GET FLIPPER VALUES

    [SerializeField] private ReadData data;
    [SerializeField] private Image[] flippers;

    void Update()
    {
        if (data.isLocked)
        {
            changeFlippers(Color.red);
        }

        else if (data.isUnlocked)
        {
            changeFlippers(Color.green);
        }
    }

    private void changeFlippers(Color color)
    {
        for (int i = 0; i < flippers.Length; i++)
        {
            flippers[i].color = color;
        }
    }
}
