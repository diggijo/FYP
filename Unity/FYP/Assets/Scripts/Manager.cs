using System;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    //GameObjects
    public GameObject STS;
    
    public GameObject Container20ft;

    [SerializeField] private Text hoistPosString;
    [SerializeField] private Text trolleyPosString;
    [SerializeField] private Text timeUI;
    [SerializeField] private Text dateUI;
    [SerializeField] private Text WindSpeed;
    [SerializeField] private Text Tonnage;
    [SerializeField] internal Text TwistLockStatus;
    [SerializeField] private Image LockIcon;

    private CraneMovement craneMovement;
    private ReadData data;

    private void Awake()
    {
        craneMovement = STS.GetComponent<CraneMovement>();
        data = GetComponent<ReadData>();
    }
    void FixedUpdate()
    {
        UIUpdate();

        timeUI.text = data.date.ToString("HH:mm:ss");
        dateUI.text = data.date.ToString("dd/MM/yyyy");
    }

    private void UIUpdate()
    {
        trolleyPosString.text = data.trolleyPosString;
        hoistPosString.text = data.hoistPosString;
        Tonnage.text = data.totalLoad.ToString("0.0");
        WindSpeed.text = data.windSpeed.ToString("0.0");

        if (data.isLocked)
        {
            TwistLockStatus.text = "Locked";
            TwistLockStatus.color = Color.red;
            LockIcon.color = Color.red;
        }

        else if (data.isUnlocked)
        {
            TwistLockStatus.text = "Unlocked";
            TwistLockStatus.color = Color.green;
            LockIcon.color = Color.green;
        }

        else
        {
            TwistLockStatus.color = Color.black;
            LockIcon.color = Color.black;

            if (TwistLockStatus.text == "Locked")
            {
                TwistLockStatus.text = "Unlocking...";
            }

            else if (TwistLockStatus.text == "Unlocked")
            {
                TwistLockStatus.text = "Locking...";
            }
        }

        if(data.hasContainer)
        {
            Container20ft.gameObject.SetActive(true);
        }
        else
        {
            Container20ft.gameObject.SetActive(false);
        }
    }
    
    public float TrolleyDisplacementValues()
    {
        return craneMovement.trolley_percent;
    }

    internal float SpreaderDisplacementValues()
    {
        return craneMovement.hoist_percent;
    }
}
