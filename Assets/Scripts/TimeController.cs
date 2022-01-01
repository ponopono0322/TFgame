using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static System.Diagnostics.Stopwatch watch;
    void Start()
    {
        watch = new System.Diagnostics.Stopwatch();
        watch.Start();
    }

    void OnApplicationQuit()
    {
        watch.Stop();
    }

}
