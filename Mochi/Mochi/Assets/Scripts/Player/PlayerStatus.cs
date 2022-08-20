using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Player Status")]
    [Range(0, Mathf.Infinity)]
    public float health = 50;
    [Range(1, Mathf.Infinity)]
    public int basicAttDmg = 15;
    public int spirits = 3;
    [Range(1, Mathf.Infinity)]
    public int skillDmg = 20;
    [Range(1, Mathf.Infinity)]
    public float playerSpeed = 10f;

    [Header("iFrames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private float numberOfFlash;

    private bool inArea;
    private bool inAreaLast;
    public bool passed = false;

    public Camera cam;
    private Totem totem;
    private SpriteRenderer sr;
    private UIManager ui;

    [SerializeField] private GameObject GameOver;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 8, false);
        sr = transform.GetComponent<SpriteRenderer>();
        ui = GameObject.FindObjectOfType<UIManager>();
        totem = GameObject.FindObjectOfType<Totem>();
    }
    private void Update()
    {
        if (health < 0)
        {
            health = 0;
        }

        if (inArea == true || inAreaLast == true)
        {
            TransferSpirits();
        }
        if (health <= 0)
        {
            AudioManager.Instance.PlayClipByName("lose");
            GameOver.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")
            || collision.gameObject.CompareTag("Enemy Spirit") 
            || collision.gameObject.CompareTag("Bird")
            || collision.gameObject.CompareTag("Bird Spirit"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            AudioManager.Instance.PlayClipByName("Damaged");
            health -= enemy.dmg;
            StartCoroutine("iFrame");
            Debug.Log(health);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("Boss projectile"))
        {
            StartCoroutine("iFrame");
        }

        if (collision.gameObject.CompareTag("Spirit"))
        {
            if (spirits > 0)
            {
                AudioManager.Instance.PlayClipByName("collect");
                spirits--;
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Sensor"))
        {

            passed = true;
            Debug.Log("a");
            cam = FindObjectOfType<Camera>();
            cam.transform.position = new Vector3(11.9f, -0.45f, -35.11629f);
            ui.timeValue = 0f;
            transform.position = new Vector2(1.081689f, transform.position.y);
            totem.barier.GetComponent<BoxCollider2D>().isTrigger = false;
        }

        if (collision.gameObject.CompareTag("Totem")  )
        {
            totem = collision.GetComponent<Totem>();
            inArea = true;
            
        }

        if (collision.gameObject.CompareTag("Last Totem"))
        {
            totem = collision.GetComponent<Totem>();
            inAreaLast = true;
            Debug.Log("enter totem");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Totem") )
        {
            inArea = false;
        }

        if (collision.gameObject.CompareTag("Last Totem"))
        {
            inAreaLast = false;
            Debug.Log("exit totem");
        }
    }

    private void TransferSpirits()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inAreaLast)
            {
                totem.lastTotemSpirits += 3 - spirits;
                if (totem.storedSpirits > 15)
                {
                    spirits = totem.lastTotemSpirits - 15;
                }
            }
            else
            {
                totem.storedSpirits += 3 - spirits;
                if(totem.storedSpirits > 15)
                {
                    spirits = totem.storedSpirits - 15;
                }
            }
            totem.nextBuffSpirits -= 3 - spirits;
            spirits = 3;
        }
    }

    public IEnumerator iFrame()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        for (int i = 0; i < numberOfFlash; i++)
        {
            sr.color = new Color(1,1,1,.5f);
            yield return new WaitForSeconds(iFrameDuration/(numberOfFlash*2));
            sr.color = Color.white;
            yield return new WaitForSeconds(.3f);
        }   
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }
}
