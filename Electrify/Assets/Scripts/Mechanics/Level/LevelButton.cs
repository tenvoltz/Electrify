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
    private bool isCompleted = false;
    private bool isLocked = true;

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

    public LevelButton(int _level, bool _isComplete, bool _isLocked)
    {
        this.level = _level;
        this.isCompleted = _isComplete;
        this.isLocked = _isLocked;
    }
    private void Awake()
    {
        retrievePlayerPref();
        UpdateAppearance();
    }

    public void UpdateAppearance()
    {
        Color textBoxColor;
        if (isLocked) textBoxColor = LockedColor;
        else if (isCompleted) textBoxColor = CompleteColor;
        else textBoxColor = DefaultColor;

        if (textBoxImage != null) textBoxImage.color = textBoxColor;
        if (textMesh != null) textMesh.text = level.ToString();
        if(lockIcon != null) lockIcon.SetActive(isLocked);
    }
    private Color getDefaultColor()
    {
        if (isLocked) return LockedColor;
        else if (isCompleted) return CompleteColor;
        else return DefaultColor;
    }

    public void retrievePlayerPref()
    {
        isCompleted = Boolean.Parse(PlayerPrefs.GetString("Level " + level + " is Completed", isCompleted.ToString()));
        isLocked = Boolean.Parse(PlayerPrefs.GetString("Level " + level + " is Locked", isLocked.ToString()));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        LevelManager.Instance.moveToLevelByMainMenu(level);
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
