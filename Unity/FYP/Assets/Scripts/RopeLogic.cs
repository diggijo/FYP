using UnityEngine;

public class RopeLogic : MonoBehaviour
{
    [SerializeField] private Transform trolleyConnector;
    [SerializeField] private Transform spreaderConnector;

    private void Update()
    {
        Vector3 midpoint = (trolleyConnector.position + spreaderConnector.position) / 2f;

        transform.localPosition = new Vector3(transform.localPosition.x, midpoint.y, transform.localPosition.z);

        float cylinderScale = trolleyConnector.position.y - midpoint.y;
        transform.localScale = new Vector3(transform.localScale.x, cylinderScale, transform.localScale.z);

        Vector3 spreaderToTrolley = trolleyConnector.position - spreaderConnector.position;
        float angle = Vector3.Angle(Vector3.up, spreaderToTrolley);

        if (gameObject.tag == "CylinderLeft")
        {
            transform.localRotation = Quaternion.Euler(-angle, transform.localRotation.y, transform.localRotation.z);
        }

        else if (gameObject.tag == "CylinderRight")
        {
            transform.localRotation = Quaternion.Euler(angle, transform.localRotation.y, transform.localRotation.z);
        }

        else if (gameObject.tag == "MiddleBack")
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, angle);
        }

        else
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, -angle);
        }
    }
}