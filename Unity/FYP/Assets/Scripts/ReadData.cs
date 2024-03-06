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
    internal string trolleyPosString;
    internal float hoistPos;
    internal string hoistPosString;
    internal DateTime date;
    private bool initialConditionMet = false;
    internal bool hasContainer = false;
    internal int containersCarried = 0;
    internal float totalLoad;
    internal float windSpeed;
    internal bool isLocked;
    internal bool isUnlocked;
    internal bool isLanded;
    internal int modeInt;
    internal char modeChar;

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

            trolleyPosString = rowData[Array.IndexOf(headers, "Trolley_Position")];
            hoistPosString = rowData[Array.IndexOf(headers, "Hoist_Position")];
            string dateTime = rowData[Array.IndexOf(headers, "Timestamp")];
            modeInt = int.Parse(rowData[Array.IndexOf(headers, "Mode")]);
            string windSpeedString = rowData[Array.IndexOf(headers, "Wind_Speed")];
            string totalLoadString = rowData[Array.IndexOf(headers, "Hoist_TotalLoad")];
            int isLockLocked = int.Parse(rowData[Array.IndexOf(headers, "TwistLockAreLocked")]);
            int isLockUnlocked = int.Parse(rowData[Array.IndexOf(headers, "TwistLockedAreUnlocked")]);
            int isSpreaderLanded = int.Parse(rowData[Array.IndexOf(headers, "SpreaderIsLanded")]);

            if (!hasContainer && isLockLocked > 0)
            {
                if (!initialConditionMet)
                {
                    initialConditionMet = true;
                    containersCarried++;
                }
                hasContainer = true;
            }

            else if (isLockLocked < 1 && isSpreaderLanded < 1)
            {
                initialConditionMet = false;
                hasContainer = false;
            }

            date = DateTime.Parse(dateTime);
            trolleyPos = float.Parse(trolleyPosString);
            hoistPos = float.Parse(hoistPosString);
            windSpeed = float.Parse(windSpeedString);
            totalLoad = float.Parse(totalLoadString);
            isLocked = isLockLocked == 1 ? true : false;
            isUnlocked = isLockUnlocked == 1 ? true : false;
            isLanded = isSpreaderLanded == 1 ? true : false;
            modeChar = checkMode();

            yield return new WaitForSeconds(LOOP_DELAY);
        }
    }

    private char checkMode()
    {
        if (modeInt == 0)
        {
            return 'M';
        }

        return 'A';
    }
}
