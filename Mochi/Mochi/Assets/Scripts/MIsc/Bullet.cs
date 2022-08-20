using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    private bool hit;
    private float direction;
    private float lifetime;

    private PlayerStatus playerStats;

    private BoxCollider2D boxCollider;
    //   [SerializeField] private AudioManager manager;

    private void Awake()
    {
        boxCollider = transform.GetComponent<BoxCollider2D>();
        playerStats = GameObject.FindObjectOfType<PlayerStatus>();
    }

    private void FixedUpdate()
    {
        // if hit collider stop
        if (hit) return;
        // bullet move to designed way
        float movementspeed = speed * Time.deltaTime * direction;
        transform.Translate(movementspeed, 0, 0);

        //if >5 seconds bullet disappear
        lifetime += Time.deltaTime;
        if (lifetime > 5) Deactivate();
    }

    //after bullet hit
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged") || collision.gameObject.CompareTag("Sensor"))
        {
            hit = true;
            Deactivate();
        }

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy Spirit") || collision.gameObject.CompareTag("Boss"))
        {
            collision.transform.GetComponent<Boss>().health -= playerStats.skillDmg;
            Deactivate();
        }
    }

    public void SetDirection(float _direction)
    {
        float localScalex = transform.localScale.x;
        lifetime = 0;
        if (_direction == 180)
        {
            direction = -1;
            localScalex = -localScalex;
        }
        else if (_direction == 0)
        {
            if(localScalex == -1)
               localScalex = -localScalex;
            direction = 1;
        }

        
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;
        transform.localScale = new Vector3(localScalex, transform.localScale.y, transform.localScale.z);
    }
    //deactivate bullet after explode
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
