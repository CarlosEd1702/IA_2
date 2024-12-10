using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _detectionRadius = 10f;
    [SerializeField] private float _chaseSpeed = 2f;
    [SerializeField] private float _patrolSpeed = 1f;
    [SerializeField] private Transform[] _patrolPoints;
    private int _currentPatrolIndex = 0;

    private Animator _animator;

    private enum ZombieState { Patrol, Chase, Idle }
    private ZombieState currentState;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        currentState = ZombieState.Patrol;
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case ZombieState.Patrol:
                    Patrol();
                    break;
                case ZombieState.Chase:
                    ChasePlayer();
                    break;
                case ZombieState.Idle:
                    Idle();
                    break;
            }
            yield return null;
        }
    }

    private void Patrol()
    {
        _animator.SetBool("isRunning", true);
        Transform targetPatrolPoint = _patrolPoints[_currentPatrolIndex];
        MoveTowards(targetPatrolPoint.position, _patrolSpeed);

        if (Vector3.Distance(transform.position, targetPatrolPoint.position) < 0.5f)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
        }

        if (Vector3.Distance(transform.position, _player.position) <= _detectionRadius)
        {
            currentState = ZombieState.Chase;
        }
    }

    private void ChasePlayer()
    {
        _animator.SetBool("isRunning", true);
        MoveTowards(_player.position, _chaseSpeed);

        if (Vector3.Distance(transform.position, _player.position) > _detectionRadius)
        {
            currentState = ZombieState.Patrol;
        }
    }

    private void Idle()
    {
        _animator.SetBool("isRunning", false);
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
