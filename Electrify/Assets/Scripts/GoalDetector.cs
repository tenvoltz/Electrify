using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalDetector : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private int nextLevelToLoad;
    private void Start()
    {
        nextLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == targetObject)
        {
            Debug.Log("The Goal is good", this);
            LevelManager.Instance.moveToNextLevel(nextLevelToLoad);     
        }
    }
}
