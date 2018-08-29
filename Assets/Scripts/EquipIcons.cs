﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
public class Equipment
{
    public string name;
    public string info;
    public string imagePath;
    public string proxyMesh;
    public string mesh;
    public int area;
}

[System.Serializable]
public class Equipments
{
    public List<Equipment> equips;
}

public class EquipIcons : MonoBehaviour
{
    public Transform container;
    public GameObject equipIconPrefab;
    private string EquiDataPath = "/StreamingAssets/Equipments.json";
    public Equipments equipments = null;
    // Use this for initialization
    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(WebConfig.Instance.serverRootAddress + EquiDataPath);
        yield return www.SendWebRequest();

        string jsonstr = www.downloadHandler.text;
        equipments = JsonUtility.FromJson<Equipments>(jsonstr);

        foreach (var i in equipments.equips)
        {
            GameObject tempEquip = Instantiate(equipIconPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            tempEquip.transform.SetParent(container);
            Sprite icon = Resources.Load<Sprite>(i.imagePath);
            string name = i.name;
            tempEquip.GetComponent<EquipIcon>().Setup(icon, name, i.info, i.proxyMesh, i.mesh,i.area);
        }


    }

}
