using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VokzalGuySpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] WalkSprites;
    [SerializeField] private Sprite[] IdleSprites;
    [SerializeField] private float AnimationSpeedByTime;
    [SerializeField] private bool idle;
    private int currentIdleSpriteId = 0;
    private int currentWalkSpriteId = 0;
    private PlayerMovementSound _sound;
    //private bool sprite1 = true;
    [SerializeField] private float speed;

    private void Start()
    {
        //idle = true;
        _sound = GetComponent<PlayerMovementSound>();
    }
    public void StartAnimation()
    {
        //StartCoroutine(SpriteAnimation());
        //Debug.Log("Start sprite animation");

    }
    public void StopAnimation()
    {
        //StopAllCoroutines();
        //Debug.Log("Stop sprite animation");
    }
    public void StartWalk()
    {
        idle = false;
    }
    public void StopWalk()
    {
        idle = true;
    }
    public void UpdateSpeedValue(float value)
    {
        speed = value;
    }
    private IEnumerator SpriteAnimation()
    {
        if (idle)
        {
            _spriteRenderer.sprite = IdleSprites[currentIdleSpriteId];
            currentIdleSpriteId++;
            if (currentIdleSpriteId >= IdleSprites.Length)
                currentIdleSpriteId = 0;
        }
        else
        {
            _spriteRenderer.sprite = WalkSprites[currentWalkSpriteId];
            if (currentWalkSpriteId == 2 || currentWalkSpriteId == 5)
                _sound.PlayStepSound();
            currentWalkSpriteId++;
            if (currentWalkSpriteId >= WalkSprites.Length)
                currentWalkSpriteId = 0;
        }
        float time = 0;
        if (speed != 0)
            time = AnimationSpeedByTime / speed;
        yield return new WaitForSeconds(time);
        //Debug.Log($"Sprite Animation {time}");
        StartCoroutine(SpriteAnimation());
    }
}
