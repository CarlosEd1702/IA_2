using UnityEngine;

public class ZombieChase : StateMachineBehaviour
{
    [SerializeField] private float _zombieVelocity = 2.5f;
    [SerializeField] private float _defaultChaseTime = 5f;
    [SerializeField] private float _chaseRadius = 10f;  // Rango de detección para seguir al jugador

    private float _timeFollow;
    private Transform _playerTransform;
    private Zombie _zombie;

    // OnStateEnter es llamado al entrar en el estado de persecución
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _timeFollow = _defaultChaseTime;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _zombie = animator.GetComponent<Zombie>();

        // Inicia la animación de correr
        animator.SetBool("isRunning", true);
    }

    // OnStateUpdate se llama en cada frame mientras se evalúa este estado
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_playerTransform == null) return;

        // Verifica la distancia entre el zombie y el jugador
        float distanceToPlayer = Vector3.Distance(animator.transform.position, _playerTransform.position);

        if (distanceToPlayer > _chaseRadius || _timeFollow <= 0)
        {
            // Si el jugador está fuera del rango de detección o se acaba el tiempo de persecución, vuelve al estado de patrullaje
            animator.SetTrigger("Back");
            animator.SetBool("isRunning", false);
            return;
        }

        // Mueve al zombie hacia el jugador en 3D (X y Z)
        Vector3 direction = (_playerTransform.position - animator.transform.position).normalized;
        direction.y = 0; // Asegura que no haya movimiento en Y

        animator.transform.position += direction * _zombieVelocity * Time.deltaTime;
        animator.transform.LookAt(new Vector3(_playerTransform.position.x, animator.transform.position.y, _playerTransform.position.z));

        // Reduce el tiempo de persecución
        _timeFollow -= Time.deltaTime;
    }

    // OnStateExit es llamado cuando el estado de persecución termina
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Resetea el estado de animación de correr
        animator.SetBool("isRunning", false);
    }
}
