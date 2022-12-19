using UnityEngine;

public class Menu : MonoBehaviour
{


    /// <summary>
    /// Function for the start button.
    /// </summary>
    public void StartGame()
    {
        GameManager.gManager.AdvanceLevel();
        GameManager.gManager.SetGameStateToGame();
    }

    /// <summary>
    /// Function for the quit button.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
