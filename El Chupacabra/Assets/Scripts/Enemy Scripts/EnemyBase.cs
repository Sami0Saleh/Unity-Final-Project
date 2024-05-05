using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI; 
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.PackageManager;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] EnemySO _enemySO;
    [SerializeField] NavMeshAgent _enemyAgent; 
    [SerializeField] Transform _player; 
    [SerializeField] GameObject _projectile;
    [SerializeField] Animator _animator;

    [SerializeField] MeshRenderer _pointA;
    [SerializeField] MeshRenderer _pointB;

    [SerializeField] LayerMask _playerLayer;

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

    private void Start()
    {
        _EnemyHealthPoint = _enemySO.startingHealth;
        _baseSpeed = _enemySO.baseSpeed;
        _type = _enemySO.type.ToString();
        

        _pointA.enabled = false;
        _pointB.enabled = false;
    }
    void Update()
    {
        enemeyState();
        Debug.Log($"Attacking is {_animator.GetBool("Attacking")}");

    }

    private void enemeyState()
    {
        //Check for Sight and Attack Range
        _playerIsInMySight = Physics.CheckSphere(transform.position, _sightRange, _playerLayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _playerLayer);

        if (!_playerIsInMySight && !_playerInAttackRange)
        {
            Debug.Log("Patroling");
            Patroling();
        }
        else if (_playerIsInMySight && !_playerInAttackRange)
        {
            Debug.Log("Chasing");
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
        //make sure the enemey does NOT MOVE

        _animator.SetBool("Attacking", true);
        _enemyAgent.SetDestination(_player.position); transform.LookAt(_player);

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

    public void TakeDamage(int damage)
    {
        Debug.Log(_EnemyHealthPoint);
        _EnemyHealthPoint--;
        if (_EnemyHealthPoint <= 0)
        {
           Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos() // SHOULD BE REMOVED
    {
       Gizmos.color = Color.yellow;
       Gizmos.DrawWireSphere(transform.position, _attackRange);
       Gizmos.color = Color.yellow;
       Gizmos.DrawWireSphere(transform.position, _sightRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerFist")
        {
            Debug.Log("Been Attacked");
            TakeDamage(1);
        }
    }

    private void ShooterAttack()
    {
        _enemyAgent.speed = 0;
        Instantiate(_projectile, transform.position, transform.rotation);
        _attackAlready = true;
        Invoke(nameof(ResetAttack), _timeBetweenAttacks);
    }
    private void MeleeAttack()
    {
        _enemyAgent.velocity = Vector3.zero;
    }
}
