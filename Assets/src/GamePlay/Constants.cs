
public static class Constants
{
    #region Colors
    public const string RIGHT_COLOR_SQUARE = "🟩"; //right letter and postion
    public const string WRONG_POSITION_COLOR_SQUARE = "🟧"; //right letter but wrong postion
    public const string WRONG_COLOR_SQUARE = "🟥"; //wrong letter
    #endregion

    public const int WRONG_LETTER = 0;
    public const int WRONG_POSITION = 1;
    public const int RIGHT_POSITION = 2;

    #region Animation
    public const string WRONG = "Wrong";
    public const string RIGHT = "Right";
    public const string INPUT = "Input";
    #endregion

    //Audio
    public const string CLICK_SOUND = "Click";

    public const int CHAR_NOT_IN_WORD = -1;
    public const int WORD_NOT_IN_LIST = 0;
    public const char CHECKED_LETTER = ' ';
    public const int MAX_COLUMNS = 6;

    public enum ImageBackground
    {
        RIGHT_COLOR, WRONG_POSITION_COLOR, WRONG_COLOR, DEFAULT
    };

    public enum GameState
    {
        WON,
        WRONG_WORD,
        WORD_NOT_IN_LIST
    };

    public enum LetterState
    {
        LETTER_RIGHT_POSITION,
        LETTER_WRONG_POSITION,
        LETTER_NOT_IN_WORD
    };


}
