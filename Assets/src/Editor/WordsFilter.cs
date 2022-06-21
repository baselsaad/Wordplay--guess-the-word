using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;


public class WordsFilter : EditorWindow
{
    #region Const-Static
    public static string DefaultSaveLocation;
    //static:
    public const string TITLE = "Filter Words by Letter";
    public const string READ_FILE_LABEL_TITLE = "Path to read from: ";
    public const string SAVE_LABEL_TITLE = "Path to save: ";
    public const int MIN_LETTERS_COUNT = 2;
    public const int MAX_LETTERS_COUNT = 7;
    public const int DO_NOT_CHECK_LETTERS_COUNT = 0;

    public const char COMMA_SPLITER = ',';
    public const char POINT_SPLITER = '.';
    public const char SPACE_SPLITER = ' ';


    #endregion

    #region Private
    private int m_chossenLettersCount = 2;
    private string m_inputPath = "Example: C:/wordlist.txt";
    private string m_outputPath = "Words/wordlist.txt";
    private int m_selectedOption = 0;
    private bool m_checkLettersCount = true;
    #endregion

    #region WindowsButton

    [MenuItem("Filter a text File/Filter")]
    public static void Constructur()
    {
        DefaultSaveLocation = Application.dataPath + "/Words/Word-List.txt";

        WordsFilter window = (WordsFilter)EditorWindow.GetWindow(typeof(WordsFilter));
        //Set Window title
        window.titleContent = new GUIContent(TITLE);
        Rect rect = new Rect();
        rect.position = new Rect().center;
        rect.size = new Vector2(600.0f, 600.0f);
        window.position = rect;

        window.Show();
    }

    [MenuItem("Filter a text File/Repair")]
    public static void Repair()
    {
        WordsFilter window = (WordsFilter)EditorWindow.GetWindow(typeof(WordsFilter));
        if (window != null)
        {
            window.Close();
            Debug.Log("repaired");
        }
    }

    #endregion

    #region Read and Write Files
    private string GetInputPath()
    {
        return EditorUtility.OpenFilePanel("Open a txt File", "", "txt");
    }

    private string GetOutputPath()
    {
        return EditorUtility.SaveFilePanel("Save file as Text"
            , ""
            , "WordListFile" + ".txt"
            , "txt");
    }

    private List<string> ReadFileAsStringList(string path, int lettersCount)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("Can't find the file, please make sure you select an existing .txt file");
            return null;
        }

        List<string> list = File.ReadAllLines(path).ToList<string>();
        List<string> sortedList = SplitWordsByNewLine(ref list, lettersCount);
        Debug.Log("SortedList= " + sortedList.Count());

        return sortedList;
    }

    private void WriteStringListAsFile(List<string> content, string path)
    {
        if (content == null)
        {
            Debug.LogError("The file could not be read successfully");
            return;
        }

        if (content.Count == 0)
        {
            Debug.LogError("The file does not contain any words");
            return;
        }

        try
        {
            content.Sort();
            // it will write a blank line at the end but we will hanlde that anyway when we load the Words into Array in GameManager
            File.WriteAllLinesAsync(path, content);
            Debug.Log("The Words have been successfully filtered, " + content.Count + " Words has been saved");
        }
        catch (System.IO.DirectoryNotFoundException e)
        {
            Debug.LogError("Can't find the path, please make sure you select the correct path to save");
            Debug.LogError(e.Message + "\n" + e.StackTrace);
        }
        catch (System.UnauthorizedAccessException e)
        {
            Debug.LogError("Can't find the path, please make sure you select the correct path to save " +
                "and name the new file  (the file has to have a .txt extension at the end)");
            Debug.LogError(e.Message + "\n" + e.StackTrace);
        }
    }

    private List<string> SplitWordsByNewLine(ref List<string> list, int lettersCount)
    {
        // Split the list by a new Line first then the list will be splitted by lettersCount 
        List<string> splitedList = new List<string>();

        // Split the List 
        for (int i = 0; i < list.Count; i++)
        {
            // Store the current line in Char Array 
            char[] currentLineCharArray = list[i].ToCharArray();
            FilterCurrentLine(ref currentLineCharArray, ref splitedList);
        }


        // Now add all words with the wanted letters count
        List<string> sortedList = new List<string>(splitedList.Count + 1);
        // check the lettersCount or not
        if (lettersCount != DO_NOT_CHECK_LETTERS_COUNT) //check
        {
            for (int i = 0; i < splitedList.Count; i++)
            {
                if (splitedList[i].Length == lettersCount && splitedList[i].Trim().Length > 0)
                {
                    sortedList.Add(splitedList[i].Trim().ToLower());
                }
            }
        }
        else //Do not check
        {
            for (int i = 0; i < splitedList.Count; i++)
            {
                if (splitedList[i].Length <= MAX_LETTERS_COUNT && splitedList[i].Trim().Length > 0)
                {
                    sortedList.Add(splitedList[i].Trim().ToLower());
                }
            }
        }

        return sortedList;
    }

    public void FilterCurrentLine(ref char[] currentLineCharArray, ref List<string> outSplitedList)
    {
        string tempWord = "";
        for (int j = 0; j < currentLineCharArray.Length; j++)
        {
            // check if the current Char is a (char)
            if ((currentLineCharArray[j] >= 'a' && currentLineCharArray[j] <= 'z')
                || (currentLineCharArray[j] >= 'A' && currentLineCharArray[j] <= 'Z'))

            {
                // Keep saving each character until we reach the split
                tempWord += currentLineCharArray[j];
            }
            else
            {
                outSplitedList.Add(tempWord.Trim().ToLower());
                tempWord = "";
            }
        }

        if (currentLineCharArray.Length > 2)
        {
            // Store the last word in list
            int lastIndex = currentLineCharArray.Length - 1;
            if ((currentLineCharArray[lastIndex] >= 'a' && currentLineCharArray[lastIndex] <= 'z')
                    || (currentLineCharArray[lastIndex] >= 'A' && currentLineCharArray[lastIndex] <= 'Z'))
            {
                outSplitedList.Add(tempWord.Trim().ToLower());
            }
        }
    }
    #endregion

    #region UI
    public void ShowLabelText()
    {
        // Font Style
        GUIStyle MyTextStyle = new GUIStyle();
        MyTextStyle.fontSize = 14;
        MyTextStyle.fontStyle = FontStyle.BoldAndItalic;
        MyTextStyle.normal.textColor = Color.white;

        //Text
        GUILayout.Label("The words in the file must be split line by line,\ni.e. each word on the new line" +
              ",\n\n if the words in your file were separated by a space or comma..,\n" +
              " you need to select the correct one in bar-box to filter the words correctly line by line\n\n", MyTextStyle);
    }

    public void InputButton()
    {
        m_inputPath = EditorGUILayout.TextField(READ_FILE_LABEL_TITLE, m_inputPath);
        if (GUILayout.Button("Chosse a text File"))
        {
            m_inputPath = GetInputPath();
        }
    }

    public void OutputButton()
    {
        DefaultSaveLocation = EditorGUILayout.TextField(SAVE_LABEL_TITLE, DefaultSaveLocation);
        if (GUILayout.Button("Chosse a path to save the file"))
        {
            m_outputPath = GetOutputPath();
            DefaultSaveLocation = m_outputPath;
        }
    }

    public void LettersCountSlider()
    {
        m_checkLettersCount = EditorGUILayout.Toggle("Letters Number", m_checkLettersCount);
        if (m_checkLettersCount)
        {
            m_chossenLettersCount = (int)EditorGUILayout.Slider("word character length: ", m_chossenLettersCount,
                                 MIN_LETTERS_COUNT, MAX_LETTERS_COUNT);
        }
        else
        {
            m_chossenLettersCount = DO_NOT_CHECK_LETTERS_COUNT;
        }
    }

    public void FilterButton()
    {
        if (GUILayout.Button("Filter"))
        {
            List<string> inputList = ReadFileAsStringList(m_inputPath, m_chossenLettersCount);
            if (inputList != null)
            {
                WriteStringListAsFile(inputList, m_outputPath);
            }
        }
    }

    #endregion

    void OnGUI()
    {
        ShowLabelText();
        InputButton();
        OutputButton();
        LettersCountSlider();
        FilterButton();
    }

}