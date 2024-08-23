using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSkill : MonoBehaviour
{   
    [SerializeField] public float nextAttack;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private float attackCooldown;
    private Player playerStats;
    private Animator anim;
    public Image skillImage;
    [SerializeField] private AudioManager manager;

    private void Awake()
    {
        attackCooldown = 0;
        playerStats = transform.GetComponent<Player>();
        anim = transform.GetComponent<Animator>();
        manager = GameObject.FindObjectOfType<AudioManager>();
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3)
            skillImage.fillAmount = (10f - attackCooldown) / nextAttack;
        if (SceneManager.GetActiveScene().buildIndex == 3 && playerStats.passed == true)
        {
            if (Input.GetKeyDown(KeyCode.E) && attackCooldown == 0 &&  playerStats.Spirits>0)
            {
                Debug.Log("skill");
                playerStats.Spirits-=1;
                attackCooldown += nextAttack;
                anim.SetBool("isSkill", true);
                manager.PlayClipByName("skill");
            }
        }
        if (attackCooldown > 0)
        {
            if ((attackCooldown - Time.deltaTime) < 0)
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
        Attack();
        anim.SetBool("isSkill", false);
    }
}
