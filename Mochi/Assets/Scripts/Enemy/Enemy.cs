using System;
using System.Collections;
using UnityEngine.SceneManagement;
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
    [SerializeField] private AudioSource audioSource;

    private BoxCollider2D enemyCollider;
    [SerializeField] public BoxCollider2D attackArea;

    private bool attack =true;
    private bool play = true;

    public static Action<int> onLastTotemSpiritIncrease;

    // Start is called before the first frame update
    void Awake()
    {
        ps = this.GetComponentInChildren<ParticleSystem>();
        sr = transform.GetComponent<SpriteRenderer>();
        enemyCollider = transform.GetComponent<BoxCollider2D>();
        anim = transform.GetComponent<Animator>();
        leftEdge = transform.position.x - moveDistance;
        rightEdge = transform.position.x + moveDistance;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        onLastTotemSpiritIncrease += LastTotemSpiritIncrease;
    }

    private void OnDisable()
    {
        onLastTotemSpiritIncrease -= LastTotemSpiritIncrease;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!(transform.CompareTag("Fungi") || transform.CompareTag("Fungi Spirit") || transform.CompareTag("Spirit")))
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
            gameObject.layer = 0;
            isMovingLeft = false;
            sr.sprite = spirit;
            gameObject.tag = "Spirit";
            audioSource.PlayOneShot(audioSource.clip);
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
            cdTime = 2f;
            countDown = true;
            if(attack)
                StartCoroutine("FungiAttack");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (gameObject.CompareTag("Fungi") || gameObject.CompareTag("Fungi Spirit")))
        {
            countDown = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if(health > 0)
        {
            health -= damage;
            StartCoroutine("Damaged");
        }

    }

    private IEnumerator FungiAttack()
    {
        Player iFrame = GameObject.FindObjectOfType<Player>();
        while(countDown == true)
        {
            attack = false;
            yield return new WaitForSeconds(cdTime);
            cdTime = 2.5f;
            AudioManager.Instance.PlayClipByName("Fungi Attack");
            ps.Play();
            if (countDown == true)
            {
                AudioManager.Instance.PlayClipByName("Damaged");           
            }
            attack = true;
        }
    }

    private void LastTotemSpiritIncrease(int amount)
    {
        if(amount >= 15){
            if(gameObject.CompareTag("Enemy") || gameObject.CompareTag("Fungi") || gameObject.CompareTag("Bird"))
                gameObject.SetActive(false);
            else
            {
                speed = 0;
                gameObject.layer = 0;
                isMovingLeft = false;
                sr.sprite = spirit;
                gameObject.tag = "Spirit";
                if(play)
                    audioSource.PlayOneShot(audioSource.clip);
                play = false;
                transform.localScale = new Vector2(1f, 1f);
                enemyCollider.size = new Vector2(0.28f, 0.488f);
                enemyCollider.offset = new Vector2(0f, 0f);
                enemyCollider.isTrigger = true;
                anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Collectibles/spirit") as RuntimeAnimatorController;
            }
        }
    }

    private IEnumerator Damaged()
    {
        transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.3f);
        transform.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1f);
    }
    
}
