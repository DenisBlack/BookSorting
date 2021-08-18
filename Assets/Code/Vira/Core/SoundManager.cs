using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all methods for performing sound accompaniment
/// </summary>
public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    /// <summary>
    /// General list of sound
    /// </summary>
    [SerializeField]
    private List<AudioSource> audioSources = new List<AudioSource>();

    /// <summary>
    /// List of AudioSourcers that already have been instantiated
    /// </summary>
    private List<AudioSource> instantiatedAudioSources = new List<AudioSource>();

    /// <summary>
    /// List of currently playing AudioSources sounds
    /// </summary>
    private List<AudioSource> playingSoundAudioSourcesList = new List<AudioSource>();

    /// <summary>
    /// Instantiated sounds speed
    /// </summary>
    private float soundSpeed = 1;
    public float SoundSpeed
    {
        get
        {
            return soundSpeed;
        }

        set
        {
            soundSpeed = value;
            PlaySoundWithNewSpeed();
        }
    }

    /// <summary>
    /// changes speed in all instantiated sounds to SoundSpeed
    /// </summary>
    public void PlaySoundWithNewSpeed()
    {
        foreach (AudioSource item in instantiatedAudioSources)
        {
            item.pitch = SoundSpeed;
        }
    }

    /// <summary>
    /// Plays sound with given name
    /// </summary>
    /// <param name="clipName">Name of the sound</param>
    /// <param name="loop">>Is it need to play looped? (false by default)</param>
    public void PlaySound(string clipName, bool loop = false)
    {
        AudioSource audioSource = playingSoundAudioSourcesList.Find(sound => sound.clip.name == clipName);

        if (audioSource != null)
        {
            instantiateSound(clipName);
            return;
        }

        audioSource = instantiatedAudioSources.Find(x => x.clip.name == clipName);

        if (audioSource == null)
        {
            audioSource = instantiateSound(clipName);
        }
        else
        {
            audioSource.Play();
            addPlayingSoundTemporarily(audioSource);
        }

        audioSource.loop = loop;
        audioSource.pitch = SoundSpeed;
    }

    /// <summary>
    /// Instantiates needed sound AudioSource(also sets the parent transform and adds it
    /// to the instantiatedAudioSourcesList and temporarily to the currently playing AudioSources list)
    /// </summary>
    /// <param name="audioSource">Sound AudioSource</param>
    private AudioSource instantiateSound(string clipName)
    {
        AudioSource audioSource = audioSources.Find(x => x.clip.name == clipName);

        if (audioSource != null)
        {
            audioSource = Instantiate(audioSource);
            audioSource.transform.SetParent(transform);
            instantiatedAudioSources.Add(audioSource);
            addPlayingSoundTemporarily(audioSource);

            return audioSource;
        }
        else
        {
            Debug.LogWarning("no audio required");
            return null;
        }
    }

    /// <summary>
    /// Method that will add the playing sound AudioSource to the list of currently playing sound AudioSources
    /// and remove it from the list after it will stop playing
    /// </summary>
    /// <param name="parSoundAudioSource">Sound AudioSource</param>
    private void addPlayingSoundTemporarily(AudioSource parSoundAudioSource)
    {
        playingSoundAudioSourcesList.Add(parSoundAudioSource);
        StartCoroutine(removePlayingSoundFromList(parSoundAudioSource));
    }

    /// <summary>
    /// Method that will remove sound AudioSource from the list of currently playing sound AudioSources
    /// after it will stop playing
    /// </summary>
    /// <param name="parSoundAudioSource">Sound AudioSource</param>
    /// <returns></returns>
    private IEnumerator removePlayingSoundFromList(AudioSource parSoundAudioSource)
    {
        yield return new WaitForSeconds(parSoundAudioSource.clip.length);
        playingSoundAudioSourcesList.Remove(parSoundAudioSource);
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(transform.gameObject);
    }
}