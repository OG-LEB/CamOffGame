using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] private float Speed;
    bool walk = true;
    [Header("Settings Scene")]
    [SerializeField] private Transform Player;
    [SerializeField] private Transform VapeStore;
    private Transform target;
    [SerializeField] private ParticleSystem VEIP;
    private float ZHIZHA = 100f;
    bool canVape = true;
    //private CharacterController characterController;

    private void Start()
    {
        ZHIZHA = Random.Range(25, 100f);
        target = Player.transform;
        //characterController = GetComponent<CharacterController>();
        if(ZHIZHA > 60) VEIP.Play();
        StartCoroutine(NapasCHeck());
        walk = true;
        Speed = 4f;
    }
    private void FixedUpdate()
    {
        if (walk)
        {
            transform.LookAt(target);
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            //characterController.Move(Vector3.forward * Speed * Time.deltaTime);
        }
    }
    private void NAPAS() 
    {
        //VEIP.transform.position = transform.position + new Vector3(0,1.3f,0);
        if (ZHIZHA >= 25f)
        {
            VEIP.Play();
            ZHIZHA -= 25f;
            walk = false;
        }
        else
        {
            canVape = false;
            Speed = 10f;
            //LARYOK
            target = VapeStore.transform;
        }
    }

    IEnumerator NapasCHeck() 
    {
        int second = Random.Range(5,10);
        yield return new WaitForSeconds(second);
        if(canVape) NAPAS();
        yield return new WaitForSeconds(2f);
        walk = true;
        StartCoroutine(NapasCHeck());
    }

    public void zapravka() 
    {
        ZHIZHA = 100f;
        canVape = true;
        target = Player.transform;
        VEIP.Play();
        Speed = 4f;
    }
}
