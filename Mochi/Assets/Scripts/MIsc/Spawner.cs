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
    private PlayerStatus player;
    private bool slow = true;
    private float currentSpeed;
    [SerializeField] private bool drop = true;

    private void Awake()
    {
        boss = GameObject.FindObjectOfType<Boss>();
        player = GameObject.FindObjectOfType<PlayerStatus>();
    }
    private void Update()
    {
        currentSpeed = player.playerSpeed;
        if(boss.special == true)
        {
            xPos = Random.Range(0.8f, 23f);
            Icicle = Ice[Random.Range(0, 2)];
            duration -= Time.deltaTime;
            if (duration > 0 && drop == true)
            {
                if (slow)
                {
                    player.playerSpeed = 0.7f * player.playerSpeed;
                    slow = false;
                    Debug.Log(slow);
                }
                drop = false;
                StartCoroutine("Special");
            }
            if(duration <= 0)
            {
                player.playerSpeed = player.playerSpeed / 0.7f;
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
