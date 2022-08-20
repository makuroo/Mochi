using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public int storedSpirits = 0;
    public int lastTotemSpirits = 0;
    public int nextBuffSpirits = 5;
    private int clearedLevels = 1;

    public GameObject barier;
    private PlayerStatus playerStats;

    private bool win;

    [SerializeField] private GameObject VictoryPanel;

    private void Awake()
    {
        barier = GameObject.FindGameObjectWithTag("Sensor");
        playerStats = GameObject.FindObjectOfType<PlayerStatus>();
    }

    void Update()
    {
        if(nextBuffSpirits<=0)
        {
            if (playerStats.health != 50)
            {
               playerStats.health += 15;
               playerStats.health = Mathf.Clamp(playerStats.health, 0f, 50f);
            }
            playerStats.playerSpeed += 1f;
            playerStats.basicAttDmg += 5;
            playerStats.skillDmg += 5;
            nextBuffSpirits += 5;
        }

        if (storedSpirits >= 15 )
         {
            barier.GetComponent<BoxCollider2D>().isTrigger = true;
            transform.GetChild(0).gameObject.SetActive(true);
         }else if (lastTotemSpirits  >= 15 && win == false)
         {
            win = true;
            clearedLevels++;
            PlayerPrefs.SetInt("ClearedLevels", clearedLevels);
            Debug.Log(PlayerPrefs.GetInt("ClearedLevels"));
            transform.GetChild(0).gameObject.SetActive(true);
            VictoryPanel.SetActive(true);
            AudioManager.Instance.StopClipByName("BGM");
            AudioManager.Instance.PlayClipByName("win");
         }  
    }
}
