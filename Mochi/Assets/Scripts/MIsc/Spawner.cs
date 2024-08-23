using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> Ice = new List<GameObject> { };
    private GameObject Icicle;
    private Boss boss;
    private float xPos;
    private float duration = 5f;
    private Player player;
    private bool slow = true;
    private float currentSpeed;
    [SerializeField] private bool drop = true;

    private void Awake()
    {
        boss = GameObject.FindObjectOfType<Boss>();
        player = GameObject.FindObjectOfType<Player>();
    }
    private void Update()
    {
        if(boss.special)
        {
            xPos = Random.Range(0.8f, 23f);
            Icicle = Ice[Random.Range(0, 2)];
            duration -= Time.deltaTime;
            if (duration > 0 && drop == true)
            {
                if (slow)
                {
                    player.CurrPlayerSpeed = 0.7f * player.CurrPlayerSpeed;
                    slow = false;
                    Debug.Log(slow);
                }
                drop = false;
                StartCoroutine(nameof(Special));
            }
            if(duration <= 0)
            {
                player.CurrPlayerSpeed /= 0.7f;
                boss.special = false;
                slow = true;
                duration = 5f;
            }
        }        
    }

    private IEnumerator Special()
    {
        Instantiate(Icicle, new Vector2(xPos, transform.position.y), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        drop = true;
    }
}
