using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    [SerializeField] private float duration;
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
                timerObject.GetComponent<Timer>().countDown(duration, WinLevel);
            }
        }
    }

    private void WinLevel()
    {
        Debug.Log("The Goal is good", this);
        LevelManager.Instance.moveToLevelByGoal(LevelManager.Instance.getCurrentLevel() + 1);
    }
}
