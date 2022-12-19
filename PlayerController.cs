using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{


    [Header("Movement")]

    [Tooltip("Movement speed of the player.")]
    [SerializeField] private int _movementSpeed = 10;

    [Tooltip("Mouse sensitivity.")]
    [SerializeField] private float _mouseSensitivity = 300;

    [Tooltip("The amount of force the player has when pushing an object.")]
    [SerializeField] private float _pushingPower = 5;

    private CharacterController _controller;
    private float _xRotation = 0f;


    [Header("Berries")]

    [Tooltip("Point where the berry is released from when throwing.")]
    [SerializeField] private Transform _releasePoint;

    private List<string> _heldBerriesList = new List<string>(); 

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Throw"))
        {
            if (_heldBerriesList.Count != 0) 
            {
                ThrowBerry();
            }
        }

         Movement();

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3))
            {
                if (hit.collider.gameObject.tag == "Berry")
                {
                    PickBerry(hit.collider.gameObject.GetComponent<Berry>());
                }
            }
        }
    }

    /// <summary>
    /// Collision for the character controller to allow interaction with the container.
    /// </summary>
    /// <param name="hit"></param>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Container"))
        {
            Rigidbody containerRb = hit.collider.attachedRigidbody;
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            containerRb.velocity = pushDirection * _pushingPower;
        }
    }

    /// <summary>
    /// Gets movement and camera input and applys to the camera and character controller.
    /// </summary>
    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = transform.right * x + transform.forward * z;

        _controller.Move(movement * _movementSpeed * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    /// <summary>
    /// Throw the last picked berry.
    /// </summary>
    private void ThrowBerry()
    {
        if (_heldBerriesList.Count <= 0) return;

        string berryType = _heldBerriesList[_heldBerriesList.Count - 1];
        GameObject currentBerry = Instantiate(GameManager.gManager.BerryPrefabsDict[berryType], _releasePoint.position, transform.rotation);
        currentBerry.GetComponent<Berry>().Launch();
        _heldBerriesList.Remove(berryType);
    }

    /// <summary>
    /// Picks berry, adding it to the held berry list.
    /// </summary>
    /// <param name="berry">Berry to pick.</param>
    private void PickBerry(Berry berry)
    {
        _heldBerriesList.Add(berry.BerryType);
        berry.Pick();
    }

}
