using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private List<Level> levels;
    private void Awake()
    {
        Instance = this;
    }

    public void moveToLevelByGoal(int nextLevel) //Assume Level is ID of level which start at 1
    {
        if(nextLevel > levels.Count)
        {
            Debug.Log("You Win!!!");
        }
        else
        {
            StartCoroutine(switchFromSceneToScene("Level " + (nextLevel - 1), "Level " + nextLevel));
            Level nextLevelState = levels[nextLevel - 1];
            nextLevelState.Unlocked();
            nextLevelState.updatePlayerPref();
            if (nextLevel != 1)
            {
                Level currentLevelState = levels[nextLevel - 2];
                currentLevelState.Completed();
                currentLevelState.updatePlayerPref();
            }     
        }
    }
    public void moveToLevelByMainMenu(int level) 
    {
        StartCoroutine(switchFromSceneToScene("Main Menu", "Level " + level));
    }
    public void resetLevel(int currentLevel)
    {
        StartCoroutine(switchFromSceneToScene("Level " + currentLevel, "Level " + currentLevel));
    }
    public void returnToMainMenu(int currentLevel)
    {
        StartCoroutine(switchFromSceneToScene("Level " + currentLevel, "Main Menu"));
    }
    public IEnumerator switchFromSceneToScene(String unloadScene, String loadScene)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(unloadScene);
        yield return asyncOperation;
        LeanTween.reset();
        TimeManager.Reset();
        asyncOperation = SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);
        yield return asyncOperation;
    }
    public int getCurrentLevel()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name != "NeverUnload")
            {
                string levelName = SceneManager.GetSceneAt(i).name;
                return Int32.Parse(Regex.Match(levelName, @"\d+").Value);
            }
        }
        return 0;
    }
}
