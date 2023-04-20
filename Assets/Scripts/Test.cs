using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using UnityEngine.Rendering;

public class Solution
{
    public int solution(int[,] dots)
    {
        int answer = 0;
        List<int> intList = new();

        float[] compareArray = new float[2];

        float[] axy = new float[2];
        float[] bxy = new float[2];

        int startIdx = 0;

        for (int i = 0; i < dots.GetLength(0) - 1; i++)
        {
            for (int a = 0; a < axy.Length; a++)
            {
                if(!intList.Contains(i) && startIdx <= i)
                {
                    intList.Add(i);
                    axy[a] = Math.Abs(dots[i, a]);

                    if (intList.Count == 0) { startIdx = i; }
                }
            }

            for (int a = 0; a < bxy.Length; a++)
            {
                if (!intList.Contains(i))
                {
                    intList.Add(i);
                    bxy[a] = Math.Abs(dots[i, a]);
                }
            }

            for (int a = 0; a < compareArray.Length; a++)
            {
                float xValue = Math.Abs(axy[a] - bxy[a]);
                float yValue = Math.Abs(axy[a] - bxy[a]);

                if (xValue != 0 && yValue != 0) { compareArray[a] = yValue / xValue; }
                else { compareArray[a] = 0; }
            }

            if (compareArray[0] == compareArray[1]) { return 1; }
            else { intList.Clear();  }
        }

        return answer;
    }
}