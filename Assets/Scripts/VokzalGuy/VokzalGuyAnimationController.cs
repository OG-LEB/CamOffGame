using UnityEngine;

public class VokzalGuyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void UpdateChasingBool(bool value) { animator.SetBool("Chasing",value); }
    public void Punch() { animator.SetTrigger("Punch"); }
    public void Check() { animator.SetTrigger("Checking"); }
    public void Flash() { animator.SetTrigger("Flash"); }
    public void Restart() { animator.SetTrigger("Restart"); }
}
