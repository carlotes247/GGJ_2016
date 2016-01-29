using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

/// <summary>
/// The Controller in charge of playing audio in the game
/// </summary>
public class AudioController : MonoBehaviour {    

    [SerializeField]
    private AudioClip[] audioSFXClips;
    /// <summary>
    /// (Property) The Array of SFX AudioClips to play (Read Only)
    /// </summary>
    public AudioClip[] AudioSFXClips { get { return this.audioSFXClips; } }

    [SerializeField]
    private AudioClip[] musicAudioClips;
    /// <summary>
    /// (Property) The array of Music AudioClips to play (Read Only)
    /// </summary>
    public AudioClip[] MusicAudioClips { get { return this.musicAudioClips; } }

    [SerializeField]
    private AudioMixer masterAudioMixer;
    /// <summary>
    /// (Property) The audio mixer that controls all the audio of the game
    /// </summary>
    public AudioMixer MasterAudioMixer { get { return this.masterAudioMixer; } }

    [SerializeField]
    private float masterVolume;
    /// <summary>
    /// (Property) The volume of all the audio
    /// </summary>
    public float MasterVolume { get { return this.masterVolume; } set { this.masterVolume = value; } }

    [SerializeField]
    private float musicVolume;
    /// <summary>
    /// (Property) The volume of the music track
    /// </summary>
    public float MusicVolume { get { return this.musicVolume; } set { this.musicVolume = value; } }

    [SerializeField]
    private float audioFXVolume;
    /// <summary>
    /// (Property) The volume of the audio effects
    /// </summary>
    public float AudioFXVolume { get { return this.audioFXVolume; } set { this.audioFXVolume = value; } }

    [SerializeField]
    private AudioMixerSnapshot[] snapshots;
    /// <summary>
    /// (Property) The audioMixer snapshots to control (read only)
    /// </summary>
    public AudioMixerSnapshot[] Snapshots { get { return this.snapshots; } }

    [SerializeField]
    private float timeToSwitchAudioStates;
    /// <summary>
    /// (Property) The time we want to switch between possible audio states (Read Only)
    /// </summary>
    public float TimeToSwitchAudioStates { get { return this.timeToSwitchAudioStates; } }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Sets the volume in the master track
    /// </summary>
    /// <param name="volume"> The volume to set </param>
    public void SetMasterVolume (float volume)
    {
        this.MasterAudioMixer.SetFloat("MasterVolume", volume);
        this.MasterVolume = volume;
    }

    /// <summary>
    /// Sets the volume in the music track
    /// </summary>
    /// <param name="volume"> The volume to set </param>
    public void SetMusicVolume (float volume)
    {
        this.MasterAudioMixer.SetFloat("MusicVolume", volume);
    }

    /// <summary>
    /// Sets the volume in the SFX track
    /// </summary>
    /// <param name="volume"> The volume to set </param>
    public void SetSFXVolume (float volume)
    {
        this.MasterAudioMixer.SetFloat("SFXVolume", volume);    
    }    

    /// <summary>
    /// Places an AudioSource to play one shot and disposes of it afterwards
    /// </summary>
    /// <param name="indexAudioClips"> The index of the track we want to access in the AudioSFXClips array</param>
    public void PlayOneShot(int indexAudioClips)
    {
        AudioSource.PlayClipAtPoint(AudioSFXClips[indexAudioClips], Camera.main.transform.position);
    }

    /// <summary>
    /// Transits to the specified snapshot in the Snapshots array in a time
    /// </summary>
    /// <param name="index"> The position of the snapshot in the Snapshots array </param>
    /// <param name="time"> The time we want to reach the snapshot </param>
    public void TransitToSnapshot(int index, float time)
    {
        // We go the position in the Snapshots array and transit to that Snapshot in time
        this.Snapshots[index].TransitionTo(time);

        if (Toolbox.Instance.GameManager.AllowDebugCode)
        {
            Debug.Log("Transit to Snapshot " + index.ToString() + " in " + time.ToString() +" seconds!");
        }
    }

    /// <summary>
    /// Applies to the musicTrack the lowpass filter or removes it depending on the bool passed in
    /// </summary>
    /// <param name="option"> True to apply, false to remove </param>
    public void LowpassMusicTrack(bool option)
    {
        // If the option is true...
        if (option)
        {
            // ... We lowpass the musicTrack in the time we want
            this.TransitToSnapshot(1, TimeToSwitchAudioStates);
            if (Toolbox.Instance.GameManager.AllowDebugCode)
            {
                Debug.Log("Transiting to Snapshot " + Snapshots[1].name);
            }
        }
        // If the option is false...
        else
        {
            //... We remove the lowpass effect from the musicTrack in the time we want
            this.TransitToSnapshot(0, TimeToSwitchAudioStates);
            if (Toolbox.Instance.GameManager.AllowDebugCode)
            {
                Debug.Log("Transiting to Snapshot " + Snapshots[0].name);
            }
        }
    }
}
