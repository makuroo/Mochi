using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text hpText;
    public TMP_Text timeText;
    public TMP_Text spiritsText;
    public Slider healthBar;

    private PlayerStatus playerStats;
    public float timeValue = 0;
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
        hpText.text = playerStats.health.ToString();
        if (playerStats.health <= 0)
            Time.timeScale = 0;
        spiritsText.text = (3 - playerStats.spirits).ToString();
        timeValue += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeValue / 60);
        int seconds = Mathf.FloorToInt(timeValue % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if(Mathf.FloorToInt(timeValue/nextDebuff) == 1)
        {
            nextDebuff += 60;
            playerStats.health -= 5;
            playerStats.basicAttDmg -= 3;
            playerStats.skillDmg -= 3;
            playerStats.playerSpeed -= 1;
        }
    }

}
