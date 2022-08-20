using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] Button[] Level;
    private void Awake()
    {
        if(!PlayerPrefs.HasKey("ClearedLevels"))
            PlayerPrefs.SetInt("ClearedLevels", 1);

        Debug.Log(PlayerPrefs.GetInt("ClearedLevels"));
        for (int i = 0; i < PlayerPrefs.GetInt("ClearedLevels"); i++)
        {
            Debug.Log(i);
            Level[i].gameObject.SetActive(true);
        }
    }
}
