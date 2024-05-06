using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI; 
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] EnemySO _enemySO;
    [SerializeField] NavMeshAgent _enemyAgent;
    [SerializeField] NewPlayerController _playerController;
    [SerializeField] Transform _player; 
    [SerializeField] GameObject _projectile;
    [SerializeField] Animator _animator;
    [SerializeField] Transform _thornSpawner;

    [SerializeField] MeshRenderer _pointA;
    [SerializeField] MeshRenderer _pointB;

    [SerializeField] LayerMask _playerLayer;

    //

    [SerializeField] AudioClip SmackHitSound; // Bonus SFX, Basic Hit Sound of Smack.
    [SerializeField] AudioClip Enemy_RangedMiss; // When the enemy misses the player with the with Ranged Attack
    [SerializeField] AudioClip Enemy_MeleeHit; // When the enemy misses the player with the with Ranged Attack
    [SerializeField] AudioClip Enemy_MeleeMiss; // When the enemy misses the player with the with Ranged Attack


    private string _type;
    private Vector3 _distanceToWalkPoint;
    private bool _goingToTA = true;

    private int _EnemyHealthPoint = 3;
    private float _baseSpeed = 1;
    public float _timeBetweenAttacks;
    bool _attackAlready;

    //States
    public float _sightRange;
    public float _attackRange;
    private bool _playerIsInMySight = false;
    private bool _playerInAttackRange = false;

    private void Awake()
    {

        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.GamePlay;
        if (newGameState == GameState.GamePlay) 
        {
            Time.timeScale = 1;
            _animator.speed = 1;
        }
        else
        {
            Time.timeScale = 0;
            _animator.speed = 0;
        }
    }

    private void Start()
    {
        _EnemyHealthPoint = _enemySO.startingHealth;
        _baseSpeed = _enemySO.baseSpeed;
        _type = _enemySO.type.ToString();
        _playerController.MaxEnemyCount++;
        
        _pointA.enabled = false;
        _pointB.enabled = false;
    }
    void Update()
    {
        enemeyState();
    }

    private void enemeyState()
    {
        //Check for Sight and Attack Range
        _playerIsInMySight = Physics.CheckSphere(transform.position, _sightRange, _playerLayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _playerLayer);

        if (!_playerIsInMySight && !_playerInAttackRange)
        {
            Patroling();
        }
        else if (_playerIsInMySight && !_playerInAttackRange)
        {
            ChasePlayer();
        }
        else if(_playerIsInMySight && _playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        _animator.SetBool("Attacking", false);

        if (_goingToTA)
        {
            _enemyAgent.SetDestination(_pointA.gameObject.transform.position);
            _distanceToWalkPoint = transform.position - _pointA.gameObject.transform.position;
        }
        else if (!_goingToTA)
        {
            _enemyAgent.SetDestination(_pointB.gameObject.transform.position);
            _distanceToWalkPoint = transform.position - _pointB.gameObject.transform.position;
        }

        if (_distanceToWalkPoint.magnitude < 2f)
        {
            _goingToTA = !_goingToTA;
        }

    }

    private void ChasePlayer() // great
    {
        _animator.SetBool("Attacking", false);

        _enemyAgent.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        
        _enemyAgent.velocity = Vector3.zero;
        _animator.SetBool("Attacking", true);
        _enemyAgent.SetDestination(_player.position); 

        transform.LookAt(_player);

        if (!_attackAlready)
        {
            switch (_type)
            {
                case "Shooter": ShooterAttack(); break;
                case "Giant": MeleeAttack(); break;
                case "Flyer": break;
            }
 
        }
    }

    private void ResetAttack()
    {
        _attackAlready= false;
    }

    public void TakeDamage()
    {
        Debug.Log(_EnemyHealthPoint);
        _EnemyHealthPoint--;
        if (_EnemyHealthPoint == 0)
        {
            _playerController.EnemyCount ++;
            _playerController.UpdateEnemyCount();
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos() // SHOULD BE REMOVED
    {
       Gizmos.color = Color.yellow;
       Gizmos.DrawWireSphere(transform.position, _attackRange);
       Gizmos.color = Color.yellow;
       Gizmos.DrawWireSphere(transform.position, _sightRange);
    }

    private void ShooterAttack()
    {
        Instantiate(_projectile, _thornSpawner.position, _thornSpawner.rotation);
        _attackAlready = true;
        Invoke(nameof(ResetAttack), _timeBetweenAttacks);
    }
    private void MeleeAttack()
    {
        _enemyAgent.velocity = Vector3.zero;
    }
}
