using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClueUI : MonoBehaviour
{
    public static ClueUI instance;

    public GameObject notificationPanel; // UI 판넬
    public Text clueNameText;            // UI 텍스트
    public Image clueImage;              // UI 이미지

    public float openDelay = 0.6f;       // 팝업 대기 시간 
    public float displayTime = 1.0f;     // 화면 표출 시간

    private void Awake()
    {
        // 싱글톤 초기화 (중복 생성 방지)
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // 패널 숨겨두기
        if (notificationPanel != null)
            notificationPanel.SetActive(false);
    }

    public void ShowNotification(ClueData clue)
    {
        // UI 패널이 연결 안 되어 있으면 실행 취소
        if (notificationPanel == null) return;

        // 단서 이름 텍스트 설정
        if (clueNameText != null) clueNameText.text = clue.clueName;

        // 단서 이미지 처리
        if (clueImage != null)
        {
            // 이미지 경로 확인
            if (!string.IsNullOrEmpty(clue.clueImagePath))
            {
                // 이미지 로드
                Sprite sprite = Resources.Load<Sprite>(clue.clueImagePath);

                // 이미지 표시
                if (sprite != null)
                {
                    clueImage.sprite = sprite;
                    clueImage.gameObject.SetActive(true);
                }
                else clueImage.gameObject.SetActive(false);
            }
            // 경로가 없으면 이미지 오브젝트 끄기
            else clueImage.gameObject.SetActive(false);
        }

        // 기존 코루틴 정지
        StopAllCoroutines();
        // 대기 후 표시하는 시퀀스 시작
        StartCoroutine(ShowAndHideSequence());
    }

    IEnumerator ShowAndHideSequence()
    {
        
        yield return new WaitForSeconds(openDelay);

        // 패널 켜기
        notificationPanel.SetActive(true);

        // 화면 표출
        yield return new WaitForSeconds(displayTime);

        // 패널 끄기
        notificationPanel.SetActive(false);
    }
}