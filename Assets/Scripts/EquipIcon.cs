using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class EquipIcon : MonoBehaviour
{

    private string cachedInfo;

    private string cachedProxyMeshPath;

    private string cachedMeshPath;

    private int cachedEquipEnumNo;

    private int cachedArea;



    public void Setup(Sprite icon, string name, string info,string proxyMeshPath, string meshPath, int equipEnumNo, int area)
    {
        //cache values from json to class fields
        cachedInfo = info;                
        cachedProxyMeshPath = proxyMeshPath;
        cachedMeshPath = meshPath;
        cachedEquipEnumNo = equipEnumNo;
        cachedArea = area;

        //setup Visual stuff
        transform.Find("IconImage").GetComponent<Image>().sprite = icon;
        transform.Find("Name").GetComponent<TMP_Text>().text = name;

        //Setup Events
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        UnityAction<BaseEventData> enterCallback = new UnityAction<BaseEventData>(MouseEnterButton);
        UnityAction<BaseEventData> exitCallback = new UnityAction<BaseEventData>(MouseExitButton);
        UnityAction<BaseEventData> downCallback = new UnityAction<BaseEventData>(MouseDownButton);


        EventTrigger.Entry enterEntry = new EventTrigger.Entry();
        enterEntry.eventID = EventTriggerType.PointerEnter;
        enterEntry.callback.AddListener(enterCallback);

        EventTrigger.Entry exitEntry = new EventTrigger.Entry();
        exitEntry.eventID = EventTriggerType.PointerExit;
        exitEntry.callback.AddListener(exitCallback);

        EventTrigger.Entry downEntry = new EventTrigger.Entry();
        downEntry.eventID = EventTriggerType.PointerDown;
        downEntry.callback.AddListener(downCallback);       

        eventTrigger.triggers.Add(enterEntry);
        eventTrigger.triggers.Add(exitEntry);
        eventTrigger.triggers.Add(downEntry);
    }

    public void MouseEnterButton(BaseEventData data)
    {        
        EquipInfoPanel.Instance.Show(cachedInfo);
    }

    public void MouseExitButton(BaseEventData data)
    {
        EquipInfoPanel.Instance.Hide();
    }

    public void MouseDownButton(BaseEventData data)
    {
        MeshDropper.Instance.StartTracing(cachedProxyMeshPath,cachedMeshPath,cachedEquipEnumNo,cachedArea);
    }
}
