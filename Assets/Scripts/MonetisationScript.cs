using System.Collections;
using UnityEngine;
using YG;

public class MonetisationScript : MonoBehaviour
{
    public void ShowAd() 
    {
        //StartCoroutine(AdTimer());
        YandexGame.FullscreenShow();
    }
    //IEnumerator AdTimer() 
    //{
    //    //Debug.Log("Started ad timer");
    //    yield return new WaitForSecondsRealtime(1);
    //    //Debug.Log("Showed ad! ");

    //}

}
