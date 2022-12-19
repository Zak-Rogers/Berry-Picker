using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{


    private Container _container;

    [Header("Controls")]

    [Tooltip("Allows the Controls text to be toggled.")]
    [SerializeField] private GameObject _controlsText;


    [Header("Text")]

    [Tooltip("Timer display text UI.")]
    [SerializeField] private TMP_Text _timerText;

    [Tooltip("Score display text UI.")]
    [SerializeField] private TMP_Text _scoreText;

    [Tooltip("Distance to container display text UI.")]
    [SerializeField] private TMP_Text _distanceText;


    [Header("Required Berrys")]

    [Tooltip("Prefab for the required berry UI.")]
    [SerializeField] private GameObject _requiredBerryUIPrefab;

    [Tooltip("Array of the required berry UI positions.")]
    [SerializeField] private GameObject[] _requiredBerryPositions = new GameObject[8];

    private List<GameObject> _requiredBerriesUI = new List<GameObject>();
    private GameObject[] _berryInfo = new GameObject[2];
    private Dictionary<string, Sprite> _berryImagesDict = new Dictionary<string, Sprite>();

    private void Start()
    {
        if (GameManager.gManager.CurrentLevel > 1) _controlsText.SetActive(false);

        PopulateRequiredBerriesUI();
    }

    private void Update()
    {
        if (Input.GetButton("Cancel") && _controlsText.activeInHierarchy) _controlsText.SetActive(false);
        UpdateUIText();
    }

    /// <summary>
    /// Updates the Distance to the container, Time remaining and the score.
    /// </summary>
    private void UpdateUIText()
    {
        float distanceToContainer = (_container.transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude;

        _distanceText.text = Mathf.FloorToInt(distanceToContainer).ToString();
        _timerText.text = Mathf.FloorToInt(GameManager.gManager.RoundTimer).ToString();
        _scoreText.text = Mathf.FloorToInt(_container.LevelScore).ToString();
    }

    /// <summary>
    /// Adds the berry name and image to the UI for each berry in the required berries list.
    /// </summary>
    private void PopulateRequiredBerriesUI()
    {
        _container = GameManager.gManager.CurrentContainer;
        _container.RequiredBerryCollected += UpdateRequiredBerriesUI;
        _berryImagesDict = GameManager.gManager.BerryImagesDict;

        for (int i = 0; i < _container.RequiredBerries.Count; i++)
        {
            GameObject requiredBerry = Instantiate(_requiredBerryUIPrefab, _requiredBerryPositions[i].transform.position, Quaternion.identity, transform);
            string berry = _container.RequiredBerries[i];

            _requiredBerriesUI.Add(requiredBerry);

            _berryInfo[0] = requiredBerry.transform.GetChild(0).gameObject;
            _berryInfo[1] = requiredBerry.transform.GetChild(1).gameObject;

            _berryInfo[0].GetComponent<Image>().sprite = _berryImagesDict[berry];
            _berryInfo[1].GetComponent<TMP_Text>().text = berry;
        }
    }

    /// <summary>
    /// Function subscribed to the container's required berry collected event.
    /// </summary>
    private void UpdateRequiredBerriesUI(object sender, EventArgs e )
    {
        foreach(GameObject requiredBerryUI in _requiredBerriesUI)
        {
            _berryInfo[0] = requiredBerryUI.transform.GetChild(0).gameObject;
            _berryInfo[1] = requiredBerryUI.transform.GetChild(1).gameObject;

            _berryInfo[0].GetComponent<Image>().sprite = null;
            _berryInfo[0].GetComponent<Image>().color = Color.black;
            _berryInfo[1].GetComponent<TMP_Text>().text = null;
        }
        for (int i = 0; i < _container.RequiredBerries.Count; i++)
        {
            string berry = _container.RequiredBerries[i];
            _berryInfo[0] = _requiredBerriesUI[i].transform.GetChild(0).gameObject;
            _berryInfo[1] = _requiredBerriesUI[i].transform.GetChild(1).gameObject;

            _berryInfo[0].GetComponent<Image>().sprite = _berryImagesDict[berry];
            _berryInfo[0].GetComponent<Image>().color = Color.white;
            _berryInfo[1].GetComponent<TMP_Text>().text = berry;
        }
    }

    private void OnDestroy()
    {
        _container.RequiredBerryCollected -= UpdateRequiredBerriesUI;
    }

}
