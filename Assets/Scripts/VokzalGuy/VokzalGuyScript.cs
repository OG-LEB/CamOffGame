using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class VokzalGuyScript : MonoBehaviour
{
    [Header("Links")]
    private VokzalGuySpriteController SpriteController;
    private LevelController _LevelController;
    [SerializeField] private SoundController _SoundController;
    [Space]
    [Header("Settings")]
    [SerializeField] private float Speed;
    [SerializeField] private float GetShotSpeed;
    [SerializeField] private float GetShotTime;
    [SerializeField] private float SpeedRegenerationStep;
    private float currentSpeed;
    private bool isGetShot;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private NavMeshAgent navMesh;
    [SerializeField] bool walk = true;
    [SerializeField] private bool SeePlayer;
    [SerializeField] private float TimeBeforeDisappear;
    [SerializeField] private float TimeBeforeHit;
    [Header("Settings Scene")]
    [SerializeField] private Transform Player;
    //private Transform target;
    //private Rigidbody rigidbody;


    private void Start()
    {
        SpriteController = GetComponent<VokzalGuySpriteController>();
        characterController = GetComponent<CharacterController>();
        navMesh = GetComponent<NavMeshAgent>();
        walk = false;
        currentSpeed = Speed;
        SpriteController.UpdateSpeedValue(currentSpeed / Speed);
        _LevelController = LevelController.GetInstance();
    }
    private void FixedUpdate()
    {
        if (!_LevelController.GetPauseState())
        {
            navMesh.speed = currentSpeed;
            if (SeePlayer)
            {
                navMesh.destination = Player.position;
            }
            if (walk && !isGetShot)
            {
                if (navMesh.remainingDistance < 0.5f && !SeePlayer)
                {
                    SpriteController.StopWalk();
                    walk = false;
                    StartCoroutine(CheckArea());
                }
                if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
                {
                    walk = false;
                    SpriteController.StopWalk();
                    navMesh.isStopped = true;
                    StartCoroutine(Hit());
                }
            }
            else if(!isGetShot)
            {
                if (SeePlayer)
                {
                    transform.LookAt(Player.position);
                }
                if (Vector3.Distance(transform.position, Player.position) > 3f && SeePlayer)
                {
                    SpriteController.StartWalk();
                    walk = true;
                    //navMesh.Resume();
                    navMesh.isStopped = false;
                    StopAllCoroutines();
                    currentSpeed = Speed;
                    //Debug.Log("Continue...");
                }
            }
            SpriteController.UpdateSpeedValue(currentSpeed / Speed);
        }

    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            walk = true;
            SeePlayer = true;
            SpriteController.StartWalk();
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
    public void Spawn()
    {
        walk = true;
        SeePlayer = true;
        SpriteController.StartAnimation();
        SpriteController.StartWalk();
    }
    public void Dissapear()
    {
        SpriteController.StopAnimation();
    }
    public void GetShot()
    {
        StopAllCoroutines();
        StartCoroutine(GetShotC());
    }
    private IEnumerator GetShotC()
    {
        isGetShot = true;
        currentSpeed = GetShotSpeed;
        //SpriteController.UpdateSpeedValue(currentSpeed / Speed);
        SpriteController.StopAnimation();
        yield return new WaitForSeconds(GetShotTime);
        SpriteController.StartAnimation();
        isGetShot = false;
        //SpriteController.StartAnimation(); // Fix
        while (currentSpeed < Speed)
        {
            SpriteController.UpdateSpeedValue(currentSpeed / Speed);
            currentSpeed += SpeedRegenerationStep;
        }
        currentSpeed = Speed;
        SpriteController.UpdateSpeedValue(currentSpeed / Speed);
        if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
        {
            walk = false;
            SpriteController.StopWalk();
            navMesh.isStopped = true;
            StopAllCoroutines();
            StartCoroutine(Hit());
        }
    }
    private IEnumerator CheckArea()
    {
        _SoundController.StartExploreSound();
        yield return new WaitForSeconds(TimeBeforeDisappear);
        LevelController.GetInstance().DisappearVokzalGuy();
    }
    private IEnumerator Hit() 
    {
        yield return new WaitForSeconds(TimeBeforeHit);
        LevelController.GetInstance().HitPlayer();
        if (Vector3.Distance(transform.position, Player.position) <= 3f && SeePlayer)
        {
            walk = false;
            SpriteController.StopWalk();
            //navMesh.Stop();
            navMesh.isStopped = true;
            //Debug.Log("Be ready for hit!");
            //Cc
            StopAllCoroutines();
            StartCoroutine(Hit());
        }
    }
}
