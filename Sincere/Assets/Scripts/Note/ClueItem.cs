using UnityEngine;
using UnityEngine.UI;

public class ClueItem : MonoBehaviour
{
    [Header("UI 참조")]
    public Text clueName;
    public Text clueDescription;
    public Image clueImage;
    public Text clueMap;

    public void Setup(ClueData clue)
    {
        if (clueName != null)
        {
            clueName.text = clue.clueName;
        }

        if (clueMap != null)
        {
            clueMap.text = clue.clueMap;
        }

        if (clueDescription != null)
        {
            clueDescription.text = clue.clueDescription;
        }

        if (clueImage != null && !string.IsNullOrEmpty(clue.clueImagePath))
        {
            // Resources 폴더에서 이미지 로드
            Sprite sprite = Resources.Load<Sprite>(clue.clueImagePath);
            if (sprite != null)
            {
                clueImage.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"이미지를 찾을 수 없습니다: {clue.clueImagePath}");
            }
        }
    }
}