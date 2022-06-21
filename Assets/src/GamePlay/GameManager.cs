using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public static GameManager s_instance;

    private void Awake()
    {
        s_instance = this;
    }


    [Header("Fields")]
    [SerializeField] private GameObject[] m_squares;

    [Header("Colors")]
    [SerializeField] private Color m_wrongColor;
    [SerializeField] private Color m_defaultColor;
    [SerializeField] private Color m_rightColor;
    [SerializeField] private Color m_wrongPosition;

    [Header("WordList Class"), Tooltip("Here is The WordList Object, you add the .text file is in WordList")]
    [SerializeField] private Wordlist m_worlist;

    [Header("Debug-Tools")]
    [SerializeField] private TMPro.TextMeshProUGUI m_debug;

    [Header("Game-Title")]
    [SerializeField] private string m_title;

    private string m_chossenWord = "Random";
    private List<int> m_lettersTracker;
    private string m_currentGuess = ""; // Alle Letters will be Trim() and toLower()
    private int m_currentRowIndex;
    private bool m_isGameCreated = false;
    private Square[] m_currentSquareRow;
    private KeyboardButtonController[] m_keyboardButtonController;
    private int m_lettersCount;

    public void StartGame()
    {
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        timer.Start();

        CreateGame();

        timer.Stop();
        // Debug.Log("Game Created in: " + timer.ElapsedMilliseconds + " ms");
        m_debug.text = "Game Created in: " + timer.ElapsedMilliseconds + " ms";
    }

    public void CreateGame()
    {
        // initialization
        m_keyboardButtonController = FindObjectsOfType<KeyboardButtonController>();
        ResetAllGameData();

        m_chossenWord = m_worlist.GetRandomWord(GetLettersCount()); // the word is allready trim() and toLower()
        Debug.Log("The Word is " + m_chossenWord);

        FetchRows();

        m_isGameCreated = true;
        m_currentRowIndex = 0;
        m_currentSquareRow = m_squares[m_currentRowIndex].GetComponentsInChildren<Square>();//first row at 0 in Array
        m_lettersTracker = new List<int>();
    }

    private void FetchRows()
    {
        Square[] inputsFields = FindObjectsOfType<Square>();
        foreach (Square sqaure in inputsFields)
        {
            //Loop through all fields and clear all fields, their position bigger than the word length
            //each field has its own attribute to determine its position in the line
            if (sqaure.GetRowPosition() >= m_worlist.GetChossenWordLength())
            {
                sqaure.DisableGameObject();
            }
        }
    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey)
        {
            if (Input.GetKeyDown(e.keyCode))
            {

                if (e.keyCode >= KeyCode.A && e.keyCode <= KeyCode.Z)
                {
                    string key = e.keyCode.ToString().ToUpper();
                    AddKey(key);
                }
                else if (e.keyCode == KeyCode.Backspace)
                {
                    DeleteLastLetter();
                }
                else if (e.keyCode == KeyCode.Return)
                {
                    EnterKeyPressed();
                }

            }
        }


    }
#endif


    public void EnterKeyPressed()
    {
        if (m_isGameCreated)
        {
            // if the user pressed Enter but the word has less letters
            // than the random word that the game generated, then run an animation
            if (this.m_currentGuess.Length < m_worlist.GetChossenWordLength())
            {
                RunWrongAnimation();
            }
            else
            {
                System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
                timer.Start();

                CheckInput();

                timer.Stop();
                m_debug.text = "Time for Checking: " + timer.ElapsedMilliseconds + " ms";
            }
        }
    }

    /************************************************************************/
    /* Get letter from Keyboard and set it to the right Square              */
    /************************************************************************/
    public void AddKey(string key)
    {
        if (key == null || key.Length == 0)
        {
            return;
        }

        if (m_currentGuess.Length < m_worlist.GetChossenWordLength())
        {
            m_currentSquareRow[m_currentGuess.Length].AnimateInput();// Simple Animation for input
            m_currentSquareRow[m_currentGuess.Length].SetCharInSquare(key);// Set the char in Square
            m_currentGuess += key.ToLower().Trim(); // Add the pressed letter to m_currentGuess, easier to compare and check later
        }
    }


    public void DeleteLastLetter()
    {
        if (this.m_currentGuess.Length > 0)
        {
            m_currentSquareRow[m_currentGuess.Length - 1].DeleteCharInSquare();
            m_currentGuess = this.m_currentGuess.Remove(this.m_currentGuess.Length - 1);
        }
    }


    private void CheckInput()
    {
        Constants.GameState state = CheckIfWordInList();
        switch (state)
        {
            case Constants.GameState.WON: //the input-word is the random word
                Win();
                break;
            case Constants.GameState.WRONG_WORD: // the word is in list but its wrong
                WordIsWrong();
                break;
            case Constants.GameState.WORD_NOT_IN_LIST: // the word is not even the list
                WordNotInList();
                break;
        }
    }

    private Constants.GameState CheckIfWordInList()
    {
        if (m_currentGuess.Equals(m_chossenWord))
        {
            return Constants.GameState.WON;
        }
        else if (m_worlist.GetWordList().BinarySearch(m_currentGuess) >= Constants.WORD_NOT_IN_LIST)
        {
            // The Word is in List => let's see how many chars are in the Random Word and set the color for each char
            return Constants.GameState.WRONG_WORD;
        }

        //the word is not in list
        return Constants.GameState.WORD_NOT_IN_LIST;
    }


    private void Win()
    {
        m_currentRowIndex++;
        CheckCurrentWord();
        StartCoroutine(GameOver());
    }

    private void WordIsWrong()
    {
        CheckCurrentWord();
        m_currentRowIndex++;
        if (!CheckGameOver())
        {
            m_currentGuess = "";
            m_currentSquareRow = m_squares[m_currentRowIndex].GetComponentsInChildren<Square>();
        }
    }

    private void WordNotInList()
    {
        RunWrongAnimation();
    }


    private void CheckCurrentWord()//Check Current Words by reading each letter and add the right state for each char
    {
        char[] chossenWord = m_chossenWord.ToCharArray();
        char[] guess = m_currentGuess.ToCharArray();

        SetCorrectChars(guess, chossenWord);
        SetWrongChars(guess, chossenWord);
    }

    private void SetCorrectChars(char[] guess, char[] chossenWord)
    {
        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == chossenWord[i])
            {
                //Animate 
                StartCoroutine(RunRightAnimation(i));
                
                // Remove the char from the copy of the chossenWord and the guess to avoid Collisions and count the char twice
                chossenWord[i] = Constants.CHECKED_LETTER;
                guess[i] = Constants.CHECKED_LETTER;
            }
        }
    }

    private void SetWrongChars(char[] guess, char[] chossenWord)
    {
        for (int guessPointer = 0; guessPointer < guess.Length; guessPointer++)
            CheckCurrentLetter(guess, chossenWord, ref guessPointer);
    }

    private void CheckCurrentLetter(char[] guess, char[] chossenWord, ref int guessPointer)
    {
        //if the char we check not deleted, then check it using IsCharInWord, else go to the next
        if (guess[guessPointer] == Constants.CHECKED_LETTER)
        {
            SetCharState(Constants.LetterState.LETTER_RIGHT_POSITION, ref guessPointer);
            return;
        }
           

        // IsCharInWord will get the index of the wanted char, but the index is relative to the chossenWord
        int chossendWordPointer = IsCharInWord(chossenWord, ref guess[guessPointer]);

        // Animate
        StartCoroutine(RunRightAnimation(guessPointer));

        if (chossendWordPointer == Constants.CHAR_NOT_IN_WORD)
        {
            SetCharState(Constants.LetterState.LETTER_NOT_IN_WORD, ref guessPointer);
        }
        else
        {
            SetCharState(Constants.LetterState.LETTER_WRONG_POSITION, ref guessPointer);
            // Remove the char from the copy of the chossenWord and the guess to avoid Collisions and count the char twice
            chossenWord[chossendWordPointer] = Constants.CHECKED_LETTER;
        }

    }


    private int IsCharInWord(char[] word, ref char b)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == b)
            {
                return i;
            }
        }

        return Constants.CHAR_NOT_IN_WORD;
    }

    private void SetCharState(Constants.LetterState state, ref int indexOfChar)
    {
        switch (state)
        {
            // Right Letter => Right_Color 
            case Constants.LetterState.LETTER_RIGHT_POSITION:
                m_lettersTracker.Add(Constants.RIGHT_POSITION); // to Copy the results later
                m_currentSquareRow[indexOfChar].ChangeBackgroundColor(Constants.ImageBackground.RIGHT_COLOR); // Change Squares Color
                ChangeColorOfKey(m_currentSquareRow[indexOfChar].GetKeyText(), GetRightColor()); // Change color of the keyboard-keys
                break;
            // Wrong Letter Position => Wrong_Color_Position
            case Constants.LetterState.LETTER_WRONG_POSITION:
                m_lettersTracker.Add(Constants.WRONG_POSITION);
                m_currentSquareRow[indexOfChar].ChangeBackgroundColor(Constants.ImageBackground.WRONG_POSITION_COLOR);
                ChangeColorOfKey(m_currentSquareRow[indexOfChar].GetKeyText(), GetWrongPositionColor());
                break;
            // Wrong Letter => Wrong_Color
            case Constants.LetterState.LETTER_NOT_IN_WORD:
                m_lettersTracker.Add(Constants.WRONG_LETTER);
                m_currentSquareRow[indexOfChar].ChangeBackgroundColor(Constants.ImageBackground.WRONG_COLOR);
                ChangeColorOfKey(m_currentSquareRow[indexOfChar].GetKeyText(), GetWrongColor());
                break;
        }
    }


    private bool CheckGameOver()
    {
        if (m_currentRowIndex >= m_squares.Length)
        {
            StartCoroutine(GameOver());
            return true;
        }

        return false;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2.0f);
        CopyTheResult();
        ResetAllGameData();
        MenuManager.s_instance.ShowSecretWord(this.m_chossenWord);
        MenuManager.s_instance.GameOver();

        foreach (Square a in Resources.FindObjectsOfTypeAll(typeof(Square)))
        {
            a.ShowGameObject();
        }

        this.m_isGameCreated = false;
        this.m_currentRowIndex = 0;
    }


    public void ResetAllGameData()
    {
        m_lettersTracker = null;
        this.m_isGameCreated = false;
        this.m_currentRowIndex = 0;
        this.m_currentGuess = "";
        ResetAllKeysColor();

        Square[] inputFields = Resources.FindObjectsOfTypeAll<Square>();
        foreach (Square a in inputFields)
        {
            if (a.gameObject.activeSelf)
            {
                a.ResetAll();
            }
            else
            {
                a.gameObject.SetActive(true);
                a.ResetAll();
            }
        }
    }

    // Convert all the result to string to copy it and the user can share it on social media
    private void CopyTheResult()
    {
        string symbol = "";
        string textToCopy = m_title + "\n" + m_currentRowIndex + "/6\n\n";

        int index = 0;
        foreach (int a in m_lettersTracker)
        {
            switch (a)
            {
                case Constants.RIGHT_POSITION:
                    symbol += Constants.RIGHT_COLOR_SQUARE;
                    break;
                case Constants.WRONG_POSITION:
                    symbol += Constants.WRONG_POSITION_COLOR_SQUARE;
                    break;
                case Constants.WRONG_LETTER:
                    symbol += Constants.WRONG_COLOR_SQUARE;
                    break;
            }

            if ((index % m_worlist.GetChossenWordLength()) == 0 && index > 0)
            {
                textToCopy += "\n" + symbol;
                symbol = "";
            }
            else
            {
                textToCopy += symbol;
                symbol = "";
            }
            index++;
        }

        MenuManager.s_instance.SetTextToCopy(textToCopy); // copy the text
    }

    private void ResetAllKeysColor()
    {
        foreach (KeyboardButtonController k in m_keyboardButtonController)
        {
            k.SetDefaultColor();
        }
    }

    private void RunWrongAnimation()
    {
        for (int i = 0; i < m_currentSquareRow.Length; i++)
        {
            m_currentSquareRow[i].AnimateWrongInput();
        }
    }

    private IEnumerator RunRightAnimation(int i)
    {
        m_currentSquareRow[i].AnimateRightInput();
        yield return new WaitForSeconds(0.02f);
    }

    private void ChangeColorOfKey(string key, Color color)
    {
        foreach (KeyboardButtonController k in m_keyboardButtonController)
        {
            if (k.GetChar().Equals(key.ToLower().Trim()))
            {
                color.a = 1f;
                k.SetColor(color);
            }
        }
    }


    public Color GetDefaultColor()
    {
        return m_defaultColor;
    }

    public Color GetWrongColor()
    {
        return m_wrongColor;
    }

    public Color GetRightColor()
    {
        return m_rightColor;
    }

    public Color GetWrongPositionColor()
    {
        return m_wrongPosition;
    }

    public string GetTitle()
    {
        return m_title;
    }

    public void SetLettersCount(int i)
    {
        m_lettersCount = i;
    }

    public int GetLettersCount()
    {
        return m_lettersCount;
    }


}
