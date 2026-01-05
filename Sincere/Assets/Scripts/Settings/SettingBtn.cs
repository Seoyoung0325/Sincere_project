using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBtn : MonoBehaviour
{
    public GameObject settingPanel;
    public GameObject slotPanel;
    public GameObject closeBtn;

    void Start()
    {
        settingPanel.SetActive(false);
    }

    public void ClickSettings()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingPanel.SetActive(false);
    }

    //´Ý±â ¹öÆ° Å¬¸¯½Ã ÆÐ³Î ´ÝÈû
    public void CloseSaving()
    {
        slotPanel.gameObject.SetActive(false);
        closeBtn.gameObject.SetActive(false);
    }
}
