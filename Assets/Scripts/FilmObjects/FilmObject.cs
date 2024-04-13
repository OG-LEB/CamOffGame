using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore;

public class FilmObject : MonoBehaviour
{
    private bool isFilmedAlready = false;
    //OutlineLogit
    private MeshRenderer mesh;
    private bool playerNear = false;
    private Transform playerTransform;
    //FadeOut
    private bool fadeout = false;
    private float colorFadeVal = 0;
    private float scaleFadeVal = 0;
    [Header("VokzalGuy Spawn")]
    [SerializeField] private Transform[] VokzalGuySpawnPoints;
    private LocationScanSystem locationScanSystem;
    private LevelController levelController;
    [SerializeField] private int OutlineMaterialId;
    private AudioSource sound;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.materials[OutlineMaterialId].color = new Color(0, 1, 0, 1);
        mesh.materials[OutlineMaterialId].SetFloat("_Scale", 0);
        locationScanSystem = LocationScanSystem.GetInstance();
        levelController = LevelController.GetInstance();
        sound = GetComponent<AudioSource>();
    }
    public void Film()
    {
        if (!isFilmedAlready && playerNear)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) < 10)
            {
                LevelController.GetInstance().NewFilmObject();
                isFilmedAlready = true;
                mesh.materials[OutlineMaterialId].color = new Color(0, 0, 0, 0);
                mesh.materials[OutlineMaterialId].SetFloat("_Scale", 0);
                SpawnVokzalGuy();
                sound.Play();
            }
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerNear = true;
            playerTransform = col.transform;
            locationScanSystem.AddFilmObject(this);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerNear = false;
            playerTransform = null;
            locationScanSystem.RemoveFilmObject(this);
        }
    }
    private void FixedUpdate()
    {
        if (fadeout)
        {
            mesh.materials[OutlineMaterialId].color = new Color(0, colorFadeVal, 0, 1);
            mesh.materials[OutlineMaterialId].SetFloat("_Scale", scaleFadeVal);
            colorFadeVal -= 0.001f;
            if (scaleFadeVal > 1)
                scaleFadeVal -= 0.001f;
            if (scaleFadeVal < 1)
                scaleFadeVal = 1;
            if (colorFadeVal <= 0 && scaleFadeVal <= 0)
            {
                fadeout = false;
            }
        }
        if (isFilmedAlready)
        {
            fadeout = false;
            mesh.materials[OutlineMaterialId].color = new Color(0, 0, 0, 0);
            mesh.materials[OutlineMaterialId].SetFloat("_Scale", 0);
        }
    }
    public void Restart()
    {
        isFilmedAlready = false;
        playerNear = false;
        colorFadeVal = 0;
        scaleFadeVal = 0;
        if (mesh == null)
            mesh = GetComponent<MeshRenderer>();
        mesh.materials[OutlineMaterialId].color = new Color(0, 1, 0, 1);
        mesh.materials[OutlineMaterialId].SetFloat("_Scale", 0);
        fadeout = false;
    }
    public void Scan()
    {
        if (!isFilmedAlready)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            float procent = distance / GetComponent<SphereCollider>().radius;
            colorFadeVal = (1 - (1 * procent));
            if (colorFadeVal < 0.25f)
            {
                colorFadeVal = 0.25f;
            }
            scaleFadeVal = 1.1f + 0.1f * (1 * procent);

            fadeout = true;
        }
    }
    private void SpawnVokzalGuy()
    {
        int value = UnityEngine.Random.Range(0, 100);
        if (value >= 70 && !levelController.GetVokzalGuySeePlayerState() && levelController.GetFilmObjectsCount() > 1)
        {
            int spawnid = UnityEngine.Random.Range(0, VokzalGuySpawnPoints.Length);
            levelController.TeleportVokzalGuy(VokzalGuySpawnPoints[spawnid].position);
        }
        else if (value < 70 && value > 30)
        {
            int spawnid = UnityEngine.Random.Range(0, VokzalGuySpawnPoints.Length);
            levelController.SpawnScarySound(VokzalGuySpawnPoints[spawnid].position);
        }

    }
}
