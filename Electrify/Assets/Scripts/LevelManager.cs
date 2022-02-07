using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<Button> levelButtons;
    private void Start()
    {
        int storedLevels = PlayerPrefs.GetInt("Finished Level", 1);
        for(int i = 0; i < levelButtons.Count; i++)
        {
            if(i + 1 > storedLevels) levelButtons[i].interactable = false;
        }
    }

    public static void moveToLevel(int level)
    {
        SceneManager.LoadScene(level);
        if (level > PlayerPrefs.GetInt("Finished Levels"))
        {
            PlayerPrefs.SetInt("Finished Levels", level);
        }
    }

}
