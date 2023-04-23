using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaVantageTimer : MonoBehaviour
{
    private static Queue<float> apiCallTimes = new Queue<float>();

    public static bool IsApiCallAllowed => apiCallTimes.Count < 50; //Allow 50 api calls per minute

    public static void UseOutApiCall() => apiCallTimes.Enqueue(Time.time);


    private void Update()
    {
        //Remove saved api call if minute passed
        if(apiCallTimes.Count > 0)
        {
            if (Time.time - apiCallTimes.Peek() > 60.0f)
            {
                apiCallTimes.Dequeue();
            }
        }
    }
}
