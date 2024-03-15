using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class VokzalGuyScript : MonoBehaviour
{
    [Header("Links")]
    //private VokzalGuySpriteController SpriteController;
    private LevelController _LevelController;
    //[SerializeField] private SoundController _SoundController;
    private VokzalGuyAnimationController _AnimationController;
    [SerializeField] private VokzalGuyVoiceScript _VoiceScript;
    [Space]
    [Header("Settings")]
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float PatrolSpeed;
    [SerializeField] private float GetShotSpeed;
    [SerializeField] private float GetShotTime;
    [SerializeField] private float SpeedRegenerationStep;
    private float currentSpeed;
    private bool isGetShot;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] bool Chase = true;
    [SerializeField] private bool PlayerInArea;
    private bool SeePlayer;
    [SerializeField] private float CheckTime;
    [SerializeField] private float TimeBeforeHit;
    [SerializeField] private Transform[] PatrolPoints;
    [SerializeField] private Transform currentPatrolPoint;
    [SerializeField] private Transform previousPatrolPoint;
    [SerializeField] private Vector3 chasePlayerPosition;
    [SerializeField] private bool Patrol;
    [SerializeField] private bool findingNewPatrolPoint = false;
    [SerializeField] private LayerMask PlayerLayer; 
    [SerializeField] private LayerMask LocationLayer; 
    [Header("Settings Scene")]
    [SerializeField] private Transform Player;
    
    

    public bool GetSeePlayerState() { return PlayerInArea; }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        navMesh = GetComponent<NavMeshAgent>();
        Chase = false;
        currentSpeed = WalkSpeed;
        _LevelController = LevelController.GetInstance();
        _AnimationController = GetComponent<VokzalGuyAnimationController>();
    }
    private void NewPatrolPoint()
    {
        previousPatrolPoint = currentPatrolPoint;
        int id = Random.Range(0, PatrolPoints.Length);
        currentPatrolPoint = PatrolPoints[id];
        if (previousPatrolPoint != null)
        {
            if (currentPatrolPoint == previousPatrolPoint)
            {
                while (currentPatrolPoint == previousPatrolPoint)
                {
                    id = Random.Range(0, PatrolPoints.Length); ;
                    currentPatrolPoint = PatrolPoints[id];
                }
            }
        }
        navMesh.destination = currentPatrolPoint.position;
        findingNewPatrolPoint = false;
    }
    private void Update()
    {
        if (!_LevelController.GetPauseState())
        {
            navMesh.speed = currentSpeed;
            if (PlayerInArea)
            {
                float distanceToTarget = Vector3.Distance(transform.position, Player.position);
                Vector3 directionToTarget = (Player.position - transform.position).normalized;
                if (Physics.Raycast(transform.position, directionToTarget, distanceToTarget, LocationLayer))
                {
                    SeePlayer = false;
                }
                else
                {
                    Debug.Log("SEE player!");
                    navMesh.destination = Player.position;
                    chasePlayerPosition = Player.position;
                    Patrol = false;
                    SeePlayer = true;
                    _AnimationController.UpdateChasingBool(true);
                }
            }
            else
            {
                SeePlayer = false;
            }
            if (Patrol)
            {
                currentSpeed = PatrolSpeed;
                if ((currentPatrolPoint == null || Vector3.Distance(transform.position, currentPatrolPoint.position) < 1f) && !findingNewPatrolPoint)
                {
                    findingNewPatrolPoint = true;
                    NewPatrolPoint();
                }

            }
            if (Chase && !isGetShot)
            {
                if (Vector3.Distance(transform.position, chasePlayerPosition) < 2.5f && !SeePlayer)
                {
                    Chase = false;
                    _AnimationController.UpdateChasingBool(false);
                    StartCoroutine(CheckArea());
                }
                if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
                {
                    Chase = false;
                    transform.LookAt(Player.position);
                    _AnimationController.UpdateChasingBool(false);
                    navMesh.isStopped = true;
                    StartCoroutine(Hit());
                }
            }
            else if (!isGetShot)
            {

                if (Vector3.Distance(transform.position, Player.position) > 3f && SeePlayer)
                {
                    Chase = true;
                    navMesh.isStopped = false;
                    StopAllCoroutines();
                    currentSpeed = WalkSpeed;
                }
            }

        }

    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Chase = true;
            PlayerInArea = true;
            _AnimationController.UpdateChasingBool(true);
            StopAllCoroutines();
            //_SoundController.StartChaseSound();
            _VoiceScript.StartTalking();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerInArea = false;
            _VoiceScript.StopTalking();
        }
    }
    public void StartMotion()
    {
        Chase = true;
        Patrol = true;
        if (_AnimationController == null)
            _AnimationController = GetComponent<VokzalGuyAnimationController>();
        _AnimationController.UpdateChasingBool(false);
    }
    public void GetShot()
    {
        StopAllCoroutines();
        StartCoroutine(GetShotC());
        _AnimationController.Flash();
    }
    private IEnumerator GetShotC()
    {
        isGetShot = true;
        currentSpeed = GetShotSpeed;
        yield return new WaitForSeconds(GetShotTime);
        isGetShot = false;
        while (currentSpeed < WalkSpeed)
        {
            currentSpeed += SpeedRegenerationStep;
        }
        currentSpeed = WalkSpeed;
        if (Vector3.Distance(transform.position, Player.position) <= 3f && PlayerInArea)
        {
            Chase = false;
            navMesh.isStopped = true;
            StopAllCoroutines();
            StartCoroutine(Hit());
        }
    }
    private IEnumerator CheckArea()
    {
        _AnimationController.Check();
        _AnimationController.UpdateChasingBool(false);
        yield return new WaitForSeconds(CheckTime);
        Patrol = true;
        NewPatrolPoint();
        //_SoundController.StartExploreSound();
    }
    private IEnumerator Hit()
    {
        _AnimationController.Punch();
        yield return new WaitForSeconds(TimeBeforeHit);
        LevelController.GetInstance().HitPlayer();
        if (Vector3.Distance(transform.position, Player.position) <= 3f && PlayerInArea)
        {
            Chase = false;
            navMesh.isStopped = true;
            StopAllCoroutines();
            StartCoroutine(Hit());
        }
    }
    public void Restart()
    {
        PlayerInArea = false;
        _AnimationController.Restart();
    }
    public void TeleportationPlayerDetection() 
    {
        navMesh.destination = Player.position;
        chasePlayerPosition = Player.position;
        Patrol = false;
    }
}
