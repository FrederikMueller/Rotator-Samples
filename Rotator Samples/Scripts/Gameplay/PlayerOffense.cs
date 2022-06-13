using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

public class PlayerOffense : MonoBehaviour
{
    [SerializeField] public PlayerCore pCore;
    [SerializeField] public PlayerMovement pMove;
    [SerializeField] public PlayerDefense pDef;

    [TabGroup("Values"), SerializeField] public float laserCD;
    [TabGroup("Values"), SerializeField] public int laserAmmoMax;
    [TabGroup("Values"), SerializeField] public int laserAmmoCurrent;

    [TabGroup("Objects", "Internal"), SerializeField] private Transform leftBlaster;
    [TabGroup("Objects", "Internal"), SerializeField] private Transform rightBlaster;
    [TabGroup("Objects", "Internal"), SerializeField] private Transform nose;

    private PoolManager poolManager;

    private bool shootCD;
    [TabGroup("State", "State"), SerializeField] public bool shooting;
    public int Ammo { get => laserAmmoCurrent; }

    private void Awake()
    {
        laserAmmoCurrent = laserAmmoMax;
        poolManager = FindObjectOfType<PoolManager>();

        pCore.ActiveFixedUpdate += InitShooting;
    }

    public void InitShooting()
    {
        if (shooting & !shootCD & laserAmmoCurrent >= 2)
        {
            shootCD = true;
            StartCoroutine(ShootIterator(laserCD));
        }
    }
    public IEnumerator ShootIterator(float interval)
    {
        GameObject projL = poolManager.FetchPooledObject(PoolableObject.Projectile);
        projL.transform.position = leftBlaster.position;
        projL.GetComponent<Projectile>().Launch(gameObject);

        GameObject projR = poolManager.FetchPooledObject(PoolableObject.Projectile);
        projR.transform.position = rightBlaster.position;
        projR.GetComponent<Projectile>().Launch(gameObject);

        laserAmmoCurrent -= 2;
        yield return new WaitForSeconds(interval);
        shootCD = false;
    }
    public void ShootOrb()
    {
        var orb = poolManager.FetchPooledObject(PoolableObject.ShockOrb);
        orb.transform.position = nose.position;
        orb.GetComponent<Projectile>().Launch(this.gameObject);
    }
    public void AbsorbProjectile()
    {
        laserAmmoCurrent++;
        if (laserAmmoCurrent > laserAmmoMax)
            laserAmmoCurrent = laserAmmoMax;
    }
}