using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Tree : MonoBehaviour
{


    private List<Transform> _berryPositions_Hanging = new List<Transform>();
    private List<Transform> _berryPositions_Branching = new List<Transform>();
    private List<Transform> _berryPositions_Stalking = new List<Transform>();
    private TreeSpawner _treeSpawner;


    public List<Transform>BerryPositions_Hanging
    {
        get { return _berryPositions_Hanging; }
    }

    public List<Transform>BerryPositions_Branching
    {
        get { return _berryPositions_Branching; }
    }

    public List<Transform>BerryPositions_Stalking
    {
        get { return _berryPositions_Stalking; }
    }

    public TreeSpawner TreeSpawner
    {
        set { _treeSpawner = value; }
    }

    private void Awake()
    {
        PopulateBerryPositionLists();
    }

    /// <summary>
    /// Checks each child object to see if it is a berry position and add it to the respective list.
    /// </summary>
    private void PopulateBerryPositionLists()
    {
        int numOfChildren = gameObject.transform.childCount;
        for (int i = 0; i < numOfChildren; i++)
        {
            Transform child = gameObject.transform.GetChild(i);

            switch (child.tag)
            {
                case "BerryPosition_Hanging":
                    _berryPositions_Hanging.Add(child);
                    break;
                case "BerryPosition_Branching":
                    _berryPositions_Branching.Add(child);
                    break;
                case "BerryPosition_Stalking":
                    _berryPositions_Stalking.Add(child);
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Tree") || collision.collider.CompareTag("Wall"))
        {
            transform.position = _treeSpawner.GenerateNewPosition();
        } 
    }

}
