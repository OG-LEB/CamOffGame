using UnityEngine;

public class Note : MonoBehaviour
{
    //Settings
    private bool gotId = false;
    private int id;
    private NoteDataScript data;
    //Scan
    private LocationScanSystem locationScanSystem;
    private Transform PlayerTransform;
    private MeshRenderer mesh;
    private bool fadeout = false;
    //private float colorFadeVal = 0;
    private float scaleFadeVal = 0;
    private void Start()
    {
        data = NoteDataScript.GetInstance();
        locationScanSystem = LocationScanSystem.GetInstance();
        mesh = GetComponentInChildren<MeshRenderer>();
    }
    public void Open()
    {
        if (!gotId)
        {
            id = data.GetNoteId();
            gotId = true;
        }
        data.OpenNoteById(id);
    }
    public void Restart()
    {
        gotId = false;
        id = 0;
        //colorFadeVal = 0;
        scaleFadeVal = 0;
        if (mesh == null)
            mesh = GetComponentInChildren<MeshRenderer>();
        mesh.materials[1].color = new Color(1, 1, 1, 1);
        mesh.materials[1].SetFloat("_Scale", 0);
        fadeout = false;
    }
    private void FixedUpdate()
    {
        if (fadeout)
        {
            //mesh.materials[1].color = new Color(0, colorFadeVal, 0, 1);
            mesh.materials[1].SetFloat("_Scale", scaleFadeVal);
            //colorFadeVal -= 0.001f;
            if (scaleFadeVal > 1)
                scaleFadeVal -= 0.001f;
            else
                scaleFadeVal = 1f;
            //if (colorFadeVal <= 0 && scaleFadeVal <= 0)
            //{
            //    fadeout = false;
            //}
            if (scaleFadeVal <= 0)
            {
                fadeout = false;
            }
        }
    }
    public void Scan()
    {
        if (PlayerTransform == null)
        {
            PlayerTransform = LevelController.GetInstance().GetPlayerTransform();
        }
        float distance = Vector3.Distance(transform.position, PlayerTransform.position);
        float procent = distance / GetComponentInChildren<SphereCollider>().radius;
        //colorFadeVal = 0.25f + (1 - (1 * procent));
        //scaleFadeVal = 1.0f + 0.3f * (1 * procent);
        scaleFadeVal = 1.1f + 0.1f * (1 * procent);
        fadeout = true;
    }

}
