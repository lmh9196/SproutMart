using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Solution
{

    public int[] solution(int[] numbers, int num1, int num2)
    {
        int[] answer = new int[] { };

        Array.Copy(answer, 0, numbers, num1, num2);

        return answer;
    }
}

