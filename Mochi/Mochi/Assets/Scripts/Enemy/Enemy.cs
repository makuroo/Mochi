using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 20;
    public int dmg = 3;

    public bool countDown  = false;

    [SerializeField] private float cdTime = 5;
    [SerializeField] private float moveDistance;
    [SerializeField] private float speed;

    private float leftEdge;
    private float rightEdge;

    private SpriteRenderer sr;
    private Animator anim;
    private ParticleSystem ps;


    [SerializeField] private bool isMovingLeft;
    [SerializeField] private Sprite spirit;

    private BoxCollider2D enemyCollider;

    // Start is called before the first frame update
    void Awake()
    {
        ps = this.GetComponentInChildren<ParticleSystem>();
        sr = transform.GetComponent<SpriteRenderer>();
        enemyCollider = transform.GetComponent<BoxCollider2D>();
        anim = transform.GetComponent<Animator>();
        leftEdge = transform.position.x - moveDistance;
        rightEdge = transform.position.x + moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!(transform.CompareTag("Fungi") || transform.CompareTag("Fungi Spirit")))
        {

            if (isMovingLeft)
            {
                if (transform.position.x > leftEdge)
                {
                    transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
                }
                else
                {
                    isMovingLeft = false;
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                }
            }
            else
            {
                if (transform.position.x < rightEdge)
                {
                    transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
                }
                else
                {
                    isMovingLeft = true;
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                }
            }
        }
        
        if (health <= 0 && (gameObject.CompareTag("Enemy Spirit") || gameObject.CompareTag("Fungi Spirit") || gameObject.CompareTag("Bird Spirit")))
        {
            if (gameObject.CompareTag("Fungi Spirit"))
                Destroy(gameObject.GetComponentInChildren<ParticleSystem>());
            speed = 0;
            isMovingLeft = false;
            sr.sprite = spirit;
            gameObject.tag = "Spirit";
            transform.localScale = new Vector2(1f, 1f);
            enemyCollider.size =new Vector2(0.28f, 0.488f);
            enemyCollider.offset = new Vector2(0f, 0f);
            enemyCollider.isTrigger = true;
            anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Collectibles/spirit") as RuntimeAnimatorController;
        }
        else if(health <= 0 && (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Fungi")|| gameObject.CompareTag("Bird")))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (gameObject.CompareTag("Fungi") || gameObject.CompareTag("Fungi Spirit")))
        {
            Debug.Log("enter");
            countDown = true;
            StartCoroutine("FungiAttack");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (gameObject.CompareTag("Fungi") || gameObject.CompareTag("Fungi Spirit")))
        {
            Debug.Log("exit");
            countDown = false;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private IEnumerator FungiAttack()
    {
        PlayerStatus iFrame = GameObject.FindObjectOfType<PlayerStatus>();
        while(countDown == true)
        {
            yield return new WaitForSeconds(cdTime);
            if (countDown == true)
            {
                AudioManager.Instance.PlayClipByName("Fungi Attack");
                ps.Play();
                AudioManager.Instance.PlayClipByName("Damaged");
                GameObject.FindObjectOfType<PlayerStatus>().health -= transform.GetComponent<Enemy>().dmg;
                StartCoroutine(iFrame.iFrame());
            }
        }
    }
    
}
