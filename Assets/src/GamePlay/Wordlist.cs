using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;

public class Wordlist : MonoBehaviour
{
    //private:
    private string m_word;
    private int m_wordsLength;
    private List<string>[] m_wordLists;
    private int m_wordListIndex;

    //Serialize:
    [SerializeField] private TextAsset[] m_dictionaryTextFile;
    [SerializeField] private TMPro.TextMeshProUGUI m_timer;


    /************************************************************************/
    /* Load the WordList into Memory at the beginning                       */
    /************************************************************************/
    private void Start()
    {
        m_wordLists = new List<string>[4]; // They are only 4 word lists
        for (int i = 0; i < m_wordLists.Length; i++)
            LoadWords(m_dictionaryTextFile[i], ref m_wordLists[i]);
    }

    /************************************************************************/
    /* @return random Word from the word list and set the word length       */
    /************************************************************************/
    public string GetRandomWord(int i)
    {
        m_wordListIndex = i - 3;
        m_word = GenerateRandomWord();
        m_wordsLength = m_word.Length;

        return m_word;
    }

    private void LoadWords(TextAsset textFilePath, ref List<string> outWordList)
    {
        if (outWordList != null)
        {
            return;
        }

        outWordList = new List<string>();
        StringReader wordListFile = new StringReader(textFilePath.text);

        string line = "";
        while ((line = wordListFile.ReadLine()) != null)
        {
            if (line.Trim().Length > 0)
                outWordList.Add(line);
        }

        wordListFile.Close();
    }

    /************************************************************************/
    /* @return a Random Word from the word list                             */
    /************************************************************************/
    private string GenerateRandomWord()
    {
        // lettersCount - 3 because so the word list with 3-letters will be at 0 in Array and 4 at 1 and so on 
        List<string> wordList = m_wordLists[m_wordListIndex];
        int randomNumber = UnityEngine.Random.Range(0, wordList.Count - 1);
        string randomWord = wordList[randomNumber].ToString().ToLower().Trim();

        return randomWord;
    }


    public string GetChossenWord()
    {
        return m_word;
    }

    public int GetChossenWordLength()
    {
        return m_wordsLength;
    }

    public ref List<string> GetWordList()
    {
        return ref m_wordLists[m_wordListIndex];
    }


}
