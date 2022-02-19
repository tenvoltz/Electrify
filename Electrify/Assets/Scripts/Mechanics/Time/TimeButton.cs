using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimeButton : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TimeState timeState;
    public bool isPressed = false;
    [SerializeField] Image buttonImage;
    [SerializeField] TimeButtonManager timeButtonManager;
    private Image image;
    [Header("Image")]
    public Sprite QuarterSpeedSprite;
    public Sprite HalfSpeedSprite;
    public Sprite PauseSprite;
    public Sprite ResumeSprite;
    public Sprite TwiceSpeedSprite;
    public Sprite QuadrupleSpeedSprite;
    [Header("Interaction")]
    public Color OnHoverColor;
    public Color OnPressColor;
    public Color DefaultColor;
    public Color InEffectColor;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Start()
    {
        UpdateAppearance();
        if(timeState == TimeState.Pause) //Begin with a Pause
        {
            isPressed = true;
            timeButtonManager.SetTimeScale(timeState, this);
            timeState = TimeState.Resume;
            UpdateAppearance();
        }
    }
    public void UpdateAppearance()
    {
        switch (timeState)
        {
            case TimeState.QuarterSpeed:
                buttonImage.sprite = QuarterSpeedSprite;
                break;
            case TimeState.HalfSpeed:
                buttonImage.sprite = HalfSpeedSprite;
                break;
            case TimeState.Pause:
                buttonImage.sprite = PauseSprite;
                break;
            case TimeState.Resume:
                buttonImage.sprite = ResumeSprite;
                break;
            case TimeState.TwiceSpeed:
                buttonImage.sprite = TwiceSpeedSprite;
                break;
            case TimeState.QuadrupleSpeed:
                buttonImage.sprite = QuadrupleSpeedSprite;
                break;
            default:
                Debug.Log("Something went wrong with updateAppearance", this);
                break;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isPressed = !isPressed;
        timeButtonManager.SetTimeScale(timeState, this);
        if (timeState == TimeState.Pause)
        {
            timeState = TimeState.Resume;
            UpdateAppearance();
        }
        else if (timeState == TimeState.Resume)
        {
            timeState = TimeState.Pause;
            UpdateAppearance();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (image != null) image.color = OnPressColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null) image.color = OnHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetDefaultColor();
    }

    public void SetDefaultColor()
    {
        if (image != null) image.color = isPressed ? InEffectColor : DefaultColor;
    }
}
