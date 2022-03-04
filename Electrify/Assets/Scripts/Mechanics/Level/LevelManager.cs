using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public static GameObject timerPrefab;
    public FadeScreen loadingScreen;
    public FadeScreen winningScreen;
    private List<Level> levels;
    private void Awake()
    {
        Instance = this;
        LoadMainMenuScene();
        levels = new List<Level>(GetComponentsInChildren<Level>());
    }
    public Level GetLevel(int levelID)
    {
        foreach(Level level in levels)
        {
            if (level.ID == levelID) return level;
        }
        return null;
    } 
    public void LoadMainMenuScene()
    {
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Additive);
    }
    public void moveToNextLevel()
    {
        moveToLevelByGoal(getCurrentLevel() + 1);
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
        }
    }
    public void UpdateLevelState()
    {
        int currentLevel = getCurrentLevel();
        Level currentLevelState = levels[currentLevel - 1];
        currentLevelState.Completed();
        currentLevelState.updatePlayerPref();
        int nextLevel = currentLevel + 1;
        if (nextLevel > levels.Count) return;
        Level nextLevelState = levels[nextLevel - 1];
        nextLevelState.Unlocked();
        nextLevelState.updatePlayerPref();
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
    public void returnToMainMenuFromLevel()
    {
        returnToMainMenu(getCurrentLevel());
    }
    public IEnumerator switchFromSceneToScene(String unloadScene, String loadScene)
    {
        loadingScreen.FadeIn();
        ProgressBar progressBar = loadingScreen.GetComponentInChildren<ProgressBar>();
        yield return null;
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(unloadScene);
        while (!asyncOperation.isDone)
        {
            progressBar.setFillPercentage(asyncOperation.progress / 2);
            yield return null;
        }

        LeanTween.reset();
        TimeManager.Reset();

        asyncOperation = SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            progressBar.setFillPercentage(asyncOperation.progress / 2 + 0.5f);
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
        progressBar.setFillPercentage(1);
        loadingScreen.FadeOut();
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
    public GameObject getTimerPrefab()
    {
        if (timerPrefab == null) timerPrefab = Resources.Load("Prefabs/Timer") as GameObject;
        return timerPrefab;
    }
}
