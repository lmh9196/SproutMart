using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;


/*
 * [[1, 4], [9, 2], [3, 8], [11, 6]]
 */
public class Solution
{

    public int solution(int[,] dots)
    {
        int answer = 0;
        int result = 0;
        for (int i = 0; i < dots.GetLength(0); i++)
        {
            int xValue = 0;
            int yValue = 0;

            for (int j = 0; j < dots.GetLength(1); j++)
            {
                if(i > 0 )
                {
                    if (j < dots.GetLength(1)) { xValue = Math.Abs(dots[i, j]) - Math.Abs(dots[i - 1, j]); }
                    else { yValue = Math.Abs(dots[i, j - 1]) - Math.Abs(dots[i - 1, j - 1]);}
                }
            }

            if(result== 0) { result = yValue / xValue; }
            else if (result == yValue / xValue) { answer = 1; break; }
        }

        return answer;
    }
}