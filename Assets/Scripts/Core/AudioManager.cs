using UnityEngine;

namespace VirtualEyeClinic.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource bgmSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }

        public void PlayBGM(AudioClip clip)
        {
            if (clip != null && bgmSource != null)
            {
                bgmSource.clip = clip;
                bgmSource.loop = true;
                bgmSource.Play();
            }
        }
    }
}
