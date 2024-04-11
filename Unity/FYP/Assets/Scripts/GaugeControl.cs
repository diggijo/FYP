using UnityEngine;
using UnityEngine.UI;

public class GaugeControl : MonoBehaviour
{
    [SerializeField] private Image OutputDial;
    [SerializeField] private GameObject Manager;
    private Manager manager;
    private float PercentageValue;
    private bool DisplayTrolleyDisplacement;
    private bool DisplaySpreaderDisplacement;

    void Start()
    {
        if (gameObject.tag == "Spreader")
        {
            DisplaySpreaderDisplacement = true;
        }

        else if (gameObject.tag == "Trolley")
        {
            DisplayTrolleyDisplacement = true;
        }

        manager = Manager.GetComponent<Manager>();
    }

    void Update()
    {
        if (OutputDial != null)
        {
            OutputDial.fillAmount = PercentageValue;
        }

        if (DisplayTrolleyDisplacement)
        {
            PercentageValue = manager.TrolleyDisplacementValues();
        }

        if (DisplaySpreaderDisplacement)
        {
            PercentageValue = manager.SpreaderDisplacementValues();
        }
    }
}
