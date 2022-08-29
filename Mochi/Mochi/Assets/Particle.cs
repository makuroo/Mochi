using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private int fungiDmg;
    private PlayerStatus iFrame;
    private int x = 0;
    private void Awake()
    {
        fungiDmg = gameObject.GetComponentInParent<Enemy>().dmg;
        iFrame = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if(other.name == "Player")
        {
            x++;
            if (x == 1)
            {
                other.GetComponent<PlayerStatus>().health -= fungiDmg;
                StartCoroutine(iFrame.iFrame());
            }
        }
    }
}
