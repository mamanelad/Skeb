using System;
using UnityEngine;
using System.Collections;
using Pathfinding;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    #region Private Fields

    private PlayerController _playerController;
    private Enemy.EnemyKind _enemyKind;
    private Transform _target; // What to chase?
    private Seeker _seeker;
    private Rigidbody2D _rb;
    private Path _path; //The calculated path
    private int _currentWaypoint; // The waypoint we are currently moving towards

    private float
        _nextWaypointDistance; // The max distance from the AI to a waypoint for it to continue to the next waypoint

    private float _updateRate; // How many times each second we will update our path
    private float _speed; //The AI's speed per second
    private bool _searchingPlayer;

    #endregion

    #region Public Fields

    [HideInInspector] public bool pathIsEnded;
    [HideInInspector] public bool lockMovement;

    #endregion

    #region Inspector Control

    [Header("Speciel Movement Settings")] [SerializeField]
    private float distanceMagePlayer = 2f;


    [Header("Movement Force Settings")] [SerializeField]
    private ForceMode2D fMode;

    [Header("Way point Settings")] [SerializeField]
    private float maxNextWaypointDistance = 20f;

    [SerializeField] private float minNextWaypointDistance = 1f;

    [SerializeField] private float maxUpdateRate = 20f;
    [SerializeField] private float minUpdateRate = 2f;

    [Header("Speed movement Settings")] [SerializeField]
    float maxSpeed = 800f;

    [SerializeField] float minSpeed = 250f;

    [SerializeField] private float minMass = 1;
    [SerializeField] private float maxMass = 5;

    [SerializeField] private float minLinearDrag = 1f;
    [SerializeField] private float maxLinearDrag = 6f;

    #endregion

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _enemyKind = GetComponent<Enemy>()._enemyKind;
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        _rb.mass = Random.Range(minMass, maxMass);
        _rb.drag = Random.Range(minLinearDrag, maxLinearDrag);

        _updateRate = Random.Range(minUpdateRate, maxUpdateRate);
        _speed = Random.Range(minSpeed, maxSpeed);
        _nextWaypointDistance = Random.Range(minNextWaypointDistance, maxNextWaypointDistance);

        if (_target == null)
        {
            if (!_searchingPlayer)
            {
                _searchingPlayer = true;
                StartCoroutine(SearchPlayer());
            }

            return;
        }

        // Start a new path to the target position, return the result to the OnPathComplete method
        _seeker.StartPath(transform.position, _target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    private IEnumerator SearchPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchPlayer());
        }
        else
        {
            _target = sResult.transform;
            _searchingPlayer = false;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }

    private IEnumerator UpdatePath()
    {
        if (_target == null)
        {
            if (!_searchingPlayer)
            {
                _searchingPlayer = true;
                StartCoroutine(SearchPlayer());
            }

            yield return false;
        }

        // Start a new path to the target position, return the result to the OnPathComplete method
        _seeker.StartPath(transform.position, _target.position, OnPathComplete);
        var currUpdateRate = _updateRate;
        yield return new WaitForSeconds(1f / currUpdateRate);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        // Debug.Log ("We got a path. Did it have an error? " + p.error);
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (lockMovement || _playerController.IsPlayerDead || _playerController._playerState
            == PlayerController.PlayerState.Falling) return;

        if (_target == null)
        {
            if (!_searchingPlayer)
            {
                _searchingPlayer = true;
                StartCoroutine(SearchPlayer());
            }

            return;
        }


        if (_path == null)
            return;

        if (_currentWaypoint >= _path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            // Debug.Log ("End of path reached.");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        //Direction to the next waypoint
        var position = transform.position;
        Vector3 dir = (_path.vectorPath[_currentWaypoint] - position).normalized;
        var curSpeed = _speed;
        dir *= curSpeed * Time.fixedDeltaTime;

        //Move the AI

        //Mage Special movement
        var distance = Vector3.Distance(transform.position, _target.position);
        if (_enemyKind == Enemy.EnemyKind.Middle && GameManager.Shared.CurrentState == GameManager.WorldState.Ice &&
            distance <= distanceMagePlayer)
        {
            _rb.AddForce(-dir, fMode);
        }

        else
        {
            _rb.AddForce(dir, fMode);
        }

        if (dir != Vector3.zero)
            MonsterWalkSound();

        var dist = Vector3.Distance(position, _path.vectorPath[_currentWaypoint]);
        var nextWaypointDistance = _nextWaypointDistance;
        if (!(dist < nextWaypointDistance)) return;
        _currentWaypoint++;
    }

    private void MonsterWalkSound()
    {
        switch (_enemyKind)
        {
            case Enemy.EnemyKind.Small:
                MonsterSound(MonsterSounds.SoundKindsMonster.SWalk);
                break;
            case Enemy.EnemyKind.Middle:
                MonsterSound(MonsterSounds.SoundKindsMonster.MWalk);
                break;
            case Enemy.EnemyKind.Big:
                MonsterSound(MonsterSounds.SoundKindsMonster.BWalk);
                break;
        }
    }
    
    public void MonsterFallSound()
    {
        switch (_enemyKind)
        {
            case Enemy.EnemyKind.Middle:
                MonsterSound(MonsterSounds.SoundKindsMonster.MFall);
                break;
            case Enemy.EnemyKind.Big:
                MonsterSound(MonsterSounds.SoundKindsMonster.BFall);
                break;
        }
    }
    
    public void MonsterAttackSound()
    {
        switch (_enemyKind)
        {
            case Enemy.EnemyKind.Small:
                MonsterSound(MonsterSounds.SoundKindsMonster.SAttack);
                break;
            case Enemy.EnemyKind.Middle:
                MonsterSound(MonsterSounds.SoundKindsMonster.MAttack);
                break;
            case Enemy.EnemyKind.Big:
                switch (GameManager.Shared.CurrentState)
                {
                    case GameManager.WorldState.Fire:
                        MonsterSound(MonsterSounds.SoundKindsMonster.BFireAttack);
                        break;
                    case GameManager.WorldState.Ice:
                        MonsterSound(MonsterSounds.SoundKindsMonster.BIceAttack);
                        break;
                }
                break;
        }
    }

    private void MonsterSound(MonsterSounds.SoundKindsMonster soundKindsMonster)
    {
        GameManager.Shared.monsterAudioManager.PlaySound(soundKindsMonster);
    }
}