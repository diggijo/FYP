using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControlCrane : MonoBehaviour
{
    [SerializeField] GameObject trolley;
    [SerializeField] GameObject hoist;
    [SerializeField] GameObject container;
    internal float trolley_percent;
    internal float hoist_percent;
    private float movementSpeed = 3f;
    internal float trolley_pos;
    internal float hoist_pos;
    private GameObject lineObject; // Reference to the GameObject containing the Line Renderer component
    private LineRenderer lineRenderer;
    private float SecondTimer;
    private const float SECOND = 1f;
    private int positionCount;

    private void Update()
    {
        trolley_pos = trolley.transform.position.z;
        hoist_pos = hoist.transform.position.y;

        SecondTimer += Time.deltaTime;
        if (SecondTimer >= SECOND)
        {
            if (lineRenderer != null)
            {
                lineRenderer.positionCount = positionCount + 1;
                lineRenderer.SetPosition(positionCount, new Vector3(transform.position.x, hoist_pos, trolley_pos - 14f));
                positionCount++;
                SecondTimer = 0;
            }
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 horizontalMovement = Vector3.forward * horizontalInput * movementSpeed * Time.deltaTime;
        Vector3 verticalMovement = Vector3.up * verticalInput * movementSpeed * Time.deltaTime;

        trolley.transform.Translate(horizontalMovement);

        hoist.transform.Translate(verticalMovement);     

        if (Input.GetKeyDown(KeyCode.Q))
        {
            container.SetActive(!container.activeSelf);

            if (container.activeSelf)
            {
                // If the container is active, create a new Line Renderer
                CreateNewLineRenderer();
            }
            else
            {
                // If the container is not active, destroy the Line Renderer if it exists
                DestroyLineRenderer();
            }
        }
    }

    private void CreateNewLineRenderer()
    {
        lineObject = new GameObject("LineRenderer");
        lineRenderer = lineObject.AddComponent<LineRenderer>();
        positionCount = 0;
        lineRenderer.material.color = Color.black;
    }

    private void DestroyLineRenderer()
    {
        if (lineObject != null)
        {
            Destroy(lineObject);
        }
    }
}
