using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaLaserAttack : Ability
{
    // Start is called before the first frame update
    public override void Use(Vector3 spawnPos) {
        RaycastHit[] hits = Physics.SphereCastAll(spawnPos, 1f, transform.forward, m_Info.Range);
        foreach(RaycastHit n in hits) {
            if (n.collider.CompareTag("Enemy")) {
                n.collider.GetComponent<EnemyController>().DecreaseHealth(m_Info.Power);
            }
        }
        var emitterShape = cc_PS.shape;
        emitterShape.length = m_Info.Range;
        cc_PS.Play();
    }
}
