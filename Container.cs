using System;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{


    [Header("Scoring")]

    [Tooltip("Base score for each berry. Added for required berry, deducted for unwanted berry.")] 
    [SerializeField] private int _baseScore = 100 ;

    public event EventHandler OnListCompleted;
    public event EventHandler RequiredBerryCollected;
    private float _levelScore;

    public float LevelScore
    {
        get { return _levelScore; }
    }


    [Header("Berry Lists")]
    private List<string> _pickedBerries = new List<string>();
    private List<string> _requiredBerries = new List<string>();

    public List<string> PickedBerries 
    { 
        get { return _pickedBerries; }
        set { _pickedBerries = value; }
    }

    public List<string> RequiredBerries 
    {
        get { return _requiredBerries; }
        set { _requiredBerries = value; }
    }

    private void Awake()
    {
        GameManager.gManager.CurrentContainer = this;
    }

    private void Update()
    {
        bool collectedAllBerries = (_requiredBerries.Count == 0); 
        if (collectedAllBerries && OnListCompleted != null)
        {
            ListComplete();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool otherIsBerry = other.tag == "Berry";
        if (otherIsBerry)
        {
            Berry berry = other.GetComponent<Berry>();
            CollectBerry(berry);
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// Checks whether the berry is in the required berries list and calculates score. If it is a required berry, adds to the picked berries and removes from required berries list.
    /// </summary>
    /// <param name="berry">Berry to be picked.</param>
    private void CollectBerry(Berry berry)
    {
        bool isRequiredBerry = _requiredBerries.Contains(berry.BerryType);
        if (isRequiredBerry)
        {
            _requiredBerries.Remove(berry.BerryType);
            RequiredBerryCollected(this, EventArgs.Empty);
            _pickedBerries.Add(berry.BerryType);
            CalculateScore(berry.Damage);
        }
        else
        {
            _levelScore -= _baseScore;
        }
    }

    /// <summary>
    /// Adds levelScore to the TotalScore and passes the levelScore to the GameManager. Finally triggering the OnListCompleted event.
    /// </summary>
    private void ListComplete()
    {
        _levelScore += GameManager.gManager.RoundTimer * 10;
        _levelScore = Mathf.FloorToInt(_levelScore);
        GameManager.gManager.TotalScore += _levelScore;
        GameManager.gManager.LevelScore = _levelScore;
        OnListCompleted(this, EventArgs.Empty);
    }

    /// <summary>
    /// Calculates the score for a correct berry using the baseScore and damage taken. Then adds to the levelScore.
    /// </summary>
    /// <param name="damage">Damage recieved during the throw.</param>
    private void CalculateScore(float damage)
    {
        float score = _baseScore - damage;
        _levelScore += score;
    }

}
