using System.Collections.Generic;
using System.Net.Http;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;
using System.IO;

public class LineRendererManager : MonoBehaviour
{
    [SerializeField] internal ReadData data;
    private LineRenderer lineRenderer;
    private Line line;
    private GameObject lineObject;
    private int positionCount;
    private int cycle = 0;
    private float halfSecondTimer = 0;
    private const float halfSecond = .5f;
    private DateTime startTime;
    private DateTime endTime;
    private double totalTime;
    private string arcPath = "Assets/Data/ArcValues.csv";
    private string liftCyclePath = "Assets/Data/LiftCycles.csv";
    private const float MILLISECONDS = 1000f;

    void Update()
    {
        halfSecondTimer += Time.deltaTime;

        if (halfSecondTimer >= halfSecond)
        {
            if(lineRenderer != null)
            {
                lineRenderer.positionCount = positionCount + 1;
                lineRenderer.SetPosition(positionCount, transform.position);
                line.AddPoint(data.trolleyPos, data.hoistPos, data.date, data.modeChar);
                positionCount++;
                halfSecondTimer = 0;
            }
        }


        if (data.activeMove)
        {
            if (data.moveStarted)
            {
                cycle++;
                CreateNewLineRenderer();
            }
        }

        if (data.moveFinished)
        {
            GetPoints(line);

            if (lineObject != null)
            {
                Destroy(lineObject);
            }
        }
    }

    private void CreateNewLineRenderer()
    {
        lineObject = new GameObject("Line " + cycle);
        lineRenderer = lineObject.AddComponent<LineRenderer>();
        positionCount = 0;
        lineRenderer.material.color = Color.yellow;
        line = new Line();
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
            await WriteToArcValues(arcPath, cycle, i, point.Trolley_Position, point.Hoist_Position, point.DateTime, point.Mode);
            //await SendToArcValues(cycleID, i, point.Trolley_Position, point.Hoist_Position, point.DateTime, point.Mode);

            startTime = line.points[0].DateTime;
            endTime = line.points[line.points.Count - 1].DateTime;
            TimeSpan duration = endTime - startTime;
            totalTime = duration.TotalMilliseconds / MILLISECONDS;
        }

        await WriteToLiftCycles(liftCyclePath, cycle, startTime, endTime, totalTime);
        //await SendToLiftCycles(cycleID, startTime, endTime, totalTime);
    }


    private async Task WriteToArcValues(string filePath, int cycleID, int point, float trolleyPos, float hoistPos, DateTime dateTime, char mode)
    {
        StringBuilder csvContent = new StringBuilder();

        string csvLine = $"{cycleID},{point},{trolleyPos},{hoistPos},{dateTime},{mode}{Environment.NewLine}";

        csvContent.AppendLine(csvLine);

        try
        {
            await File.AppendAllTextAsync(filePath, csvLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing to the ArcValues file: {ex.Message}");
        }
    }

    private async Task WriteToLiftCycles(string filePath, int cycleID, DateTime startTime, DateTime endTime, double totalTime)
    {
        StringBuilder csvContent = new StringBuilder();

        string csvLine = $"{cycleID},{startTime},{endTime},{totalTime}{Environment.NewLine}";

        csvContent.AppendLine(csvLine);

        try
        {
            await File.AppendAllTextAsync(filePath, csvLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing to the LiftCycles file: {ex.Message}");
        }
    }

    /*
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
    }*/
}