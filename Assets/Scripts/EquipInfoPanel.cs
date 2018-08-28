using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInfoPanel : Singleton<EquipInfoPanel> {
    

    public Transform equipInfoPanel;


    public void Show(string info)
    {
        equipInfoPanel.gameObject.SetActive(true);
        equipInfoPanel.GetComponentInChildren<TMPro.TMP_Text>().SetText(info);
    }

    public void Hide()
    {
        equipInfoPanel.gameObject.SetActive(false);
        equipInfoPanel.GetComponentInChildren<TMPro.TMP_Text>().SetText("");
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        equipInfoPanel.position = Input.mousePosition;
    }
}
