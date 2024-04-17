using System.Collections;
using UnityEngine;
using YG;

public class MonetisationScript : MonoBehaviour
{
    public void ShowAd() 
    {
        StartCoroutine(AdTimer());
    }
    IEnumerator AdTimer() 
    {
        //Debug.Log("Started ad timer");
        yield return new WaitForSecondsRealtime(1);
        YandexGame.FullscreenShow();
        //Debug.Log("Showed ad! ");

    }

}
