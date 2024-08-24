using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Status")]
    [SerializeField] private float health = 50;

    private float currHealth;
    public float CurrHealth
    {
        get => currHealth;
        set => currHealth = value;
    }
    
    [SerializeField] private int basicAttDmg = 15;

    private int currBasicAttackDamage;
    public int CurrAttackDamage
    {
        get => currBasicAttackDamage;
        set => currBasicAttackDamage = value;
    }

    public float CurrPlayerSpeed
    {
        get => currPlayerSpeed;
        set => currPlayerSpeed = value;
    }

    public int Spirits
    {
        get => spirits;
        set => spirits = value;
    }

    public int CurrSkillDamage
    {
        get => currSkillDamage;
        set => currSkillDamage = value;
    }

    public float PlayerSpeed => playerSpeed;

    [SerializeField] private int spirits = 0;
    
    [SerializeField] private int skillDmg = 25;
    private int currSkillDamage;
    
    [SerializeField] private float playerSpeed = 10f;

    private float currPlayerSpeed;

    [Header("iFrames")]
    [SerializeField] private float iFrameDuration;
    [SerializeField] private float numberOfFlash;

    private bool inArea;
    private bool inAreaLast;
    public bool passed = false;
    public bool damaged = false;

    [SerializeField] private Camera cam;
    private SpriteRenderer sr;

    [SerializeField] private GameObject skillIcon;
    [SerializeField] private GameObject GameOver;

    public Action<float> OnTakeDamage;
    public Action OnDebuff;
    public Action OnBuff;

    private Totem _totem;

    private void OnEnable()
    {
        OnTakeDamage += TakeDamage;
        OnDebuff += Debuff;
        OnBuff += Buff;
    }

    private void OnDisable()
    {
        OnTakeDamage -= TakeDamage;
        OnDebuff -= Debuff;
        OnBuff -= Buff;
    }

    private void Start()
    {
        sr = transform.GetComponent<SpriteRenderer>();
        Init();
    }

    private void Init()
    {
        CurrHealth = health;
        CurrAttackDamage = basicAttDmg;
        CurrPlayerSpeed = PlayerSpeed;
        CurrSkillDamage = skillDmg;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Enemy enemy)) return;
        AudioManager.Instance.PlayClipByName("Damaged");
        TakeDamage(enemy.dmg);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&&_totem)
        {
            _totem.RecieveSpirits(Spirits);
            Spirits = 0;
            AudioManager.Instance.PlayClipByName("collect");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("Boss projectile"))
        {
            StartCoroutine(nameof(iFrame));
        }

        if (collision.gameObject.CompareTag("Spirit"))
        {
            if (Spirits < 3)
            {
                AudioManager.Instance.PlayClipByName("collect");
                Spirits++;
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Sensor"))
        {

            passed = true;
            cam = FindObjectOfType<Camera>();
            cam.transform.position = new Vector3(11.9f, -0.45f, -35.11629f);
            transform.position = new Vector2(1.081689f, transform.position.y);
            skillIcon.SetActive(true);
        }

        if (collision.TryGetComponent(out Totem totem))
        {
            _totem = totem;
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

    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out Particle particle))
        {
            Debug.Log("jeeere");
        }
    }

    private void TakeDamage(float damage)
    {
        CurrHealth -= damage;
        UIManager.Instance.OnUpdateHealthUI?.Invoke(CurrHealth);
        StartCoroutine(nameof(iFrame));
        if (CurrHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.Instance.PlayClipByName("lose");
        GameOver.gameObject.SetActive(true);
        Destroy(gameObject);
    }

    private void Debuff()
    {
        OnTakeDamage?.Invoke(5);
        CurrAttackDamage -= 3;
        CurrPlayerSpeed -= 1;
    }

    private void Buff()
    {
        if (CurrHealth < 50)
        {
            CurrHealth = Mathf.Clamp(CurrHealth + 10, 0, 50);
        }

        CurrPlayerSpeed += 1;
        CurrAttackDamage += 3;
        CurrSkillDamage += 5;
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
        damaged = false;
    }
}
