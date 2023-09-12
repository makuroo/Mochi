using UnityEngine;

public class AudioOptionsManager : MonoBehaviour
{
    public static float bgmVolume { get;  private set; }
    public static float sfxVolume { get;  private set; }
    public static float masterVolume { get;  private set; }

    private void Awake()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGM Volume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFX Volume");
        masterVolume = PlayerPrefs.GetFloat("Master Volume");
    }

    public void OnBGMSliderValueChange(float value)
    {
        PlayerPrefs.SetFloat("BGM Volume", value);
        bgmVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnSFXSliderValueChange(float value)
    {
        PlayerPrefs.SetFloat("SFX Volume", value);
        sfxVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnMasterSliderValueChange(float value)
    {
        PlayerPrefs.SetFloat("Master Volume", value);
        masterVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
    }
}
