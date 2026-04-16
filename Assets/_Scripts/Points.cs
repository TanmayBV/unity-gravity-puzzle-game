using UnityEngine;

public class Points : MonoBehaviour, IInteractable
{
    private UI u;
    private AudioManager audio;
    private void Start()
    {
        u = FindAnyObjectByType<UI>();
        audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

    }

    //CALL WHEN PLAYER INTERACTS
    public void Interact() 
    {
        audio.PlaySFX(audio.pointsSFX);

        gameObject.SetActive(false);
        u.setScore();
    }
}
