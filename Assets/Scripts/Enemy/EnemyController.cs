// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private Transform target;
    GameObject playa;
    EnemyManager em;

    float viewAngle = 60f;
    float viewRadius = 10f;

    private LayerMask targetMask, playerMask;
    bool isPlayerInRange;
    Vector3 playerPosition;

    private const float CHASE_RANGE = 15f;
    private const float ATTACK_RANGE = 1f;
    private const float ROAM_SPEED = 1f;
    private const float CHASE_SPEED = 4.5f;
    // float distanceToTarget;
    private bool isTargetInChaseRange, isTargetInAttackRange;

    [SerializeField] private Vector3 roamPoint;
    private bool isRoamPointSet;
    const float ROAM_RANGE = 7.5f;
    
    private bool isAttacking;
    private float attackInterval = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        em = GetComponent<EnemyManager>();
        
        playerMask = LayerMask.GetMask("Player");
        playa = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        SetTarget(playa.transform, playerMask);

        isPlayerInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ScanForPlayer();
        if (target == null) SetTarget(playa.transform, playerMask);

        isTargetInChaseRange = Physics.CheckSphere(transform.position, CHASE_RANGE, targetMask);
        isTargetInAttackRange = Physics.CheckSphere(transform.position, ATTACK_RANGE, targetMask);

        if (!isTargetInChaseRange){
            Roam();
        } else {
            if (!isTargetInAttackRange)
                MoveToTarget();
            else AttackTarget();
        }

        // distanceToTarget = Vector3.Distance(transform.position, target.position);
    }

    public void SetTarget(Transform newTarget, LayerMask newMask){
        target = newTarget;
        targetMask = newMask;
    }
    void Roam(){
        if (!isRoamPointSet)
            GetMovePoint();
        else {
            agent.SetDestination(roamPoint);
            agent.speed = ROAM_SPEED;
        }

        Vector3 distanceToMovePoint = transform.position - roamPoint;
        isRoamPointSet = true;

        if (distanceToMovePoint.magnitude < 1f)
            isRoamPointSet = false;
    }
    void GetMovePoint(){
        // calculate random move point within range
        float randomX = Random.Range(-ROAM_RANGE, ROAM_RANGE);
        float randomZ = Random.Range(-ROAM_RANGE, ROAM_RANGE);

        roamPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }
    void MoveToTarget()
    {
        // RotateToTarget();
        agent.SetDestination(target.position);
        agent.speed = CHASE_SPEED;
        transform.LookAt(target);
    }
    // void RotateToTarget()
    // {
    //     Vector3 direction = target.position - transform.position;
    //     Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
    //     transform.rotation = rotation;
    // }
    void AttackTarget(){
        // stop the enemy
        agent.SetDestination(transform.position);

        // attacking the target
        if (!isAttacking){
            if (target.TryGetComponent(out IDamage dmg)) target.GetComponent<IDamage>().TakeDamage(em.GetAttackDamage());

            isAttacking = true;
            Invoke(nameof(ResetAttack), attackInterval);
        }
    }
    void ResetAttack(){
        isAttacking = false;
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CHASE_RANGE);
    }

    // void ScanForPlayer(){
    //     Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, LayerMask.GetMask("Player"));

    //     for (int i = 0; i < playerInRange.Lenght; i++){
    //         Transform player = playerInRange[i].transform;
    //         Vector3 directionToPlayer = (player.position - transform.position).normalized;
            
    //         if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle/2){
    //             float distanceToPlayer = Vector3.Distance(transform.position, player.position);
    //             if (Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer)){
    //                 isPlayerInRange = true;
    //             }
    //             else isPlayerInRange = false;
    //         }

    //         if (Vector3.Distance(transform.position, player.position) > viewRadius){
    //             isPlayerInRange = false;
    //         }

    //         if (isPlayerInRange){
    //             playerPosition = player.position;
    //         }
    //     }
    // }

    // void Chase(){
    //     isPlayer
    // }
}