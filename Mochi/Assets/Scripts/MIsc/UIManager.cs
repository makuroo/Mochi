using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text spiritsText;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image spiritImage;
    [SerializeField] private List<Sprite> spiritTextImage = new List<Sprite> { };
    [SerializeField] private Image debuffIcon;
    
    [SerializeField] private Player playerStats;
    public float timeValue = 0, iconTime = 0;
    private float nextDebuff = 60f;

    public Action<float> OnUpdateHealthUI;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        OnUpdateHealthUI += UpdateHealthUI;
    }

    private void OnDisable()
    {
        OnUpdateHealthUI -= UpdateHealthUI;
    }

    private void Update()
    {
        spiritImage.sprite = spiritTextImage[playerStats.Spirits];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeValue += Time.deltaTime;
        iconTime += Time.deltaTime;
        debuffIcon.fillAmount = iconTime/ 60f;
        int minutes = Mathf.FloorToInt(timeValue / 60);
        int seconds = Mathf.FloorToInt(timeValue % 60);
        timeText.text = $"{minutes:00}:{seconds:00}";
        
        if(Mathf.FloorToInt(timeValue/nextDebuff) == 1)
        {
            iconTime = 0;
            nextDebuff += 60;
            playerStats.OnDebuff?.Invoke();
            AudioManager.Instance.PlayClipByName("Debuff");
        }
    }


    private void UpdateHealthUI(float currHealth)
    {
        healthBar.value = Mathf.Clamp(currHealth, 0, 100);
    }
}
