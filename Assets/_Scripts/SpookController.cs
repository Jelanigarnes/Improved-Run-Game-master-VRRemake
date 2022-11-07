using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SpookController : MonoBehaviour {

    //PRIVATE
    private NavMeshAgent navMeshAgent;
    private GameObject _gameControllerObject;
    private GameController _gameController;
    private GameObject _spawnPoint;
    private GameObject _player;
    private bool _goingup;
    private float _speed;
    private Transform _transform;
    private Animator _animator;
    public int _currentPoint;
    private float _currentDistance;
    private float _distanceFromPoint;
    private Ray _ray;
    private RaycastHit _hit;
    private float _maxDistanceToCheck = 6.0f;
    private Vector3 _checkDirection;

    //PUBLIC
    public GameObject[] PatrolPoints;

    // PUBLIC PROPERTIES+++++++++++++++++++++++++++++++++
    public float Speed
    {
        get
        {
            return this._speed;
        }
        set
        {
            this._speed = value;
        }
    }



    // Use this for initialization
    void Start () {
        Initialize();
        _goingup = true;
    }
    void Initialize()
    {
        this._animator = gameObject.GetComponent<Animator>();
        this.navMeshAgent = GetComponent<NavMeshAgent>();
        this._transform = this.GetComponent<Transform>();
        this._speed = 0.01f;
        this._gameControllerObject = GameObject.Find("Game Controller");
        this._gameController = this._gameControllerObject.GetComponent<GameController>() as GameController;
        this._player = GameObject.Find("Player");
        this._spawnPoint = GameObject.Find("SpawnPoint");
        var i=0;
        foreach (GameObject Patrol in GameObject.FindGameObjectsWithTag("WayPoint"))
        {
            PatrolPoints[i]= Patrol;
            i++;
        }
        _currentPoint = Random.Range(0, 8);
        navMeshAgent.SetDestination(PatrolPoints[_currentPoint].transform.position);
        _updateAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        _slowMovement();
        //First we check distance from the player 
        _currentDistance = Vector3.Distance(_player.transform.position, transform.position);


        //Then we check for visibility
        _checkDirection = _player.transform.position - this.transform.position;
        _ray = new Ray(this.transform.position, _checkDirection);
        if (Physics.Raycast(_ray, out _hit, _maxDistanceToCheck))
        {
            if (_hit.collider.gameObject == _player)
            {
                _animator.SetBool("PlayerVisible", true);
            }
            else
            {
                _animator.SetBool("PlayerVisible", false);
            }
        }
        else
        {
            _animator.SetBool("PlayerVisible", false);
        }
        //Lastly, we get the distance to the next waypoint target
        _distanceFromPoint = Vector3.Distance(PatrolPoints[_currentPoint].transform.position, transform.position);
        _updateAnimator();
    }
    //private methods
    private void _updateAnimator()
    {
        _animator.SetFloat("DistanceFromPlayer", _currentDistance);
        _animator.SetFloat("DistanceFromPoint", _distanceFromPoint);
    }
    private void _slowMovement()
    {
        Vector3 newPosition = this._transform.position;

        if (this._transform.position.y > 3.1f)
        {
            _goingup = false;
        }
        else if (this._transform.position.y < 1.43f)
        {
            _goingup = true;
        }

        if (_goingup)
        {
            newPosition.y += this._speed;
            this._transform.position = newPosition;
        }
        else
        {
            newPosition.y -= this._speed;
            this._transform.position = newPosition;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        _player.transform.position = _spawnPoint.transform.position;
        _player.transform.rotation = _spawnPoint.transform.rotation;
        _gameController.SpookLaugh.Play();
        _gameController.AmountOfSpooks--;
        Destroy(gameObject);
    }
    //Public Methods
    public void SetNextPoint()
    {
        switch (_currentPoint)
        {
            case 0:
                _currentPoint = 1;
                break;
            case 1:
                _currentPoint = 2;
                break;
            case 2:
                _currentPoint = 3;
                break;
            case 3:
                _currentPoint = 4;
                break;
            case 4:
                _currentPoint = 5;
                break;
            case 5:
                _currentPoint = 6;
                break;
            case 6:
                _currentPoint = 7;
                break;
            case 7:
                _currentPoint = 8;
                break;
            case 8:
                _currentPoint = 0;
                break;
        }
        navMeshAgent.SetDestination(PatrolPoints[_currentPoint].transform.position);
    }
}
