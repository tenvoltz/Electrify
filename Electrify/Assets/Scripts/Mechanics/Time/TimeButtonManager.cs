using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeButtonManager : MonoBehaviour
{
    [SerializeField] private List<TimeButton> timeButtons;

    private void Awake()
    {
        if (timeButtons == null)
        {
            timeButtons = new List<TimeButton>(GetComponentsInChildren<TimeButton>());
        }
    }

    public void SetTimeScale(TimeState timeState, TimeButton timeButton)
    {
        TimeManager.SetTimeScale(timeState);
        foreach(TimeButton button in timeButtons)
        {
            if (button != timeButton)
            {
                button.isPressed = false;
                if(button.timeState == TimeState.Resume)
                {
                    button.timeState = TimeState.Pause; //Set button to display pause if other mode is selected
                }
            }   
            button.SetDefaultColor(); //Update Appearance
            button.UpdateAppearance();
        }
    }

}
