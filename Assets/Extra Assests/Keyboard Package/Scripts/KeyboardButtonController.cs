using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardButtonController : MonoBehaviour
{
    [SerializeField] Image containerBorderImage;
    [SerializeField] Image containerFillImage;
    [SerializeField] Image containerIcon;
    [SerializeField] TextMeshProUGUI containerText;
    [SerializeField] TextMeshProUGUI containerActionText;

    void Start()
    {
        SetDefaultColor();
    }

    public void SetDefaultColor()
    {
        SetContainerBorderColor(ColorDataStore.GetKeyboardBorderColor());
        SetContainerFillColor(ColorDataStore.GetKeyboardFillColor());
        SetContainerTextColor(ColorDataStore.GetKeyboardTextColor());
        SetContainerActionTextColor(ColorDataStore.GetKeyboardActionTextColor());
    }

    public void SetContainerBorderColor(Color color)
    {
        color.a = 1.0f;
        containerBorderImage.color = color;
    }
    public void SetContainerFillColor(Color color) => containerFillImage.color = color;
    public void SetContainerTextColor(Color color) => containerText.color = color;

    public void SetContainerActionTextColor(Color color)
    {
        containerActionText.color = color;
        containerIcon.color = color;
    }

    public void AddLetter()
    {
        if (GameManager.s_instance != null)
        {
            GameManager.s_instance.AddKey(containerText.text.Trim().ToUpper());
            AudioManager.s_instance.Play(Constants.CLICK_SOUND);
        }
    }

    public void DeleteLetter()
    {
        if (GameManager.s_instance != null)
        {
            GameManager.s_instance.DeleteLastLetter();
            AudioManager.s_instance.Play(Constants.CLICK_SOUND);
        }
    }

    public void SubmitWord()
    {
        if (GameManager.s_instance != null)
        {
            GameManager.s_instance.EnterKeyPressed();
            AudioManager.s_instance.Play(Constants.CLICK_SOUND);
        }
    }

    public void SetColor(Color color)
    {
        SetContainerBorderColor(color);
    }

    public string GetChar()
    {
        return containerText.text.Trim().ToLower();
    } 
}