using System.Collections;
using UnityEngine;

public class RecSignScript : MonoBehaviour
{
    [SerializeField] private GameObject RecSign;
    private LevelController levelController;
    private bool working;

    private void Start()
    {
        levelController = LevelController.GetInstance();
        RecSign.SetActive(true);
        working = true;
        StartCoroutine(Rec());
    }
    private void Update()
    {
        if (levelController.GetPauseState()) 
        {
            if (working)
            {
                StopAllCoroutines();
                working = false;
            }
        }
        else
        {
            if (!working)
            {
                StartCoroutine(Rec());
                working = true;
            }
        }
    }
    private IEnumerator Rec()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            RecSign.SetActive(!RecSign.activeSelf);
        }
    }
}
