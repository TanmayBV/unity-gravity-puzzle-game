using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource background;
    public AudioSource sfx;

    [Header("AudioSource Clips")]
    public AudioClip bakgMusic;
    public AudioClip pointsSFX;

    private void Start()
    {
        background.clip = bakgMusic;
        background.Play();
    }

    //SFX AUDIO PLAY 
    public void PlaySFX(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }
}
