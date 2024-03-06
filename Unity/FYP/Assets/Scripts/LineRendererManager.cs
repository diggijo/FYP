using System.Collections.Generic;
using System.Net.Http;
using System;
using UnityEngine;
using System.Threading.Tasks;

public class LineRendererManager : MonoBehaviour
{
    [SerializeField] private CraneMovement cm;
    [SerializeField] internal ReadData data;
    private LineRenderer lineRenderer;
    private LineRenderer automationLineRenderer;
    private Line line;
    private GameObject lineObject;
    private int positionCount;
    private int automationPosCount;
    private float quarterSecondTimer = .25f;
    private const float quarterSecond = .25f;
    private int cycle = 1;
    private int previousMode = -1;
    private float secondTimer = 0;
    private const float oneSecond = 1f;
    private float liftCycleTimer = 0;
    private int cycleID = 1;
    private DateTime startTime;
    private DateTime endTime;
    private float totalTime;

    void Start()
    {
        CreateNewLineRenderer();
        CreateAutomationLine();
    }

    void Update()
    {
        quarterSecondTimer += Time.deltaTime;
        secondTimer += Time.deltaTime;
        liftCycleTimer += Time.deltaTime;

        if (quarterSecondTimer >= quarterSecond)
        {
            automationLineRenderer.positionCount = automationPosCount + 1;
            automationLineRenderer.SetPosition(automationPosCount, transform.position);
            automationPosCount++;
            quarterSecondTimer = 0;
        }

        if (secondTimer >= oneSecond)
        {
            lineRenderer.positionCount = positionCount + 1;
            lineRenderer.SetPosition(positionCount, transform.position);
            line.AddPoint(data.trolleyPos, data.hoistPos, data.date, data.modeChar);
            positionCount++;
            secondTimer = 0;
        }

        if (data.modeInt != previousMode)
        {
            CreateAutomationLine();
            previousMode = data.modeInt;
        }

        if (!data.hasContainer && data.containersCarried >= cycle)
        {
            cycle++;
            GetPoints(line);
            CreateNewLineRenderer();
            totalTime = liftCycleTimer;
            Debug.Log("Lift Time: " + totalTime);
            liftCycleTimer = 0;
        }
    }

    private void CreateNewLineRenderer()
    {
        if (lineObject != null)
        {
            Destroy(lineObject);
        }

        lineObject = new GameObject("Line " + cycle);
        lineRenderer = lineObject.AddComponent<LineRenderer>();
        positionCount = 0;
        lineRenderer.material.color = Color.yellow;
        line = new Line();
    }

    private void CreateAutomationLine()
    {
        Color lineColor = data.modeInt == 0 ? Color.red : Color.blue;
        GameObject newLineObject = new GameObject("Line");
        automationLineRenderer = newLineObject.AddComponent<LineRenderer>();
        automationLineRenderer.material.color = lineColor;
        automationPosCount = 0;
    }

    private async void GetPoints(Line line)
    {
        if (line.points.Count == 0)
        {
            Debug.Log("Line is empty.");
            return;
        }

        for (int i = 0; i < line.points.Count; i++)
        {
            Line.Point point = line.points[i];
            await SendToArcValues(cycleID, i, point.Trolley_Position, point.Hoist_Position, point.DateTime, point.Mode);

            startTime = line.points[0].DateTime;
            endTime = line.points[line.points.Count - 1].DateTime;
        }

        await SendToLiftCycles(cycleID, startTime, endTime, totalTime);
        cycleID++;
    }

    private async Task SendToArcValues(int cycleID, int point, float trolleyPos, float hoistPos, DateTime dateTime, char mode)
    {
        string functionUrl = "http://localhost:7089/api/SendToArcValues";

        HttpClient httpClient = new HttpClient();

        var requestData = new Dictionary<string, string>
        {
        { "Cycle_ID", cycleID.ToString() },
        { "Point", point.ToString() },
        { "Trolley_Position", trolleyPos.ToString() },
        { "Hoist_Position", hoistPos.ToString() },
        { "DateTime", dateTime.ToString() },
        { "Mode", mode.ToString() },
        };

        var content = new FormUrlEncodedContent(requestData);

        HttpResponseMessage response = await httpClient.PostAsync(functionUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Failed to send the values. StatusCode: " + response.StatusCode);
        }
    }

    private async Task SendToLiftCycles(int cycleID, DateTime startTime, DateTime endTime, float totalTime)
    {
        string functionUrl = "http://localhost:7089/api/SendToLiftCycles";

        HttpClient httpClient = new HttpClient();

        var requestData = new Dictionary<string, string>
        {
        { "Cycle_ID", cycleID.ToString() },
        { "Start_Time", startTime.ToString() },
        { "End_Time", endTime.ToString() },
        { "Total_Time", totalTime.ToString() }
        };

        var content = new FormUrlEncodedContent(requestData);

        HttpResponseMessage response = await httpClient.PostAsync(functionUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Failed to send the values. StatusCode: " + response.StatusCode);
        }
    }
}