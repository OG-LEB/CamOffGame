using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
    [SerializeField] private float ChaseSpeed;
    [SerializeField] private float PatrolSpeed;
    [SerializeField] private float GetShotSpeed;
    [SerializeField] private float GetShotTime;
    [SerializeField] private float SpeedRegenerationStep;
    [SerializeField] private float currentSpeed;
    private bool isGetShot;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] private bool Chase = true;
    [SerializeField] private bool PlayerInArea;
    [SerializeField] private bool Patrol;
    [SerializeField] private bool isTalking = false;
    [SerializeField] private bool HittingPlayer = false;
    [SerializeField] private bool NavMeshIsStopped;
    [SerializeField] private bool SeePlayer;
    [SerializeField] private float CheckTime;
    [SerializeField] private float TimeBeforeHit;
    [SerializeField] private Transform[] PatrolPoints;
    [SerializeField] private Transform currentPatrolPoint;
    [SerializeField] private Transform previousPatrolPoint;
    [SerializeField] private Vector3 chasePlayerPosition;
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
        currentSpeed = ChaseSpeed;
        _LevelController = LevelController.GetInstance();
        _AnimationController = GetComponent<VokzalGuyAnimationController>();
    }
    private void NewPatrolPoint()
    {
        previousPatrolPoint = currentPatrolPoint;
        int id = UnityEngine.Random.Range(0, PatrolPoints.Length);
        currentPatrolPoint = PatrolPoints[id];
        if (previousPatrolPoint != null)
        {
            if (currentPatrolPoint == previousPatrolPoint)
            {
                while (currentPatrolPoint == previousPatrolPoint)
                {
                    id = UnityEngine.Random.Range(0, PatrolPoints.Length); ;
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
            NavMeshIsStopped = navMesh.isStopped;
            navMesh.speed = currentSpeed;
            //Если игрок в зоне видимости
            if (PlayerInArea)
            {
                float distanceToTarget = Vector3.Distance(transform.position, Player.position);
                Vector3 directionToTarget = (Player.position - transform.position).normalized;
                //Если рейкаст не сработал
                if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), directionToTarget, distanceToTarget, LocationLayer))
                {
                    if (SeePlayer)
                    {
                        StartCoroutine(LastSecPathToPlayer());
                    }
                    SeePlayer = false;
                }
                //Если рейкаст сработал
                else
                {
                    navMesh.destination = Player.position;
                    chasePlayerPosition = Player.position;
                    Patrol = false;
                    if (!SeePlayer)
                    {
                        _VoiceScript.PlayFoundTalk();
                    }
                    SeePlayer = true;
                    if (!Chase)
                    {
                        Chase = true;
                        _AnimationController.UpdateChasingBool(true);
                        _SoundController.StartChaseSound();

                    }
                    if (!isTalking)
                    {
                        _VoiceScript.StartChaseTalking();
                        isTalking = true;
                    }
                }
                if (distanceToTarget < 3)
                {
                    SeePlayer = true;
                }
            }
            //Если игрок вне зоны видимости то мы его не видим
            else
            {
                if (SeePlayer)
                {
                    StartCoroutine(LastSecPathToPlayer());
                }
                SeePlayer = false;
            }
            //Патрулирование
            if (Patrol)
            {
                currentSpeed = PatrolSpeed;
                if ((currentPatrolPoint == null || Vector3.Distance(transform.position, currentPatrolPoint.position) < 1f) && !findingNewPatrolPoint)
                {
                    findingNewPatrolPoint = true;
                    NewPatrolPoint();
                }
                //Debug.Log("Patrol Area");
            }
            //Погоня
            if (Chase && !isGetShot && !HittingPlayer)
            {
                currentSpeed = ChaseSpeed;
                //Если мы дошли до последней точки и не видим игрока
                if (Vector3.Distance(transform.position, chasePlayerPosition) < 2.5f && !SeePlayer)
                {
                    Chase = false;
                    _AnimationController.UpdateChasingBool(false);
                    StartCoroutine(CheckArea());
                    //Debug.Log("Checking Area");
                }
                //Если мы рядом с игроком то бьём его
                if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer && !HittingPlayer)
                {
                    HittingPlayer = true;
                    Chase = false;
                    transform.LookAt(Player.position);
                    _AnimationController.UpdateChasingBool(false);
                    navMesh.isStopped = true;
                    StartCoroutine(Hit());
                    //Debug.Log("Hitting player");
                }
                //Если игрок в зоне видимости, но не рядом
                if (Vector3.Distance(transform.position, Player.position) > 3f && SeePlayer)
                {
                    if (!Chase)
                    {
                        Chase = true;
                        _AnimationController.UpdateChasingBool(true);
                    }
                    HittingPlayer = false;
                    navMesh.isStopped = false;
                    StopAllCoroutines();
                    currentSpeed = ChaseSpeed;
                    //Debug.Log("Chasing player");
                }
            }
            //else if (!isGetShot && !HittingPlayer)
            //{

            //}

        }
        IEnumerator LastSecPathToPlayer()
        {
            //Debug.Log("Started last sec");
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.25f);
                navMesh.destination = Player.position;
                chasePlayerPosition = Player.position;
            }
            //Debug.Log("End of last sec");
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerInArea = true;
            StopAllCoroutines();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerInArea = false;
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
        _VoiceScript.PlayGetShotTalk();
        StartCoroutine(GetShotC());
        _AnimationController.Flash();
        Console.Clear();
        //Debug.Log("Get Shot");
    }
    private IEnumerator GetShotC()
    {
        isGetShot = true;
        HittingPlayer = false;
        currentSpeed = GetShotSpeed;
        yield return new WaitForSeconds(GetShotTime);
        isGetShot = false;
        while (currentSpeed < ChaseSpeed)
        {
            currentSpeed += SpeedRegenerationStep;
        }
        currentSpeed = ChaseSpeed;
    }
    private IEnumerator CheckArea()
    {
        _AnimationController.Check();
        Chase = false;
        isTalking = false;
        _VoiceScript.StopChaseTalking();
        _VoiceScript.PlayLostTalk();
        _AnimationController.UpdateChasingBool(false);
        yield return new WaitForSeconds(CheckTime);
        Patrol = true;
        NewPatrolPoint();
        _VoiceScript.PlayChasingVoice();
        navMesh.isStopped = false;
        _SoundController.StartExploreSound();
        _VoiceScript.PlayLostTalk();
    }
    private IEnumerator Hit()
    {
        //Debug.Log("HIT Ienumenator start");
        _AnimationController.Punch();
        _VoiceScript.PlayHitTalk();
        yield return new WaitForSeconds(TimeBeforeHit);
        LevelController.GetInstance().HitPlayer();
        //Debug.Log("Player hitted!!!");
        HittingPlayer = false;
        navMesh.isStopped = false;
    }
    public void Restart()
    {
        PlayerInArea = false;
        _AnimationController.Restart();
        isTalking = false;
        HittingPlayer = false;
        Chase = false;
        currentPatrolPoint = null;
        Patrol = true;
    }
    public void TeleportationPlayerDetection()
    {
        navMesh.destination = Player.position;
        chasePlayerPosition = Player.position;
        Patrol = false;
    }
    public bool GetPatrolBool() { return Patrol; }
}
