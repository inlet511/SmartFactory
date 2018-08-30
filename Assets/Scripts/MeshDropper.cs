﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDropper : Singleton<MeshDropper>
{

    //storage,milling, detecting, assembly
    public Transform[] areas;

    public Transform ObjectContainer;
    private GameObject currentProxyMesh;
    private GameObject currentMesh;


    //wether should we update the proxy mesh location
    private bool bTracing;

    private Camera mainCamera;

    private string cachedMeshPath;

    private int cachedEquipEnumNo;

    private int cachedArea;

    RaycastHit hit;
    Ray ray;

    public void StartTracing(string proxyMesh, string mesh, int equipEnumNo, int area)
    {
        if (currentProxyMesh != null)
        {
            DestroyImmediate(currentProxyMesh);
        }
        currentProxyMesh = (GameObject)Instantiate(Resources.Load(proxyMesh, typeof(GameObject)), Vector3.one * 100.0f, Quaternion.identity);
        cachedMeshPath = mesh;
        cachedEquipEnumNo = equipEnumNo;
        cachedArea = area;
        bTracing = true;
    }

    public void StopTracing()
    {
        bTracing = false;
        Destroy(currentProxyMesh);
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!bTracing || currentProxyMesh == null) return;

        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {

            currentProxyMesh.transform.position = hit.point;

            if (Input.GetMouseButtonDown(0))
            {
                // 人物可以放置于任何场景
                if (cachedArea == -1 || (FindElementIndex(ref areas, hit.collider.transform) == cachedArea))
                {
                    //在正确的区域
                    GameObject newAddedMesh = (GameObject)Instantiate(Resources.Load(cachedMeshPath, typeof(GameObject)), hit.point, Quaternion.identity);
                    newAddedMesh.transform.SetParent(ObjectContainer);
                    switch (cachedArea)
                    {
                        case 0:
                            Blueprint.Instance.CurrentStorage.Add((Equip)cachedEquipEnumNo);
                            break;
                        case 1:
                            Blueprint.Instance.CurrentMilling.Add((Equip)cachedEquipEnumNo);
                            break;
                        case 2:
                            Blueprint.Instance.CurrentDetection.Add((Equip)cachedEquipEnumNo);
                            break;
                        case 3:
                            Blueprint.Instance.CurrentAssembly.Add((Equip)cachedEquipEnumNo);
                            break;
                    }
                    StopTracing();

                }
                else
                {
                    PopUpInfoManager.Instance.ShowInfo();
                }

            }
            if(Input.GetMouseButtonDown(1))
            {
                bTracing = false;
                DestroyImmediate(currentProxyMesh);
            }
        }

    }

    public int FindElementIndex(ref Transform[] array, Transform item)
    {
        for (var i = 0; i < array.Length; i++)
        {
            if (item == array[i])
                return i;
        }

        return -1;
    }
}
