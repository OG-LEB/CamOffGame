using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class VokzalGuyScript : MonoBehaviour
{
    [Header("Links")]
    //private VokzalGuySpriteController SpriteController;
    private LevelController _LevelController;
    [SerializeField] private SoundController _SoundController;
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
    [SerializeField] bool walk = true;
    [SerializeField] private bool SeePlayer;
    [SerializeField] private float CheckTime;
    [SerializeField] private float TimeBeforeHit;
    [SerializeField] private Transform[] PatrolPoints;
    [SerializeField] private Transform currentPatrolPoint;
    [SerializeField] private Transform previousPatrolPoint;
    [SerializeField] private Vector3 chasePlayerPosition;
    [SerializeField] private bool Patrol;
    [SerializeField] private bool findingNewPatrolPoint = false;
    [Header("Settings Scene")]
    [SerializeField] private Transform Player;
    
    

    public bool GetSeePlayerState() { return SeePlayer; }
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        navMesh = GetComponent<NavMeshAgent>();
        walk = false;
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
    private void FixedUpdate()
    {
        if (!_LevelController.GetPauseState())
        {
            navMesh.speed = currentSpeed;
            if (SeePlayer)
            {
                navMesh.destination = Player.position;
                chasePlayerPosition = Player.position;
                Patrol = false;
            }
            if (Patrol)
            {
                Debug.Log("Patrol area");
                currentSpeed = PatrolSpeed;
                if ((currentPatrolPoint == null || Vector3.Distance(transform.position, currentPatrolPoint.position) < 1f) && !findingNewPatrolPoint)
                {
                    findingNewPatrolPoint = true;
                    NewPatrolPoint();
                }

            }
            if (walk && !isGetShot)
            {
                if (Vector3.Distance(transform.position, chasePlayerPosition) < 2.5f && !SeePlayer)
                {
                    Debug.Log("Start Checking");
                    walk = false;
                    StartCoroutine(CheckArea());
                }
                if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
                {
                    walk = false;
                    navMesh.isStopped = true;
                    StartCoroutine(Hit());
                }
            }
            else if (!isGetShot)
            {
                if (SeePlayer)
                {
                    transform.LookAt(Player.position);
                }
                if (Vector3.Distance(transform.position, Player.position) > 3f && SeePlayer)
                {
                    walk = true;
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
            walk = true;
            SeePlayer = true;
            _AnimationController.UpdateChasingBool(true);
            StopAllCoroutines();
            _SoundController.StartChaseSound();
            _VoiceScript.StartTalking();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            SeePlayer = false;
            _VoiceScript.StopTalking();
        }
    }
    public void StartMotion()
    {
        walk = true;
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
        if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
        {
            walk = false;
            navMesh.isStopped = true;
            StopAllCoroutines();
            StartCoroutine(Hit());
        }
    }
    private IEnumerator CheckArea()
    {
        _SoundController.StartExploreSound();
        _AnimationController.Check();
        _AnimationController.UpdateChasingBool(false);
        yield return new WaitForSeconds(CheckTime);
        Patrol = true;
        NewPatrolPoint();
    }
    private IEnumerator Hit()
    {
        _AnimationController.Punch();
        yield return new WaitForSeconds(TimeBeforeHit);
        LevelController.GetInstance().HitPlayer();
        if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
        {
            walk = false;
            navMesh.isStopped = true;
            StopAllCoroutines();
            StartCoroutine(Hit());
        }
    }
    public void Restart()
    {
        SeePlayer = false;
        _AnimationController.Restart();
    }
}
