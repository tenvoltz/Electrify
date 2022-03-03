using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int ID;
    public string levelName;
    [TextArea(5, 10), Multiline()]
    public string levelDescription;
    [HideInInspector] public bool isCompleted = false;
    public bool isLocked = true;
    public void Completed()
    {
        isCompleted = true;
    }
    public void Incompleted()
    {
        isCompleted = false;
    }
    public void Locked()
    {
        isLocked = true;
    }
    public void Unlocked()
    {
        isLocked = false;
    }
    public void updatePlayerPref()
    {
        PlayerPrefs.SetString("Level" + ID + "isCompleted", isCompleted.ToString());
        PlayerPrefs.SetString("Level" + ID + "isLocked", isLocked.ToString());
    }
}
