using UnityEngine.UI;
using UnityEngine;

public class SetSlider : MonoBehaviour
{

    private void Awake()
    {
        if (gameObject.CompareTag("Master Slider"))
            gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Master Volume",1f);
        if (gameObject.CompareTag("BGM Slider"))
             gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat("BGM Volume",1f);
        if (gameObject.CompareTag("SFX Slider"))
            gameObject.GetComponent<Slider>().value = PlayerPrefs.GetFloat("SFX Volume",1f);
    }
}
