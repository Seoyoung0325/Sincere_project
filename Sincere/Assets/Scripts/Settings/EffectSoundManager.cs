using UnityEngine;

public class EffectSoundManager : MonoBehaviour
{
    // public AudioSource effectSource;  // 오디오 소스가 아직 없으므로 주석 처리
    private static EffectSoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // float savedVolume = PlayerPrefs.GetFloat("EffectsVolume", 0.5f);
            // if (effectSource != null)
            //     effectSource.volume = savedVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float value)
    {
        // if (effectSource != null)
        //     effectSource.volume = value;

        PlayerPrefs.SetFloat("EffectsVolume", value);
    }

    public void PlayEffect(AudioClip clip)
    {
        // if (effectSource != null)
        // {
        //     effectSource.clip = clip;
        //     effectSource.Play();
        // }
    }
}