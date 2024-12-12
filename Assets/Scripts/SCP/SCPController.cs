using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class SCPController : MonoBehaviour
{
    #region Variables

    private const float MIN_EFFECT_RANGE = 5f;
    private const float MAX_EFFECT_RANGE = 10f;
    
    public Transform player;
    public SCPState SCP_State;
    public NavMeshAgent agent;
    public float agentSpeed = 3f;
    public float agentAcceleration = 1000f; // High acceleration to instantly reach max speed
    public float detectionRange = 20f; // SCP detection range (visual)
    public float attackRange = 2f; // Range at which SCP catches the player
    public float fieldOfViewAngle = 120f; // Field of View (FOV) angle
    public float soundDetectionRange = 15f; // SCP sound detection range

    public Transform scpHead; // Position of the SCP's head for raycast
    public LayerMask obstructionMask; // Mask for objects that can block line of sight (e.g., walls)
    
    public Animator animator; // Reference to the Animator component
    public GameObject scpModel; // Reference to the SCP model (child GameObject) that will blink

    public float invisibleDuration = 3f; // Fixed time SCP stays invisible
    public float minVisibleTime = 2f; // Minimum time SCP stays visible
    public float maxVisibleTime = 5f; // Maximum time SCP stays visible
    public bool isVisible = false;
    
    public bool playerInSight; // Is the player visible to SCP?
    public bool playerHeard; // Has the SCP heard the player?
    private Vector3 lastSoundPosition; // Last position where SCP heard the player
    private bool isCatchPlayer = false;

    public SimpleFirstPersonController simpleFirstPersonController; // Reference to the player's movement script
    public bool forcePlayer;
    public ActionSoundControl actionSoundControl;

    public GameObject volumeEffect;
    public List<Transform> wanderingPositions;
    private Transform currentWanderingTarget;
    private bool isWaitingAtPosition = false;

    public bool isWanderingMode;

    private bool _isStun;

    #endregion

    #region Unity Methods

    private void Start()
    {
        StartCoroutine(BlinkRandomly());
        SoundManager.instance.PlayFloatingSound();

        if (agent)
        {
            agent.speed = agentSpeed;
            agent.acceleration = agentAcceleration; // High acceleration for instant max speed
        }
    }

    private void Update()
    {
        if(!simpleFirstPersonController)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<SimpleFirstPersonController>();
        }

        if(!actionSoundControl)
        {
            actionSoundControl = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionSoundControl>();
        }
        

        if (IsPlayerCaught()) return; // Skip normal behavior if player is caught

        playerInSight = IsPlayerInSight();
        playerHeard = IsPlayerHeard();

        if (playerInSight)
        {
            agent.SetDestination(player.position); // Chase the player

            if (actionSoundControl)
            {
                actionSoundControl.PlayDetectionSound(); // Play detection sound
                actionSoundControl.StartChaseMusic(); // Start chase music
            }

            if (volumeEffect.GetComponent<SphereCollider>().radius < MAX_EFFECT_RANGE)
            {
                volumeEffect.GetComponent<SphereCollider>().radius += Time.deltaTime * 2f;
            }
        }
        else if (playerHeard)
        {
            agent.SetDestination(lastSoundPosition); // Move to the last sound location

            if (actionSoundControl)
            {
                actionSoundControl.StartChaseMusic(); // Continue chase music
            }

            if (volumeEffect.GetComponent<SphereCollider>().radius < MAX_EFFECT_RANGE)
            {
                volumeEffect.GetComponent<SphereCollider>().radius += Time.deltaTime * 2f;
            }
        }
        else
        {
            if (actionSoundControl)
            {
                actionSoundControl.StopChaseMusic(); // Stop chase music when not chasing
            }

            if (volumeEffect.GetComponent<SphereCollider>().radius > MIN_EFFECT_RANGE)
            {
                volumeEffect.GetComponent<SphereCollider>().radius -= Time.deltaTime;
            }
        }

        // SCP catches the player if close enough
        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            if(!isCatchPlayer)
            {
                CatchPlayer(); // Stop the player and make SCP visible permanently
                isCatchPlayer = true;
            }
        }

        UpdateAnimation();
        DebugRayToPlayer();

        if(SCP_State == SCPState.normal)
        {
            invisibleDuration = 0.8f;
            minVisibleTime = 0.3f;
            maxVisibleTime = 0.5f;
            agentSpeed = 5;
        }
        else if(SCP_State == SCPState.Angry)
        {
            invisibleDuration = 0.8f;
            minVisibleTime = 1f;
            maxVisibleTime = 2f;
            agentSpeed = 5;
        }
        else if(SCP_State == SCPState.Mad)
        {
            invisibleDuration = 0.6f;
            minVisibleTime = 0.1f;
            maxVisibleTime = 0.3f;
            agentSpeed = 7;
        }

        if (isWanderingMode && !playerInSight && !playerHeard)
        {
            PerformWandering();
        }
    }
    private void PerformWandering()
    {
        if (isWaitingAtPosition || wanderingPositions.Count == 0) return;

        if (currentWanderingTarget == null || agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WanderingRoutine());
        }
    }

    private IEnumerator WanderingRoutine()
    {
        isWaitingAtPosition = true;

        // If the agent is at a target, stop and wait for 2 seconds
        agent.isStopped = true;
        yield return new WaitForSeconds(3f);

        // Select a new random position
        currentWanderingTarget = wanderingPositions[Random.Range(0, wanderingPositions.Count)];
        agent.SetDestination(currentWanderingTarget.position);
        agent.isStopped = false;

        isWaitingAtPosition = false;
    }
    #endregion

    #region Player Catching Mechanism

    bool IsPlayerCaught()
    {
        // Logic to check if the SCP has already caught the player
        return !simpleFirstPersonController.enabled; // If player control is disabled, they are caught
    }

    void CatchPlayer()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.layer = 0;
        }
        

        // Stop SCP from going invisible once the player is caught
        StopAllCoroutines(); // Stop blinking behavior
        scpModel.SetActive(true); // Make SCP permanently visible

        SoundManager.instance.PlayJumpscareSound();
        LookAtPlayer();

        // Stop player movement and camera control
        simpleFirstPersonController.disablePlayerControll = true;

        // Force the player's camera to look at the SCP's face
        simpleFirstPersonController.StartSmoothLookAt(scpHead);

        // Calculate the target position near the SCP's face for the camera to move towards
        Vector3 targetPosition = scpHead.position + scpHead.forward * 0.5f; // 0.5 units in front of the SCP's face
        simpleFirstPersonController.StartMoveCloser(targetPosition); // Start moving the camera closer


        // Stop SCP movement
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        Debug.Log("SCP has caught the player!");

        GameManager.Instance.RestartGame();
        TabletManager.Instance.isDisableTablet = true;
    }

    void LookAtPlayer()
    {
        // Calculate direction from SCP to player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Ensure SCP only rotates on the Y-axis to face the player (prevent tilting)
        directionToPlayer.y = 0;

        // Calculate the rotation required to face the player
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate SCP to face the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); // Adjust 10f for speed of rotation
    }


    #endregion

    #region Animation Control

    void UpdateAnimation()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed); // Control animation based on SCP speed
    }

    #endregion

    #region Blinking Logic

    public IEnumerator BlinkRandomly()
    {
        while (!_isStun)
        {
            // Hide the SCP model (make it invisible) and allow it to move
            scpModel.SetActive(false);
            isVisible = false;
            agent.isStopped = false;

            yield return new WaitForSeconds(invisibleDuration);

            // Show the SCP model (make it visible) and stop it from moving
            scpModel.SetActive(true);
            isVisible = true;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            // Play the glitch sound when the SCP becomes visible
            if(TabletManager.Instance.isTablet)
                SoundManager.instance.PlayGlitchSound();

            float visibleTime = Random.Range(minVisibleTime, maxVisibleTime);
            yield return new WaitForSeconds(visibleTime);
        }
    }

    #endregion

    #region Player Detection

    bool IsPlayerInSight()
    {
        if(forcePlayer)
        {
            return true;
        }
        else if(Vector3.Distance(player.position, scpHead.position) < 15)
        {
            Vector3 directionToPlayer = (player.position - scpHead.position).normalized;
            float angleBetweenSCPAndPlayer = Vector3.Angle(scpHead.forward, directionToPlayer);
            if (angleBetweenSCPAndPlayer < fieldOfViewAngle / 2f)
            {
                float distanceToPlayer = Vector3.Distance(scpHead.position, player.position);
                if (!Physics.Raycast(scpHead.position, directionToPlayer, distanceToPlayer, obstructionMask))
                {
                    Debug.DrawRay(scpHead.position, directionToPlayer * distanceToPlayer, Color.green);
                    return true;
                }
                else
                {
                    Debug.DrawRay(scpHead.position, directionToPlayer * distanceToPlayer, Color.red);
                }
            }
        }
        return false;
    }

    bool IsPlayerHeard()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0);
        if (isRunning && Vector3.Distance(player.position, transform.position) <= soundDetectionRange)
        {
            lastSoundPosition = player.position;
            Debug.Log("SCP heard the player running!");
            return true;
        }
        return false;
    }

    #endregion

    #region Debugging

    void DebugRayToPlayer()
    {
        Vector3 directionToPlayer = (player.position - scpHead.position).normalized;
        float distanceToPlayer = Vector3.Distance(scpHead.position, player.position);
        Debug.DrawRay(scpHead.position, directionToPlayer * distanceToPlayer, Color.red);
    }

    private void OnDrawGizmosSelected()
    {
        if (scpHead == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(scpHead.position, detectionRange);

        Vector3 fovLine1 = Quaternion.Euler(0, fieldOfViewAngle / 2f, 0) * scpHead.forward * detectionRange;
        Vector3 fovLine2 = Quaternion.Euler(0, -fieldOfViewAngle / 2f, 0) * scpHead.forward * detectionRange;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(scpHead.position, fovLine1);
        Gizmos.DrawRay(scpHead.position, fovLine2);

        int rayCount = 30;
        for (int i = 0; i <= rayCount; i++)
        {
            float angle = -fieldOfViewAngle / 2 + (i * (fieldOfViewAngle / rayCount));
            Vector3 direction = Quaternion.Euler(0, angle, 0) * scpHead.forward;
            Gizmos.color = new Color(0, 0, 1, 0.1f);
            Gizmos.DrawRay(scpHead.position, direction * detectionRange);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, soundDetectionRange);
    }

    #endregion

    #region Attack

    void AttackPlayer()
    {
        Debug.Log("SCP is attacking the player!");
    }

    #endregion
    public enum SCPState
    {
        normal,
        Angry,
        Mad
    }

    #region Stun

    public void StunSCP(float stunDuration)
    {
        StartCoroutine(StunRoutine(stunDuration));
    }

    private IEnumerator StunRoutine(float stunDuration)
    {
        Debug.Log("SCP is stunned!");

        // Make SCP invisible and stop it
        _isStun = true;
        scpModel.SetActive(true);
        isVisible = true;
        agent.isStopped = true;
        animator.SetBool("Stun",true);
        foreach (Transform child in gameObject.transform)
        {
            if (child == null) continue;
        }
        agent.velocity = Vector3.zero;

        // Wait for the stun duration
        yield return new WaitForSeconds(stunDuration);

        // Reactivate SCP
        animator.SetBool("Stun",false);
        _isStun = false;
        StartCoroutine(BlinkRandomly());
        foreach (Transform child in gameObject.transform)
        {
            if (child == null) continue;
        }
        Debug.Log("SCP is no longer stunned!");
    }

    #endregion
}
