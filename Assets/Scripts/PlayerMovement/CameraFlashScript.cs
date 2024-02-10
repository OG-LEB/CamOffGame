using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraFlashScript : MonoBehaviour
{
    [SerializeField] private Image FlashImage;
    [SerializeField] private float FlashUpStep;
    [SerializeField] private float FlashDownStep;
    [SerializeField] private float alphaMaxFalue;
    [SerializeField] private float alpha;
    private bool isFlashing;
    private bool goUp;

    private void Start()
    {
        alpha = 0;
    }
    public void Flash() 
    {
        isFlashing = true;
        goUp = true;
    }
    private void FixedUpdate()
    {
        if (isFlashing) 
        {
            if (goUp)
            {
                alpha += FlashUpStep;
                if (alpha >= alphaMaxFalue)
                {
                    alpha = alphaMaxFalue;
                    goUp = false;
                }
                FlashImage.color = new Color(0, 0, 0, alpha);
            }
            if (!goUp)
            {
                alpha -= FlashDownStep;
                if (alpha <= 0)
                {
                    alpha = 0;
                    isFlashing = false;
                }
                FlashImage.color = new Color(0, 0, 0, alpha);
            }
        }
    }

}
