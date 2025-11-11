using UnityEngine;

public class BgmManager : MonoBehaviour
{
    // public AudioSource bgmSource; // 아직 오디오 소스가 없으므로 주석 처리
    private static BgmManager instance; // 싱글톤 인스턴스

    void Awake()
    {
        // 싱글톤 패턴: 최초 생성된 인스턴스만 유지
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 오브젝트 유지

            // 저장된 볼륨 값 불러와서 적용
            float savedVolume = PlayerPrefs.GetFloat("BgmVolume", 0.5f);

            // 오디오 소스가 있을 경우 볼륨 적용 (현재는 주석 처리)
            // if (bgmSource != null)
            //     bgmSource.volume = savedVolume;
        }
        else
        {
            // 중복 생성 방지
            Destroy(gameObject);
        }
    }

    // 외부에서 볼륨 설정을 전달받는 함수
    public void SetVolume(float value)
    {
        // if (bgmSource != null)
        //     bgmSource.volume = value;

        PlayerPrefs.SetFloat("BgmVolume", value); // 설정값 저장
    }

    // 외부에서 BGM을 재생할 때 사용하는 함수
    public void PlayBgm(AudioClip clip)
    {
        // if (bgmSource != null)
        // {
        //     bgmSource.clip = clip;
        //     bgmSource.Play();
        // }
    }
}