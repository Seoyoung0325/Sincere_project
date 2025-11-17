using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceUI : MonoBehaviour
{
    [SerializeField] private Transform targetObject;  //UI가 따라다닐 오브젝트
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0); //오브젝트로부터의 오프셋

    private RectTransform rectTransform;
    private Camera mainCamera;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;

        if (targetObject == null)
        {
            targetObject = transform.parent;
        }
    }

    void LateUpdate()
    {
        if (targetObject != null && mainCamera != null)
        {
            //월드 좌표를 스크린 좌표로 변환
            Vector3 worldPosition = targetObject.position + offset;
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

            //UI 위치 업데이트
            rectTransform.position = screenPosition;
        }
    }
}