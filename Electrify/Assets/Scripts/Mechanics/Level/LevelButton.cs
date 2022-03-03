using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class LevelButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int level;
    [HideInInspector] public Level myLevel;

    [Header("Interaction")]
    public Color OnHoverColor;
    public Color OnPressColor;
    [Header("Default")]
    public Color DefaultColor;
    public Color CompleteColor;
    public Color LockedColor;
    [Header("Reference")]
    [SerializeField] private Image textBoxImage;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private GameObject lockIcon;

    private void Start()
    {
        myLevel = LevelManager.Instance.GetLevel(level);
        retrievePlayerPref();
        UpdateAppearance();
    }
    public void UpdateAppearance()
    {
        Color textBoxColor;
        if (myLevel.isLocked) textBoxColor = LockedColor;
        else if (myLevel.isCompleted) textBoxColor = CompleteColor;
        else textBoxColor = DefaultColor;

        if (textBoxImage != null) textBoxImage.color = textBoxColor;
        if (textMesh != null) textMesh.text = level.ToString();
        if(lockIcon != null) lockIcon.SetActive(myLevel.isLocked);
    }
    private Color getDefaultColor()
    {
        if (myLevel.isLocked) return LockedColor;
        else if (myLevel.isCompleted) return CompleteColor;
        else return DefaultColor;
    }
    public void retrievePlayerPref()
    {
        myLevel.isCompleted = Boolean.Parse(PlayerPrefs.GetString("Level " + level + " is Completed", myLevel.isCompleted.ToString()));
        myLevel.isLocked = Boolean.Parse(PlayerPrefs.GetString("Level " + level + " is Locked", myLevel.isLocked.ToString()));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!myLevel.isLocked) LevelManager.Instance.moveToLevelByMainMenu(level);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (textBoxImage != null) textBoxImage.color = OnPressColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textBoxImage != null) textBoxImage.color = OnHoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (textBoxImage != null) textBoxImage.color = getDefaultColor();
    }
}
