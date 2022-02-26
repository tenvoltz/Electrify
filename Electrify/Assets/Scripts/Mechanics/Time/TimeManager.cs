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
                Time.fixedDeltaTime = 0.005f;
                break;
            case TimeState.HalfSpeed:
                isPaused = false;
                Time.timeScale = 0.5f;
                Time.fixedDeltaTime = 0.01f;
                break;
            case TimeState.Pause:
                isPaused = true;
                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0f;
                break;
            case TimeState.Resume:
                isPaused = false;
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                break;
            case TimeState.TwiceSpeed:
                isPaused = false;
                Time.timeScale = 2f;
                Time.fixedDeltaTime = 0.04f;
                break;
            case TimeState.QuadrupleSpeed:
                isPaused = false;
                Time.timeScale = 4f;
                Time.fixedDeltaTime = 0.08f;
                break;
            default:
                Debug.Log("Something went wrong with setTimeScale");
                break;
        }
    }

    public static void Reset()
    {
        SetTimeScale(TimeState.Resume);
    }
}
