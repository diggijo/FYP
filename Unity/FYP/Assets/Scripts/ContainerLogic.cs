using UnityEngine;

public class ContainerLogic : MonoBehaviour
{
    [SerializeField] private ReadData rd;
    [SerializeField] private GameObject container;

    void Update()
    {
        if (rd.hasContainer)
        {
            container.SetActive(true);
        }

        else
        {
            container.SetActive(false);
        }
    }
}
