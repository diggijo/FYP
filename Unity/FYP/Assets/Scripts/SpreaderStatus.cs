using UnityEngine;
using UnityEngine.UI;

public class SpreaderStatus : MonoBehaviour
{
    [SerializeField] private Image yellowLightImage;
    [SerializeField] private Image greenLightImage;
    [SerializeField] private Image redLightImage;
    [SerializeField] private Image greenYellowImage;
    [SerializeField] private Image redYellowImage;
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject greenLight;
    [SerializeField] private GameObject whiteLight;
    [SerializeField] ReadData data;

    private void Update()
    {
        UpdateLights(data.isLanded, data.isLocked);
    }

    public void UpdateLights(bool isLanded, bool isLocked)
    {
        greenLight.SetActive(false);
        whiteLight.SetActive(false);
        redLight.SetActive(false);
        yellowLightImage.gameObject.SetActive(false);
        greenLightImage.gameObject.SetActive(false);
        redLightImage.gameObject.SetActive(false);
        greenYellowImage.gameObject.SetActive(false);
        redYellowImage.gameObject.SetActive(false);

        if (isLanded)
        {
            yellowLightImage.gameObject.SetActive(true);
            whiteLight.SetActive(true);

            if (isLocked)
            {
                redYellowImage.gameObject.SetActive(true);
                redLight.SetActive(true);
            }
            else
            {
                greenYellowImage.gameObject.SetActive(true);
                greenLight.SetActive(true);
            }
        }

        else
        {
            if (isLocked)
            {
                redLightImage.gameObject.SetActive(true);
                redLight.SetActive(true);
            }
            else
            {
                greenLightImage.gameObject.SetActive(true);
                greenLight.SetActive(true);
            }
        }
    }
}