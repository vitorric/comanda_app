using UnityEngine.Audio;
using System;
using UnityEngine;

public class EasyAudioUtility : MonoBehaviour
{
    public enum Som
    {
        Background,
        Click_OK,
        Click_Cancel,
        Error,
        Success,
        LevelUp,
        LevelUp2,
        Compra_Item,
        Som_Camera
    }

    //Static reference
    public static EasyAudioUtility Instance;

    //Master Audio Mixer
    public AudioMixerGroup mixerGroup;

    //Helper Class
    public EasyAudioUtility_Helper[] helper;

    void Awake()
    {
        //creating static instance so we don't need any physical reference
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //Adding audio source in all helpers
        foreach (EasyAudioUtility_Helper h in helper)
        {
            h.source = gameObject.AddComponent<AudioSource>();
            h.source.clip = h.clip;
            h.source.loop = h.loop;

            h.source.outputAudioMixerGroup = mixerGroup;
        }

        Play(Som.Background);

    }

    /// <summary>
    /// Play an Audio Clip defined in the inspector
    /// </summary>
    /// <param name="sound"></param>
    public void Play(Som som)
    {
        string sound = som.ToString();

        EasyAudioUtility_Helper h = Array.Find(helper, item => item.name == sound);
        //randomizing volume by variation
        h.source.volume = h.volume;
        //randomizing pitch by variation
        h.source.pitch = h.pitch;

        //playing it after setting all variations
        if (h.source.enabled)
            h.source.Play();
    }

    public void AjustarSomBG(float volume){
        
        EasyAudioUtility_Helper h = Array.Find(helper, item => item.name == Som.Background.ToString());

        h.source.volume = volume * (1f + UnityEngine.Random.Range(-h.volumeVariance / 2f, h.volumeVariance / 2f));
    }

    public void AjustarSomSFX(float volume){
        
        Array.ForEach(helper, item => {
            if (item.name != Som.Background.ToString())   {
                item.volume = volume * (1f + UnityEngine.Random.Range(-item.volumeVariance / 2f, item.volumeVariance / 2f));
            }
        });

    }

    /// <summary>
    /// Stops an Audio which is being played
    /// </summary>
    /// <param name="sound"></param>
    public void Stop(string sound)
    {
        EasyAudioUtility_Helper h = Array.Find(helper, item => item.name == sound);
        //Stopping
        h.source.Stop();
    }

}
