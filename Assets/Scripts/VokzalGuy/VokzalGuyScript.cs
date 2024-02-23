using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class VokzalGuyScript : MonoBehaviour
{
    [Header("Links")]
    //private VokzalGuySpriteController SpriteController;
    private LevelController _LevelController;
    [SerializeField] private SoundController _SoundController;
    private VokzalGuyAnimationController _AnimationController;
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
    [SerializeField] private bool Patrol;
    [Header("Settings Scene")]
    [SerializeField] private Transform Player;


    public bool GetSeePlayerState() { return SeePlayer; }
    private void Start()
    {
        //SpriteController = GetComponent<VokzalGuySpriteController>();
        characterController = GetComponent<CharacterController>();
        navMesh = GetComponent<NavMeshAgent>();
        walk = false;
        currentSpeed = WalkSpeed;
        //SpriteController.UpdateSpeedValue(currentSpeed / WalkSpeed);
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
    }
    private void FixedUpdate()
    {
        if (!_LevelController.GetPauseState())
        {
            navMesh.speed = currentSpeed;
            if (SeePlayer)
            {
                navMesh.destination = Player.position;
                Patrol = false;
            }
            if (Patrol)
            {
                currentSpeed = PatrolSpeed;
                if (currentPatrolPoint == null || navMesh.remainingDistance < 0.5f)
                {
                    NewPatrolPoint();
                }

            }
            if (walk && !isGetShot)
            {
                if (navMesh.remainingDistance < 0.5f && !SeePlayer)
                {
                    //SpriteController.StopWalk();
                    walk = false;
                    StartCoroutine(CheckArea());
                }
                if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
                {
                    walk = false;
                    //SpriteController.StopWalk();
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
                    //SpriteController.StartWalk();
                    walk = true;
                    //navMesh.Resume();
                    navMesh.isStopped = false;
                    StopAllCoroutines();
                    currentSpeed = WalkSpeed;
                    //Debug.Log("Continue...");
                }
            }
            //SpriteController.UpdateSpeedValue(currentSpeed / WalkSpeed);
        }

    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            walk = true;
            SeePlayer = true;
            _AnimationController.UpdateChasingBool(true);
            //SpriteController.StartWalk();
            StopAllCoroutines();
            _SoundController.StartChaseSound();
            //Debug.Log("Player on trigger");
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            SeePlayer = false;
            //Debug.Log("Player goes away");
        }
    }
    public void StartMotion()
    {
        walk = true;
        Patrol = true;
        _AnimationController.UpdateChasingBool(false);
        //SeePlayer = true;
        //SpriteController.StartAnimation();
        //SpriteController.StartWalk();
    }
    public void Dissapear()
    {
        //SpriteController.StopAnimation();
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
        //SpriteController.UpdateSpeedValue(currentSpeed / Speed);
        //SpriteController.StopAnimation();
        yield return new WaitForSeconds(GetShotTime);
        //SpriteController.StartAnimation();
        isGetShot = false;
        //SpriteController.StartAnimation(); // Fix
        while (currentSpeed < WalkSpeed)
        {
            //SpriteController.UpdateSpeedValue(currentSpeed / WalkSpeed);
            currentSpeed += SpeedRegenerationStep;
        }
        currentSpeed = WalkSpeed;
        //SpriteController.UpdateSpeedValue(currentSpeed / WalkSpeed);
        if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
        {
            walk = false;
            //SpriteController.StopWalk();
            navMesh.isStopped = true;
            StopAllCoroutines();
            StartCoroutine(Hit());
        }
    }
    private IEnumerator CheckArea()
    {
        _SoundController.StartExploreSound();
        _AnimationController.Check();
        yield return new WaitForSeconds(CheckTime);
        //LevelController.GetInstance().DisappearVokzalGuy();
        //Continue
        Patrol = true;
        _AnimationController.UpdateChasingBool(false);
    }
    private IEnumerator Hit()
    {
        _AnimationController.Punch();
        yield return new WaitForSeconds(TimeBeforeHit);
        LevelController.GetInstance().HitPlayer();
        if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
        {
            walk = false;
            //SpriteController.StopWalk();
            //navMesh.Stop();
            navMesh.isStopped = true;
            //Debug.Log("Be ready for hit!");
            //Cc
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
