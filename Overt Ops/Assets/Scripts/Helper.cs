using System;
using UnityEngine;

public class Helper : MonoBehaviour
{
    /// <summary>
     /// Returns true if all booleans in range are true, else false
     /// </summary>
     /// <param name="booleans">An array of booleans</param>
     /// <param name="start">Start index (inclusive)</param>
     /// <param name="end">End index (inclusive), supports EOL indexing (-1 for last term)</param>
     /// <returns>Returns whether or not all bools in an array are false.</returns>
    public static bool AllTrue(bool[] booleans, int start, int end)
    {
        int maxIndice = booleans.Length;
        if(end < 0)
            end = maxIndice + end + 1;
        else if(end > maxIndice)
            end = maxIndice;
        else if(start > end)
        {
            throw new ArgumentException("Cannot start after the end index.", "start");
        }
        for(int i = start; i <= end; i++)
        {
            if(!booleans[i])
                return false;
        }
        return true;
    }
    public static bool AllFalse(bool[] booleans, int start, int end)
    {
        int maxIndice = booleans.Length -1;
        if(end < 0)
            end = maxIndice + end + 1;
        else if(end > maxIndice)
            end = maxIndice;
        else if(start > end)
        {
            throw new ArgumentException("Cannot start after the end index.", "start");
        }
        for(int i = start; i <= end; i++)
        {
            if(booleans[i])
                return false;
        }
        return true;
    }
}
