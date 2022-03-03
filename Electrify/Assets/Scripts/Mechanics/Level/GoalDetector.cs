using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    [SerializeField] private float duration;
    private Timer myTimer;
    public Color color;
    private void Awake()
    {
        UpdateColor();
    }
    private void OnValidate()
    {
        UpdateColor();
    }
    private void UpdateColor()
    {
        foreach(Transform child in transform)
        {
            if(child != null && child.GetComponent<Renderer>() != null) 
                child.GetComponent<Renderer>().material.color = color;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Goalable>() == null) return;
        else
        {
            if(other.GetComponent<Goalable>().myGoal.gameObject == gameObject)
            {
                Vector3 position = transform.position + transform.TransformDirection(new Vector3(1, 1, 0)) * 2;
                GameObject timerObject = Instantiate(LevelManager.Instance.getTimerPrefab(), position, Quaternion.identity);
                timerObject.transform.SetParent(transform, true);
                myTimer = timerObject.GetComponent<Timer>();
                myTimer.CountDown(duration, WinLevel);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Goalable>() == null) return;
        else
        {
            if (other.GetComponent<Goalable>().myGoal.gameObject == gameObject)
            {
                if(myTimer != null) myTimer.CancelCountDown();
            }
        }
    }

    private void WinLevel()
    {
        Debug.Log("The Goal is good", this);
        LevelManager.Instance.UpdateLevelState();
        LevelManager.Instance.winningScreen.FadeIn();
    }
}
