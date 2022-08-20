using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSkill : MonoBehaviour
{   
    [SerializeField] public float nextAttack;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private float attackCooldown;
    private PlayerStatus playerStats;
    private Animator anim;
 //   [SerializeField] private AudioManager manager;

    private void Awake()
    {
        attackCooldown = 0;
        playerStats = transform.GetComponent<PlayerStatus>();
        anim = transform.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (Input.GetKeyDown(KeyCode.E) && attackCooldown == 0)
            {
                anim.SetBool("isSkill", true);
                Attack();
                //manager.PlayShotSound();
            }
        }
        if (attackCooldown > 0)
        {
            if((attackCooldown-Time.deltaTime) < 0)
            {
                attackCooldown = 0;
            }
            else
            {
                attackCooldown -= Time.deltaTime;
            }
        }
    }

    void Attack()
    {
        bullets[FindBullet()].transform.position = firePoint.position;
        bullets[FindBullet()].GetComponent<Bullet>().SetDirection(transform.localRotation.eulerAngles.y);
        attackCooldown += nextAttack;
    }
    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return i;
            }
        }

        return 0;
    }

    public void SkillEnd()
    {
        anim.SetBool("isSkill", false);
    }
}
