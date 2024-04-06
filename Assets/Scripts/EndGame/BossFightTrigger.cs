using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    private LevelController _levelController;
    [SerializeField] private VokzalGuyScript vokzalGuy;
    private void Start()
    {
        _levelController = LevelController.GetInstance();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (vokzalGuy.GetPatrolBool())
            {
                LevelController.GetInstance().BossFight(false);
                gameObject.SetActive(false);
            }
            else
            {
                LevelController.GetInstance().BossFight(true);
                gameObject.SetActive(false);
            }

        }
    }
}
