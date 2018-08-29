using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInfoPanel : Singleton<EquipInfoPanel> {
    

    public RectTransform equipInfoPanel;


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

        Vector3 clampedPosition = new Vector3( Input.mousePosition.x,  Mathf.Max(Input.mousePosition.y, equipInfoPanel.sizeDelta.y),0.0f);
        equipInfoPanel.position = clampedPosition;
    }
}
