using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitterArm : MonoBehaviour
{
    float hitCD = 1f;
    float lastHit;

    private void OnTriggerEnter(Collider other)
    {
        EnemyBehaviour enemy = transform.GetComponentInParent<EnemyBehaviour>();

        if (other.GetComponent<PlayerBehaviour>())
        {
            var p = other.GetComponent<PlayerBehaviour>();
            if ((Time.time - lastHit < hitCD) || p.IsDead())
                return;

            p.TakeDamage(enemy.Damage);
            lastHit = Time.time;
        }
    }
}
