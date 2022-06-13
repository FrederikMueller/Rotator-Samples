using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField, Required] private EnemyCore dynamicEnemy;
    private void Start()
    {
        StartCoroutine(ShootingSpecial(6));
    }

    public void RestartBehavior()
    {
        StartCoroutine(ShootingSpecial(6));
    }

    public void ShootSpecial()
    {
        if (dynamicEnemy.timeController.IsRewinding)
            return;

        GameObject orb = dynamicEnemy.GameManager.PoolManager.FetchPooledObject(PoolableObject.ShockOrb);
        orb.transform.position = dynamicEnemy.nose.transform.position;
        orb.GetComponent<Projectile>().Launch(this.gameObject);
    }
    // To be deleted
    public void CreateBomb()
    {
    }

    public IEnumerator ShootingSpecial(float cd)
    {
        while (!dynamicEnemy.timeController.IsRewinding)
        {
            //ShootSpecial();
            CreateBomb();
            yield return new WaitForSeconds(cd);
        }
    }
}