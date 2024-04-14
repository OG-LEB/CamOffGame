using UnityEngine;
using YG;

public class MonetisationScript : MonoBehaviour
{
    //private YandexGame YaGame;
    //private void Start()
    //{
    //    YaGame = GetComponent<YandexGame>();
    //}
    public void ShowAd() 
    {
        YandexGame.FullscreenShow();
    }
}
