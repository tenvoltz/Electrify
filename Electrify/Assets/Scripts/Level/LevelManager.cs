using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public int mainMenuSceneIndex = 1;
    [SerializeField] private List<Level> levels;
    private void Awake()
    {
        Instance = this;
    }

    public void moveToLevelByGoal(int level) //Assume Level is ID of level which start at 1
    {
        if(level  > levels.Count)
        {
            Debug.Log("You Win!!!");
        }
        else
        {
            SceneManager.LoadSceneAsync(level + mainMenuSceneIndex, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(level + mainMenuSceneIndex - 1);
            Level nextLevel = levels[level - 1];
            nextLevel.Unlocked();
            nextLevel.updatePlayerPref();
            if (level != 1)
            {
                Level currentLevel = levels[level - 2];
                currentLevel.Completed();
                currentLevel.updatePlayerPref();
            }     
        }
    }
    public void moveToLevelByMainMenu(int level) //Assume Level is ID of level which start at 1
    {
        SceneManager.LoadSceneAsync(level + mainMenuSceneIndex, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(level + mainMenuSceneIndex - 1);
    }
}
