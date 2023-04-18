using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
  
}

public class Solution
{
    string[] strArray = { "aya", "ye", "woo", "ma" };


    public int solution(string[] babbling)
    {
        int answer = 0;

        for (int i = 0; i < babbling.Length; i++)
        {
            for (int j = 0; j < strArray.Length; j++)
            {
                babbling[i] = babbling[i].Replace(strArray[j], "");
            }
            if (babbling[i].Length > 0) { answer++; }
        }
     
        return answer;
    }
}