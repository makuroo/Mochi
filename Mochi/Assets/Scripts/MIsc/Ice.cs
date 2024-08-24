using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public int damage = 12;
    public List<AudioClip> iceSFX = new List<AudioClip> { };
    private AudioSource audioSource;
    
    private void Awake()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
        {
            if(gameObject.CompareTag("Big Ice"))
            {
                audioSource.clip = iceSFX[0];
                audioSource.Play();
            }
            else
            {
                audioSource.clip = iceSFX[1];
                audioSource.Play();
            }

            if (collision.gameObject.TryGetComponent(out Player player))
            {
                player.OnTakeDamage?.Invoke(damage);
            }
            Destroy(gameObject, 2f);
        }
    }
}
