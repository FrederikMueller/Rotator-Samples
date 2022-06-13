using Sirenix.OdinInspector;
using UnityEngine;

// GO with normal enemy components and this as an offensive strategy.
// Add 2 models/hitboxes. One for the core bomb that is killable and
// Bomb spawner => bomb => bomb effect?
// effect is covered by its own TC and logic. Effects gets removed via event.
public class BombEffect : MonoBehaviour, IPoolable
{
    [InfoBox("Needs TCScale X Script")]
    [SerializeField] public float GrowthRate;
    private TCScaleX timeController;

    public PoolableObject POType => PoolableObject.BombEffect;

    public void Awake()
    {
        timeController = GetComponent<TCScaleX>();
        timeController.ReachedBeginning += Despawn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<PlayerDefense>().TakeHit(9999);
    }

    private void FixedUpdate()
    {
        if (!timeController.IsRewinding)
        {
            gameObject.transform.localScale *= GrowthRate;
            timeController.ProgressForwardsInHistory();
        }
        else
            timeController.ProgressBackwardsInHistory();
    }
    public void OnSpawn()
    {
        Debug.Log("registered");
        timeController.RegisterObject();
    }

    public void Despawn()
    {
        timeController.ResetIndexes();
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        GrowthRate = 1;
        FindObjectOfType<PoolManager>().ReturnPoolableObject(gameObject);
    }
}