using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭한 버튼을 EventSystem의 현재 선택으로 강제 지정
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
