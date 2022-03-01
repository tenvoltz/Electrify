using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goalable : MonoBehaviour
{
    [HideInInspector] public PhysicsObject physicsObject;
    public GoalDetector myGoal;
    public void Init()
    {
        physicsObject = GetComponent<PhysicsObject>();
        physicsObject.UI.UpdateGoalColor(myGoal);
    }
}
