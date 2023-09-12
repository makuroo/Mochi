using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup bgmMixerGroup;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private Sound[] sounds;
    private float value;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateMixerVolume();
        //Debug.Log(bgmMixerGroup.audioMixer.GetFloat("BGM Volume",out value));
        //Debug.Log($"BGM Volume: {value}");
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.isLoop;
            s.source.playOnAwake = s.playOnAwake;
            s.source.volume = s.volume;
            switch (s.audioType)
            {
                case Sound.AudioTypes.SFX:
                    s.source.outputAudioMixerGroup = sfxMixerGroup;
                    break;

                case Sound.AudioTypes.BGM:
                    s.source.outputAudioMixerGroup = bgmMixerGroup;
                    break;
            }

            if (s.playOnAwake)
            {
                s.source.Play();
            }
        }

    }

    public void PlayClipByName(string _clipName)
    {
        Sound soundToPlay = Array.Find(sounds, dummySound => dummySound.clipName == _clipName);
        if (soundToPlay != null)
        {
            soundToPlay.source.Play();
        }
        

        //foreach(Sound s in sounds){
        //    if(s.clipName == _clipName){
        //         soundToPlay = sounds;
        //      }
        //}
    }

    public void StopClipByName(string _clipName)
    {
        Sound soundToStop = Array.Find(sounds, dummySound => dummySound.clipName == _clipName);
        if (soundToStop != null)
        {
            soundToStop.source.Stop();
        }
    }

    public void PlayOneShot(string _clipName)
    {
        Sound soundToStop = Array.Find(sounds, dummySound => dummySound.clipName == _clipName);
        if (soundToStop != null)
        {
            soundToStop.source.PlayOneShot(soundToStop.clip);
        }
    }

    public void UpdateMixerVolume()
    {
        bgmMixerGroup.audioMixer.SetFloat("BGM Volume", Mathf.Log10(AudioOptionsManager.bgmVolume) * 20);
        sfxMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(AudioOptionsManager.sfxVolume) * 20);
        masterMixerGroup.audioMixer.SetFloat("Master Volume", Mathf.Log10(AudioOptionsManager.masterVolume) * 20);
    }
}
