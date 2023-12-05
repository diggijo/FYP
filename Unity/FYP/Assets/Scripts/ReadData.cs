using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadData : MonoBehaviour
{
    public string csvFilePath = "Data/CraneData.csv";
    private const float LOOP_DELAY = 0;//.2f;
    internal float trolleyPos;

    private void Start()
    {
        StartCoroutine(readCSV());
    }

    private IEnumerator readCSV()
    {
        string fullPath = Path.Combine(Application.dataPath, csvFilePath);

        if (!File.Exists(fullPath))
        {
            Debug.LogError("CSV file not found: " + fullPath);
        }

        string csvFileText = File.ReadAllText(fullPath);

        StringReader reader = new StringReader(csvFileText);
        string headerLine = reader.ReadLine();

        string[] headers = headerLine.Split(',');

        while (reader.Peek() != -1)
        {

            string line = reader.ReadLine();

            string[] rowData = line.Split(',');

            string trolleyPosString = rowData[Array.IndexOf(headers, "Trolley_Position")];

            trolleyPos = float.Parse(trolleyPosString);

            yield return new WaitForSeconds(LOOP_DELAY);
        }
    }
}
