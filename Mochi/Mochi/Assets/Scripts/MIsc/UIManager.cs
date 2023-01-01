using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text hpText;
    public TMP_Text timeText;
    public TMP_Text spiritsText;
    public Slider healthBar;
    public Image spiritImage;
    public List<Sprite> spiritTextImage = new List<Sprite> { };
    public Image debuffIcon; 
    private PlayerStatus playerStats;
    public float timeValue = 0, iconTime = 0;
    private float nextDebuff = 60f;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindObjectOfType<PlayerStatus>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthBar.value = playerStats.health;
        if (playerStats.health <= 0)
            Time.timeScale = 0;
        spiritImage.sprite = spiritTextImage[playerStats.spirits];
        timeValue += Time.deltaTime;
        iconTime += Time.deltaTime;
        debuffIcon.fillAmount = iconTime/ 60f;
        int minutes = Mathf.FloorToInt(timeValue / 60);
        int seconds = Mathf.FloorToInt(timeValue % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if(Mathf.FloorToInt(timeValue/nextDebuff) == 1)
        {

            iconTime = 0;
            nextDebuff += 60;
            playerStats.health -= 5;
            playerStats.basicAttDmg -= 3;
            playerStats.playerSpeed -= 1;
            AudioManager.Instance.PlayClipByName("Debuff");
        }
    }

}
