using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverMenu : MonoBehaviour
{


    [Header("Scoring")]

    [Tooltip("Score display text UI.")]
    [SerializeField] private TMP_Text _scoreTxt;

    [Tooltip("Score heading text UI.")]
    [SerializeField] private TMP_Text _scoreHeadingTxt;
    

    [Header("Level Info")]

    [Tooltip("Level complete text UI.")]
    [SerializeField] private TMP_Text _levelCompleteTxt;

    [Tooltip("Level display text UI.")]
    [SerializeField] private TMP_Text _levelTxt;


    [Header("Buttons")]

    [Tooltip("Next Level Button.")]
    [SerializeField] private GameObject _nextLevelBtn;

    [Tooltip("Restart Level Button.")]
    [SerializeField] private GameObject _restartLevelBtn;

    [Tooltip("Quit Game Button.")]
    [SerializeField] private GameObject _quitBtn;


    [Header("Background")]

    [Tooltip("Background UI.")]
    [SerializeField] private GameObject _background;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CheckForLoss();
        CheckIfFinalLevel();

        _levelTxt.text = GameManager.gManager.CurrentLevel.ToString();
    }

    /// <summary>
    /// Checks if its the final level and updates the UI and sets the score to the total score, otherwise it sets the score to the level score.
    /// </summary>
    private void CheckIfFinalLevel()
    {
        if (GameManager.gManager.IsFinalLevel())
        {
            _scoreTxt.text = GameManager.gManager.TotalScore.ToString();
            _nextLevelBtn.SetActive(false);
            _quitBtn.SetActive(true);
            _levelCompleteTxt.text = "Congratulations you finished the game.";
            _scoreHeadingTxt.text = "Total Score:";
        }
        else
        {
            _scoreTxt.text = GameManager.gManager.LevelScore.ToString();
        }
    }

    /// <summary>
    /// Checks to see if the level was lost and updates the UI, otherwise ensures the reset button is not active and the next level button is.
    /// </summary>
    private void CheckForLoss()
    {
        if (!GameManager.gManager.Win)
        {
            _levelCompleteTxt.text = "Level Failed";
            _background.GetComponent<Image>().color = Color.red;
            _nextLevelBtn.SetActive(false);
            _restartLevelBtn.SetActive(true);
        }
        else
        {
            _nextLevelBtn.SetActive(true);
            _restartLevelBtn.SetActive(false);
        }
    }

    /// <summary>
    /// Function for the next level button. Advances the level, resets the level score and sets the game state to game.
    /// </summary>
    public void NextLevel()
    {
        GameManager.gManager.AdvanceLevel();
        GameManager.gManager.LevelScore = 0;
        GameManager.gManager.SetGameStateToGame();
    }

    /// <summary>
    /// Function for restarting the level. Loops through the picked berries list repopulating the required berries with ones previously picked.
    /// </summary>
    public void RestartLevel()
    {
        foreach (string berry in GameManager.gManager.CurrentContainer.PickedBerries)
        {
            GameManager.gManager.CurrentContainer.RequiredBerries.Add(berry);
        }
        GameManager.gManager.SetGameStateToGame();
    }

    /// <summary>
    /// Function for the main menu button, sets the game state to menu.
    /// </summary>
    public void MainMenu()
    {
        GameManager.gManager.SetGameStateToMenu();
    }

    /// <summary>
    /// Function for the quit button, quits the game.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
