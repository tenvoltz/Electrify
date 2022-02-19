using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeState
{
    QuarterSpeed,
    HalfSpeed,
    Pause,
    Resume,
    TwiceSpeed,
    QuadrupleSpeed
}
public class TimeManager : MonoBehaviour
{
    public static bool isPaused = false;
    public static void SetTimeScale(TimeState timeState)
    {
        switch (timeState)
        {
            case TimeState.QuarterSpeed:
                isPaused = false;
                Time.timeScale = 0.25f;
                break;
            case TimeState.HalfSpeed:
                isPaused = false;
                Time.timeScale = 0.5f;
                break;
            case TimeState.Pause:
                isPaused = true;
                Time.timeScale = 0f;
                break;
            case TimeState.Resume:
                isPaused = false;
                Time.timeScale = 1f;
                break;
            case TimeState.TwiceSpeed:
                isPaused = false;
                Time.timeScale = 2f;
                break;
            case TimeState.QuadrupleSpeed:
                isPaused = false;
                Time.timeScale = 4f;
                break;
            default:
                Debug.Log("Something went wrong with setTimeScale");
                break;
        }
    }
}
