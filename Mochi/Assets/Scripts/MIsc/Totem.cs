using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public int storedSpirits = 0;
    public int lastTotemSpirits = 0;
    public int nextBuffSpirits = 5;
    private int clearedLevels = 1;
    [SerializeField] private GameObject BossHealthBar;
    [SerializeField] private GameObject arrow;

    public GameObject barier;
    private Player playerStats;
 

    private bool win;

    [SerializeField] private GameObject VictoryPanel;

    [SerializeField] private bool isLastTotem;

    private void Awake()
    {
        barier = GameObject.FindGameObjectWithTag("Sensor");
        playerStats = GameObject.FindObjectOfType<Player>();
        
    }

    void Update()
    {
        if (nextBuffSpirits<=0)
        {
            playerStats.OnBuff?.Invoke();
            nextBuffSpirits += 5;
            AudioManager.Instance.PlayClipByName("Buff");
        }
        
        if(playerStats.passed == true)
        {
            barier.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        
        if (storedSpirits >= 15 )
        {
            if(playerStats.passed == false)
            {
                arrow.SetActive(true);
                barier.GetComponent<BoxCollider2D>().isTrigger = true;
            }   
            transform.GetChild(0).gameObject.SetActive(true);
        }else if (lastTotemSpirits  >= 15 && win == false)
        {
            if(SceneManager.GetActiveScene().buildIndex == 3)
            {
               GameObject Boss  = GameObject.FindGameObjectWithTag("Boss");    
                Boss.GetComponent<Boss>().health = 0;
                BossHealthBar.transform.position = new Vector2(2000f, 2000f);
                Boss.SetActive(false);
            }
            win = true;
            clearedLevels = SceneManager.GetActiveScene().buildIndex + 1;
            PlayerPrefs.SetInt("ClearedLevel", clearedLevels);
            transform.GetChild(0).gameObject.SetActive(true);
            VictoryPanel.SetActive(true);
            AudioManager.Instance.StopClipByName("BGM");
            AudioManager.Instance.PlayClipByName("win");
            Time.timeScale = 0;
        }
    }

    private IEnumerator Delay(GameObject Boss)
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Destroy");
        Destroy(Boss);
    }
}
