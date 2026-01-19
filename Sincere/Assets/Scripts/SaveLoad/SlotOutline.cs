using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline;
    private Button button;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        button = GetComponent<Button>();

        if (outline != null)
        {
            outline.enabled = false;  // Ã³À½¿£ ¼û±è
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline != null && button.interactable)
        {
            outline.enabled = true;  // ¸¶¿ì½º ¿Ã¸®¸é ¿Ü°û¼± º¸ÀÓ
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = false;  // ¸¶¿ì½º ³»¸®¸é ¿Ü°û¼± ¼û±è
        }
    }
}