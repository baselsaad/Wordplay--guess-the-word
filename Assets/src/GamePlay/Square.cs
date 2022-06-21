using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Square : MonoBehaviour
{
    //SerializeField:
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Sprite m_defaultImage, m_redImage, m_grennImage, m_orangeImage;
    [SerializeField] private bool m_isTutorial = false;


    //private:
    private Image m_image;
    private Animator m_animator;

    void Awake()
    {
        this.m_image = GetComponent<Image>();

        if (!m_isTutorial)
        {
            m_animator = GetComponent<Animator>();
        }
    }


    public void DisableGameObject()
    {
        if (!m_isTutorial)
        {
            this.gameObject.SetActive(false);
        }
    }

    public int GetRowPosition()
    {
        string input = this.gameObject.name;
        string pattern = "[^0-9]"; // replace Anything not Number

        Regex rgx = new Regex(pattern);
        string result = rgx.Replace(input, "");

        if (result.Length == 0)
        {
            return 0;
        }
        else
        {
            return int.Parse(result);
        }


    }

    public void ShowGameObject()
    {
        if (!m_isTutorial)
        {
            this.gameObject.SetActive(true);
        }
    }

    public void SetCharInSquare(string letter)
    {
        if (!m_isTutorial)
        {
            //Animation
            this.textMeshProUGUI.text = letter;
        }
    }

    public string GetKeyText()
    {
        string key = "";
        if (!m_isTutorial)
        {
            key = this.textMeshProUGUI.text;
        }
        return key;
    }

    public void ResetAll()
    {
        this.m_image = GetComponent<Image>();
        if (!m_isTutorial)
        {
            this.m_image.sprite = m_defaultImage;
            this.m_image.color = GameManager.s_instance.GetDefaultColor();
            this.textMeshProUGUI.text = "";
        }
    }

    public void DeleteCharInSquare()
    {
        textMeshProUGUI.text = "";
    }
    public void ChangeBackgroundColor(Constants.ImageBackground typ)
    {
        if (!m_isTutorial)
        {
            switch (typ)
            {
                case Constants.ImageBackground.RIGHT_COLOR:
                    this.m_image.color = GameManager.s_instance.GetRightColor();
                    break;
                case Constants.ImageBackground.WRONG_POSITION_COLOR:
                    this.m_image.color = GameManager.s_instance.GetWrongPositionColor();
                    break;
                case Constants.ImageBackground.WRONG_COLOR:
                    this.m_image.color = GameManager.s_instance.GetWrongColor();
                    break;
                default:
                    this.m_image.color = GameManager.s_instance.GetDefaultColor();
                    break;
            }
        }

    }

    public void AnimateWrongInput()
    {
        if (!m_isTutorial)
        {
            m_animator.SetTrigger(Constants.WRONG);
        }
    }

    public void AnimateRightInput()
    {
        if (!m_isTutorial)
        {
            m_animator.SetTrigger(Constants.RIGHT);
        }
    }

    public void AnimateInput()
    {
        if (!m_isTutorial)
        {
            m_animator.SetTrigger(Constants.INPUT);
        }
    }

    public void SetColor(Color color)
    {
        this.m_image.color = color;
    }

}
