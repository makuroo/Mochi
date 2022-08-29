using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraFit : MonoBehaviour
{
    public SpriteRenderer sr;
    public float xVariable;
    public float yVariable;
    public GameObject Boss;
    private PlayerStatus playerStats;
    [SerializeField] private GameObject healthbar;
    // Start is called before the first frame update
    private void Awake()
    {
        Boss = GameObject.FindGameObjectWithTag("Boss");
        playerStats = GameObject.FindObjectOfType<PlayerStatus>();
    }
    void Update()
    {
        float screenRatio = (float) Screen.width / (float)Screen.height;
        float targetratio = (xVariable*sr.bounds.size.x) / (yVariable*sr.bounds.size.y);

        if (screenRatio >= targetratio)
        {
            Camera.main.orthographicSize = sr.bounds.size.y/2f;
        }
        else
        {
            float differenceInSize = targetratio / screenRatio;
            Camera.main.orthographicSize = sr.bounds.size.y / 2 * differenceInSize;
        }

        if (playerStats.passed && SceneManager.GetActiveScene().buildIndex == 3 && Boss!=null)
        {
            Boss.GetComponent<Boss>().enabled = true;
            healthbar.SetActive(true);
        }

    }
}
