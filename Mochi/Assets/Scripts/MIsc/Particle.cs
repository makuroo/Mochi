using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private int fungiDmg;
    private Player iFrame;
    private void Awake()
    {
        fungiDmg = gameObject.GetComponentInParent<Enemy>().dmg;
    }
    private void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent(out Player player)&& !player.damaged)
        {
            player.damaged = true;
            player.OnTakeDamage?.Invoke(fungiDmg);
        }
    }
}
