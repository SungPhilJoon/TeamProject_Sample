using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackBehaviour : AttackBehaviour
{
    public override void ExecuteAttack(GameObject target = null, Transform startPosition = null)
    {
        if (target == null)
        {
            return;
        }

        Vector3 projectilePosition = startPosition?.position ?? transform.position;
        if (effectPrefab)
        {
            GameObject projectileGO = GameObject.Instantiate<GameObject>(effectPrefab, projectilePosition, Quaternion.identity);
            projectileGO.transform.forward = transform.forward;

            Projectile projectile = projectileGO.GetComponent<Projectile>();
            if (projectile)
            {
                projectile.owner = this.gameObject;
                projectile.target = target;
                projectile.attackBehaviour = this;
            }
        }

        calcCoolTime = 0f;
    }
}
