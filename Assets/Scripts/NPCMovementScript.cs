using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovementScript : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Animator _animator;
    [SerializeField] TriggerChecker triggerChecker;
    [SerializeField] GameObject entity;
    
    // Define a variable for animation blend
    private float _animationBlend;

    // Define animator IDs
    private int _animIDSpeed;
    private int _animIDMotionSpeed;




    [Header("Configurables")]
    [Range(0, 10)] [SerializeField] float movementSpeed;
    [SerializeField] float scaledSpeedConfig = 6f;
    [SerializeField] float wanderRadius = 10f;
    [SerializeField] float SpeedChangeRate = 10.0f; // Speed of transition between walk and idle animation
    [Range(0, 10)] [SerializeField] float minStoppingTime;
    [Range(0, 10)] [SerializeField] float maxStoppingTime;


    void Start()
    {   
        // Initialize animator parameters
        AssignAnimationIDs();
        
        SetRandomDestination();
        navMeshAgent.speed = movementSpeed;
    }

    void Update()
    {
        UpdateAnimator();
    }

    public void MoveNPCRandom()
    {
        if(navMeshAgent == null) { return; }
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            StartCoroutine(SetRandomDestination());
        }    
    }

    void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    void UpdateAnimator()
    {
        // Get the speed of the NavMeshAgent
        float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;

        float scaledSpeed = speed * scaledSpeedConfig;


        // You can adjust this calculation if you want a different animation blending setup
        _animationBlend = Mathf.Lerp(_animationBlend, scaledSpeed, Time.deltaTime * SpeedChangeRate);

        // Set the animator parameters
        _animator.SetFloat(_animIDSpeed, _animationBlend);
        _animator.SetFloat(_animIDMotionSpeed, speed > 0 ? 1f : 0); // 1 if moving, 0 if not
    }

    IEnumerator SetRandomDestination()
    {
        yield return new WaitForSeconds(Random.Range(minStoppingTime, maxStoppingTime));
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        Vector3 finalPosition = hit.position;
        navMeshAgent.SetDestination(finalPosition);
    }

    public void SetDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
        //navMeshAgent.Move(navMeshAgent.desiredVelocity);
    }

    public GameObject GetPlayer()
    {
        if(triggerChecker.GetPlayer() == null) { return null; }
        return triggerChecker.GetPlayer();
    }

    public GameObject GetEntity()
    {
        if(entity == null) { Debug.Log("Entity is null"); return null; }
        return entity;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return navMeshAgent;
    }
}
