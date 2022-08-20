using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health = 480;
    private int clawDamage = 10;
    public int projectileDamage = 8;
    [SerializeField] private bool attack = false;
    public int basicAttackCount = 0;
    [SerializeField] GameObject projectile;
    public bool special = false;
    [SerializeField] private PlayerStatus player;
    private AudioSource source;
    public List<AudioClip> SFX = new List<AudioClip> {};
    private bool played = false;
    [SerializeField] private float fireRate = 5f;
    private float nextFire;
    [SerializeField] private Slider healthBar;

    [SerializeField] private Animator anim;

    private void Start()
    {
        nextFire = 5f;
        player = GameObject.FindObjectOfType<PlayerStatus>();
        source = transform.GetComponent<AudioSource>();
        healthBar = transform.GetChild(0).GetChild(0).GetComponent<Slider>();
    }
    // Start is called before the first frame update
    void Awake()
    {
        anim = transform.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;
        if (player != null || player.health > 0)
        {
            CheckTimeToFire();
        }
        if (health <= 0)
        {
            if(played == false)
            {
                source.clip = SFX[3];
                source.PlayOneShot(source.clip);
                played = true;
            }
            StopCoroutine(player.iFrame());
            Destroy(gameObject, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
            attack = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            attack = false;
    }

    private void Attack()
    {
        if (attack && player!=null)
        {
            anim.SetBool("attack", true);
            if (attack)
            {
                player.health -= clawDamage;
                StartCoroutine(player.iFrame());
            }
        }

    }

    public void EndAttack()
    {
        anim.SetBool("attack", false);
    }

    private void CheckTimeToFire()
    {
        if (Time.timeSinceLevelLoad > nextFire && player!=null){
            if (attack)
            {
                anim.SetBool("attack", true);
                source.clip = SFX[0];
                source.Play();
                Attack();
                basicAttackCount++;
            }
            else
            {
                anim.SetBool("shoot", true);
                source.clip = SFX[1];
                source.Play();
                Instantiate(projectile, transform.position, Quaternion.identity);
                basicAttackCount++;
            }
            anim.SetInteger("BasicAttackCount", basicAttackCount);
            nextFire = Time.timeSinceLevelLoad + fireRate;
        }
    }

    public void EndShoot()
    {
        anim.SetBool("shoot", false);
    }

    public void EndSpecialAnim()
    {
        basicAttackCount = 0;
        anim.SetInteger("BasicAttackCount", basicAttackCount);
        special = true;
    }

    public void PlaySFX()
    {
        source.clip = SFX[2];
        source.PlayOneShot(source.clip);
        health += 30;
    }
}
