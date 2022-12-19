using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public static GameManager gManager;

    private enum GameState { MENU, GAME, GAMEOVER}
    private GameState currentState;
    private bool _win = false;

    public bool Win
    {
        get { return _win; }
        set { _win = value; }
    }


    [Header("Timer")]

    [Tooltip("Round Duration.")]
    [SerializeField] private float _roundDuration = 120;

    private float _roundTimer = 0;

    public float RoundTimer
    {
        get { return _roundTimer; }
    }


    [Header("Levels")]

    [Tooltip("Number of Levels.")]
    [SerializeField] private int _numberOfLevels = 24;

    private int _currentLevel;
    private Dictionary<int, List<string>> _requiredBerriesDict = new Dictionary<int, List<string>>();
    
    public int CurrentLevel
    {
        get { return _currentLevel; }
        set { _currentLevel = value; }
    }


    [Header("Scoring")]
    private float _totalScore = 0;
    private float _levelScore = 0;
    private Container _currentContainer;

    public float TotalScore
    {
        get {return _totalScore; }
        set { _totalScore = value; } 
    }

    public float LevelScore
    {
        get { return _levelScore; }
        set { _levelScore = value; }
    }

    public Container CurrentContainer 
    {
        get { return _currentContainer; }
        
        set 
        {
            _currentContainer = value;
            _currentContainer.OnListCompleted += ListCompleted;
            SetRequiredBerries();
        }
    }


    [Header("Berry Prefab Dictionary")]

    [Tooltip("List of berry types.")]
    [SerializeField] private List<string> _typesOfBerriesK;

    [Tooltip("List of berry sprites, needs to match the order of the berry types.")]
    [SerializeField] private List<Sprite> _berryImagesV;

    [Tooltip("List of berry prefabs, needs to match the order of the berry types.")]
    [SerializeField] private List<GameObject> _berryPrefabsV;

    private Dictionary<string, GameObject> _berryPrefabsDict = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> _berryImagesDict = new Dictionary<string, Sprite>();

    public Dictionary<string, Sprite> BerryImagesDict
    {
        get { return _berryImagesDict; }
    }

    public Dictionary<string, GameObject> BerryPrefabsDict
    {
        get { return _berryPrefabsDict; }
    }

    public List<string> TypesOfBerries 
    {
        get { return _typesOfBerriesK; }
    }

    private void Awake()
    {
        if(gManager == null)
        {
            gManager = this;
            DontDestroyOnLoad(this);
            SetRequiredBerriesDict();
        }
    }

    private void Start()
    {
        _currentLevel = 0;

        PopulateDictionaries();
    }

    private void Update()
    {
        if (currentState != GameState.GAME) return;
        if (_roundTimer >= 0)
        {
            _roundTimer -= Time.deltaTime;
        }
        else
        {
            _win = false;
            SetGameStateToGameOver();
        }
    }

    /// <summary>
    /// Function subscribed to the containers OnListCompleted event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListCompleted(object sender, EventArgs e)
    {
        _win = true;
        SetGameStateToGameOver();
    }

    /// <summary>
    /// Returns a random BerryType from the avalible types of berries.
    /// </summary>
    /// <returns>string randBerryType</returns>
    private string RandBerryType()
    {
        return _typesOfBerriesK[UnityEngine.Random.Range(0, _typesOfBerriesK.Count - 1)];
    }

    /// <summary>
    /// Populates the Required berries lists with random berries for each level and adds to the required berries dictionary with the level as the key.
    /// </summary>
    private void SetRequiredBerriesDict()
    {
        for (int i = 1; i <= _numberOfLevels; i++)
        {
            if( i >= 1 && i <= 3)
            {
                _requiredBerriesDict[i] = new List<string> {
                    RandBerryType()
                };
            }
            else if (i>=4 && i <=6)
            {
                _requiredBerriesDict[i] = new List<string> {
                    RandBerryType(), RandBerryType() };
            }
            else if (i >= 7 && i <= 9)
            {
                _requiredBerriesDict[i] = new List<string> {
                    RandBerryType(),RandBerryType(),
                    RandBerryType() };
            }
            else if (i >= 10 && i <= 12)
            {
                _requiredBerriesDict[i] = new List<string> {
                    RandBerryType(), RandBerryType(),
                    RandBerryType(), RandBerryType() };
            }
            else if (i >= 13 && i <= 15)
            {
                _requiredBerriesDict[i] = new List<string> {
                    RandBerryType(), RandBerryType(),
                    RandBerryType(), RandBerryType(),
                    RandBerryType() };
            }
            else if (i >= 16 && i <= 18)
            {
                _requiredBerriesDict[i] = new List<string> {
                    RandBerryType(), RandBerryType(),
                    RandBerryType(), RandBerryType(),
                    RandBerryType(), RandBerryType() };
            }
            else if (i >= 19 && i <= 21)
            {
                _requiredBerriesDict[i] = new List<string> {
                    RandBerryType(), RandBerryType(),
                    RandBerryType(), RandBerryType(),
                    RandBerryType(), RandBerryType(),
                    RandBerryType()};
            }
            else if (i >= 22 && i <= _numberOfLevels) 
            {
                _requiredBerriesDict[i] = new List<string> {
                    RandBerryType(), RandBerryType(),
                    RandBerryType(), RandBerryType(),
                    RandBerryType(), RandBerryType(),
                    RandBerryType(), RandBerryType() };
            }
        }
    }

    /// <summary>
    /// Updates the game state to game.
    /// </summary>
    public void SetGameStateToGame()
    {
        currentState = GameState.GAME;
        SceneManager.LoadScene(1);
        _roundTimer = _roundDuration;
    }

    /// <summary>
    /// Updates the game state to game over. Unsubscribes ListCompleted function from the container's OnListCompleted event. Loads the game over scene.
    /// </summary>
    public void SetGameStateToGameOver()
    {
        currentState = GameState.GAMEOVER;
        _currentContainer.OnListCompleted -= ListCompleted;
        SceneManager.LoadScene(2); 
    }

    /// <summary>
    /// Updates the game state to Menu. resets the level to 0 and loads the Menu scene.
    /// </summary>
    public void SetGameStateToMenu()
    {
        currentState = GameState.MENU;
        SceneManager.LoadScene(0);
        _currentLevel = 0;
    }

    /// <summary>
    /// Sets the levels container's required berries based of the dictionary produced 
    /// </summary>
    public void SetRequiredBerries()
    {
        _currentContainer.RequiredBerries = _requiredBerriesDict[_currentLevel];
    }

    /// <summary>
    /// Increments the level by 1.
    /// </summary>
    public void AdvanceLevel()
    {
        _currentLevel++;
    }

    /// <summary>
    /// Checks if the current level is the final level.
    /// </summary>
    /// <returns>True if the current level is the final level.</returns>
    public bool IsFinalLevel()
    {
        bool isFinalLevel = false;

        if(_currentLevel == _numberOfLevels) isFinalLevel = true;
        return isFinalLevel;
    }

    /// <summary>
    /// Populates the Berry prefabs dictionary and berry images dictionary with the berry type (key) and respective image (value) or prefab (value) using the string, sprite and gameobject lists.
    /// </summary>
    private void PopulateDictionaries()
    {
        int index = 0;
        foreach (string name in _typesOfBerriesK)
        {
            _berryImagesDict.Add(name, _berryImagesV[index]);
            _berryPrefabsDict.Add(name, _berryPrefabsV[index]);
            index++;
        }
    }

}
