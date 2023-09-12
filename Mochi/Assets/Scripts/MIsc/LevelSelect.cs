using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] Button[] Level;
    private void Awake()
    {
        if(!PlayerPrefs.HasKey("ClearedLevel"))
            PlayerPrefs.SetInt("ClearedLevel", 1);

        if (PlayerPrefs.GetInt("ClearedLevel") > 3)
            PlayerPrefs.SetInt("ClearedLevel", 3);
        Debug.Log(PlayerPrefs.GetInt("ClearedLevel"));
        for (int i = 0; i < PlayerPrefs.GetInt("ClearedLevel"); i++)
        {
            Debug.Log(i);
            Level[i].gameObject.SetActive(true);
        }
    }
}
