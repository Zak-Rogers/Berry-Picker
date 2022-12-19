using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider), typeof(MeshRenderer) )]

public class Berry : MonoBehaviour
{


    [Header("Respawn")]

    [Tooltip("Time till berry respawns on the tree.")]
    [SerializeField] private float _respawnTime = 10;

    private float _respawnTimer = 0;
    private MeshRenderer _meshRenderer;
    private Collider _berryCollider;


    [Header("Movement")]

    [Tooltip("Height at which the berry is thrown.")]
    [SerializeField] private float _archHeight = 5;

    private Rigidbody _rb;
    private const float _gravity = -9.8f;
    private float _heightDifference;
    private Transform _container;


    [Header("Damage")]

    [Tooltip("Hardness of the berry. The harder the fruit the more easily it damages.")]
    [Range(0.5f,1.0f)] [SerializeField] private float _hardness = 1;

    private float _damage = 0;

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }


    [Header("Type")]

    [Tooltip("The type of berry it is.")]
    [SerializeField] private string _berryType = "";

    [Tooltip("The variety of the berry. ( Branching, Stalking or Hanging")]
    [SerializeField] private string _berryVariety = "";

    public string BerryType 
    { 
        get { return _berryType; }
        set { _berryType = value; }
    }

    public string BerryVariety
    {
        get { return _berryVariety; }
        set { _berryVariety = value; }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _container = GameManager.gManager.CurrentContainer.transform;
        _meshRenderer = GetComponent<MeshRenderer>();
        _berryCollider = GetComponent<Collider>();
        _heightDifference = _container.position.y - transform.position.y;
    }

    private void Start()
    {
        _respawnTimer = _respawnTime;
    }
    
    private void Update()
    {
        if (_respawnTimer > 0)
        {
            _respawnTimer -= Time.deltaTime;
        }
        else
        {
            _meshRenderer.enabled = true;
            _berryCollider.enabled = true;
        }
    }

    /// <summary>
    /// Launchs the berry towards the container.
    /// </summary>
    public void Launch()
    {
        _rb.useGravity = true;
        Vector3 directionXZ = new Vector3(_container.position.x - transform.position.x, 0, _container.position.z - transform.position.z);
        _damage = directionXZ.magnitude * _hardness;

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * _gravity * _archHeight);
        Vector3 velocityXZ = directionXZ / (Mathf.Sqrt(-2 * _archHeight / _gravity) + Mathf.Sqrt(2 * (_heightDifference - _archHeight) / _gravity));

        _rb.velocity = velocityY + velocityXZ;
    }

    /// <summary>
    /// Pick the berry, Hides the berry on the tree till it respawns.
    /// </summary>
    public void Pick()
    {
        _meshRenderer.enabled = false;
        _berryCollider.enabled = false;
        _respawnTimer = _respawnTime;
    }

}
