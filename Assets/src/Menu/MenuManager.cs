using TMPro;
using UnityEngine;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    #region Singleton
    public static MenuManager s_instance;

    private void Awake()
    {
        s_instance = this;
    }
    #endregion

    //private:
    private string m_textToCopy;

    //Objects:
    [SerializeField] private GameObject m_startMenu, m_gameOverMenu, m_game;
    [SerializeField] private GameObject m_settingsWindow;
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private GameObject m_letterChoicePanel;
    [SerializeField] private GameObject m_gameViewPanel;

    // UI
    [SerializeField] private TextMeshProUGUI m_secretWord;
    [SerializeField] private TMPro.TextMeshProUGUI m_gameTitle;
    [SerializeField] private TMPro.TextMeshProUGUI m_letterCountChoice;

    private void Start()
    {
        m_gameTitle.SetText(GameManager.s_instance.GetTitle());
    }


    public void PlayButton()
    {
        m_game.SetActive(true);
        m_letterChoicePanel.SetActive(true);

        m_startMenu.SetActive(false);
        m_gameOverMenu.SetActive(false);
        m_gameViewPanel.SetActive(false);
    }

    public void StartButton()
    {
        m_game.SetActive(true); // is true any way
        m_gameViewPanel.SetActive(true);

        m_letterChoicePanel.SetActive(false);

        int lettersCount = int.Parse(m_letterCountChoice.text);// Sure that the text will be a Number
       
        
        m_gameManager.SetLettersCount(lettersCount);
        m_gameManager.StartGame();
    }

    public void GameOver()
    {
        m_game.SetActive(false);
        ShowGameOverMenu();
    }

    public void ShowSecretWord(string word)
    {
        m_secretWord.text = "The Word is: " + word.ToUpper();
    }
    public void ShowStartMenu()
    {
        m_gameManager.ResetAllGameData();
        m_game.SetActive(false);
        m_startMenu.SetActive(true);
        m_gameOverMenu.SetActive(false);
    }

    public void ShowSettingsMenu()
    {
        m_startMenu.SetActive(false);

        m_gameOverMenu.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        m_startMenu.SetActive(false);
        m_gameOverMenu.SetActive(true);
    }

    public void ShareButton()
    {
        GUIUtility.systemCopyBuffer = this.m_textToCopy; //Constants.RED_SQUARE; // copy the text
        string tempText = m_secretWord.text;
        StartCoroutine(PrintTextForSeconds(tempText, "Copied results to Clipboard", 2.0f));
    }

    public IEnumerator PrintTextForSeconds(string orginalText, string text, float waitTime)
    {
        m_secretWord.text = text;
        yield return new WaitForSeconds(waitTime);
        m_secretWord.text = orginalText;
    }

    public void SetTextToCopy(string pattern)
    {
        this.m_textToCopy = pattern;
    }

    public void ShowSettingsWindow()
    {
        m_settingsWindow.SetActive(true);
    }

    public void ResumeButtonSetting()
    {
        m_settingsWindow.SetActive(false);
    }

    public void QuitButtonSetting()
    {
        m_gameManager.ResetAllGameData();
        m_startMenu.SetActive(true);

        m_game.SetActive(false);
        m_gameOverMenu.SetActive(false);
        m_settingsWindow.SetActive(false);
    }
}
