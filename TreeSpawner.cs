using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{


    [Header("Prefabs")]

    [Tooltip("Array of tree prefabs.")]
    [SerializeField] private GameObject[] _treePrefabs = new GameObject[3];

    private List<Transform> _berryPositions_Hanging = new List<Transform>();
    private List<Transform> _berryPositions_Branching = new List<Transform>();
    private List<Transform> _berryPositions_Stalking = new List<Transform>();
    private Dictionary<string, GameObject> _berryPrefabsDict = new Dictionary<string, GameObject>();


    [Header("Amount")]

    [Tooltip("Number of trees to spawn.")]
    [SerializeField] private int _numOfTrees = 10;


    [Header("Spawning Area")]

    [Tooltip("Ground Object.")]
    [SerializeField] private GameObject _ground;

    private void Start()
    {
        _berryPrefabsDict = GameManager.gManager.BerryPrefabsDict;
        SpawnTrees();
        SpawnRequiredBerries();
        SpawnBerries();
    }


    /// <summary>
    /// Generates a random position for the tree to spawn based on the size of the ground.
    /// </summary>
    /// <returns>Vector3 position.</returns>
    private Vector3 GenerateRandomPosition()
    {
        
        float min = _ground.transform.localScale.x / 2 * -10;
        float max = _ground.transform.localScale.x / 2 * 10;
        float x = Random.Range(min, max);

        float y = _ground.transform.position.y + 3;

        min = _ground.transform.localScale.z / 2 * -10;
        max = _ground.transform.localScale.z / 2 * 10;
        float z = Random.Range(min, max);

        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Selects a random berry then checks the berry's variety to see if it can spawn at that position. If sucessfull berry is spawned and rotated as required. If unsucessfull no berry will spawn at that position.
    /// </summary>
    /// <param name="berryVariety">berryVariety to try to spawn.</param>
    /// <param name="position">Position to try and spawn a berry at.</param>
    private void TrySpawnRandomBerry (string berryVariety,Transform position)
    {
        int rand = Random.Range(0, GameManager.gManager.TypesOfBerries.Count - 1); 
        string berryType = GameManager.gManager.TypesOfBerries[rand];
        bool incorrectBerryVariety = _berryPrefabsDict[berryType].GetComponent<Berry>().BerryVariety != berryVariety;

        if (incorrectBerryVariety) return;

        GameObject berry = Instantiate(_berryPrefabsDict[berryType], position.position, Quaternion.identity, position);
        SetBerryRotation(berryType, berry);
    }

    /// <summary>
    /// Sets the local rotation of the berry to appear attached to the tree.
    /// </summary>
    /// <param name="berryType">BerryType of the berry being rotated.</param>
    /// <param name="berry">Berry that is being rotated.</param>
    private static void SetBerryRotation(string berryType, GameObject berry)
    {
        bool isJellyBerry = berryType == "JellyBerry";
        bool isRedBerry = berryType == "RedBerry";
        bool isNarNarBerry = berryType == "NarNarBerry";

        if (isJellyBerry || isRedBerry || isNarNarBerry)
        {
            berry.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            berry.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
    }

    /// <summary>
    /// Spawns the trees and adds their berry positions to the respective berry variety list.
    /// </summary>
    private void SpawnTrees()
    {
        for (int i = 0; i < _numOfTrees; i++)
        {
            int randomTree = Random.Range(0, _treePrefabs.Length);
            GameObject tree = Instantiate(_treePrefabs[randomTree], GenerateRandomPosition(), Quaternion.identity);
            Tree treeScript = tree.GetComponent<Tree>();
            treeScript.TreeSpawner = this;

            foreach (Transform position in treeScript.BerryPositions_Hanging)
            {
                _berryPositions_Hanging.Add(position);
            }

            foreach (Transform position in treeScript.BerryPositions_Branching)
            {
                _berryPositions_Branching.Add(position);
            }

            foreach (Transform position in treeScript.BerryPositions_Stalking)
            {
                _berryPositions_Stalking.Add(position);
            }
        }
    }

    /// <summary>
    /// Goes through the required berries ensuring each gets spawned into the level. Then removes the position from the lists to provent two berries spawning at one position.
    /// </summary>
    private void SpawnRequiredBerries()
    {
        int numOfRequireBerries = GameManager.gManager.CurrentContainer.RequiredBerries.Count;
        string berryType;
        string berryVariety;
        GameObject berry;
        List<string> requiredBerries = GameManager.gManager.CurrentContainer.RequiredBerries;
        for (int i = 0; i < numOfRequireBerries; i++)
        {
            berryType = requiredBerries[i]; 
            berryVariety = _berryPrefabsDict[berryType].GetComponent<Berry>().BerryVariety;

            switch (berryVariety)
            {
                case "Hanging":
                    berry = Instantiate(_berryPrefabsDict[berryType], _berryPositions_Hanging[i].position, Quaternion.identity, _berryPositions_Hanging[i]);
                    _berryPositions_Hanging.RemoveAt(i);
                    SetBerryRotation(berryType, berry);
                    break;

                case "Branching":
                    berry = Instantiate(_berryPrefabsDict[berryType], _berryPositions_Branching[i].position, Quaternion.identity, _berryPositions_Branching[i]);
                    SetBerryRotation(berryType, berry);
                    _berryPositions_Branching.RemoveAt(i);
                    break;

                case "Stalking":
                    berry = Instantiate(_berryPrefabsDict[berryType], _berryPositions_Stalking[i].position, Quaternion.identity, _berryPositions_Stalking[i]);
                    _berryPositions_Stalking.RemoveAt(i);
                    SetBerryRotation(berryType, berry);
                    break;
            }
        }
    }

    /// <summary>
    /// Tries to spawn berries at all the other positions.
    /// </summary>
    private void SpawnBerries()
    {
        foreach (Transform position in _berryPositions_Hanging)
        {
            TrySpawnRandomBerry("Hanging", position);
        }

        foreach (Transform position in _berryPositions_Branching)
        {
            TrySpawnRandomBerry("Branching", position);
        }

        foreach (Transform position in _berryPositions_Stalking)
        {
            TrySpawnRandomBerry("Stalking", position);
        }

        _berryPositions_Hanging.Clear();
        _berryPositions_Branching.Clear();
        _berryPositions_Stalking.Clear();
    }

    /// <summary>
    /// Public function to return a random position for when trees spawn colliding into each other.
    /// </summary>
    /// <returns>Vector3 </returns>
    public Vector3 GenerateNewPosition()
    {
        return GenerateRandomPosition();
    }

}
