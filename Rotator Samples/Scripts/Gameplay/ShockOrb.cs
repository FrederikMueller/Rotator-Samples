using TimeControl;
using UnityEngine;

public class ShockOrb : Projectile
{
    private LayerMask shipsMask;
    public override PoolableObject POType { get => PoolableObject.ShockOrb; }

    protected override void EnemyOwnedProjectilehit(Collider other)
    {
        base.EnemyOwnedProjectilehit(other);

        if (other.gameObject.layer == 9)
        {
            if (other.gameObject.tag == "Projectile" && other.gameObject.GetComponent<Projectile>().owner == this.owner) // check for same owner, dont combo with other enemies
            {
                ShockCombo();
                other.gameObject.GetComponent<Projectile>().FullReset();
            }
            else
            {
                // Contact with non-projectile tagged projectile layer obj
            }
        }
    }
    protected override void PlayerOwnedProjectileHit(Collider other)
    {
        base.PlayerOwnedProjectileHit(other);

        if (other.gameObject.layer == 9)
        {
            if (other.gameObject.CompareTag("Projectile") && other.gameObject.GetComponent<Projectile>().owner?.tag == owner?.tag)
            {
                if (other.gameObject.GetComponent<TCBase>().IsRewinding != timeController.IsRewinding)
                    ShockCombo(true);
                else
                    ShockCombo();

                other.gameObject.GetComponent<Projectile>().FullReset();
            }
            else
            {
                // Contact with non-projectile tagged projectile layer obj
            }
        }
    }
    private void ShockCombo(bool isMixed = false)
    {
        shipsMask = LayerMask.GetMask("Ships");
        string ownersTag = owner.gameObject.tag;

        CallEffectType(isMixed);

        var colliders = Physics.OverlapSphere(gameObject.transform.position, 10, shipsMask);
        foreach (var col in colliders)
        {
            if (!col.gameObject.CompareTag(ownersTag))
            {
                if (timeController.DynamicType == DynamicType.InvertedProjectile)
                    col.gameObject.GetComponent<DefComponent>().Defense.TakeInvertedHit(2000);
                else
                    col.gameObject.GetComponent<DefComponent>().Defense.TakeHit(2000);
            }
        }

        FullReset();
    }

    private void CallEffectType(bool isMixed)
    {
        bool isInverted = timeController.DynamicType == DynamicType.InvertedProjectile;
        if (isMixed)
            GameManager.EffectManager.PlayEffect(EffectID.ShockCombo, gameObject.transform.position, isInverted, true);
        else
            GameManager.EffectManager.PlayEffect(EffectID.ShockCombo, gameObject.transform.position, isInverted, false);
    }
}