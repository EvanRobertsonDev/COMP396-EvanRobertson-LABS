using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GuardSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    public void PlayStep()
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
