using UnityEngine;
using TMPro;

public class KeyboardGameManager : MonoBehaviour
{
    //public:
    public static KeyboardGameManager Instance;

    //private:
    private string textBox;

    private void Start()
    {
        Instance = this;
        textBox = "";
    }

    public void deleteLetter()
    {
        if (textBox.Length != 0)
        {
            textBox = textBox.Remove(textBox.Length - 1);
        }
    }

    public void addLetter(string letter)
    {
        if (GameManager.s_instance != null)
        {
            GameManager.s_instance.AddKey(letter);
        }
        
    }

    public void submitWord()
    {
        textBox = "";
        Debug.Log("Text submitted successfully!");
    }

    public string getTextBox()
    {
        return this.textBox;
    }
}
