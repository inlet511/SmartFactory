using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum OperationState
{
    none = 0,
    tracing = 1,
    delete = 2
}



public class MeshDropper : Singleton<MeshDropper>
{

    public Button deleteButton;
    public Texture2D normalCursor;
    public Texture2D deleteCursor;
    //当前操作状态
    public OperationState currentOpState = OperationState.none;

    private OperationState lastOpState;
    //storage,milling, detecting, assembly
    public Transform[] areas;

    public LayerMask groundLayerMask;

    public LayerMask objectLayerMask;

    public Transform ObjectContainer;
    private GameObject currentProxyMesh;
    private GameObject currentMesh;




    private Camera mainCamera;

    private string cachedMeshPath;

    private int cachedEquipEnumNo;

    private int cachedArea;

    RaycastHit Groundhit;

    RaycastHit Objecthit;
    Ray ray;

    private List<GameObject> allObjects;

    private Transform LastHitObject;

    public void StartTracing(string proxyMesh, string mesh, int equipEnumNo, int area)
    {
        if (currentOpState != OperationState.none)
            return;
        if (currentProxyMesh != null)
        {
            DestroyImmediate(currentProxyMesh);
        }
        currentProxyMesh = (GameObject)Instantiate(Resources.Load(proxyMesh, typeof(GameObject)), Vector3.one * 100.0f, Quaternion.identity);
        cachedMeshPath = mesh;
        cachedEquipEnumNo = equipEnumNo;
        cachedArea = area;
        currentOpState = OperationState.tracing;
    }

    public void StopTracing()
    {
        currentOpState = OperationState.none;
        Destroy(currentProxyMesh);
    }

    void Start()
    {
        mainCamera = Camera.main;
        allObjects = new List<GameObject>();
        Cursor.SetCursor(normalCursor, new Vector2(0, 0), CursorMode.Auto);

    }

    void Update()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);


        switch (currentOpState)
        {
            case OperationState.tracing:

                if (currentProxyMesh == null) break;

                var hitGround = Physics.Raycast(ray, out Groundhit, 10000.0f, groundLayerMask);

                if (hitGround)
                {
                    currentProxyMesh.transform.position = Groundhit.point;

                    if (Input.GetMouseButtonDown(0))
                    {
                        // 人物可以放置于任何场景
                        if (cachedArea == -1 || (FindElementIndex(ref areas, Groundhit.collider.transform) == cachedArea))
                        {

                            if (cachedArea == -1)
                            {
                                cachedArea = FindElementIndex(ref areas, Groundhit.collider.transform);
                            }
                            //在正确的区域
                            GameObject newAddedMesh = (GameObject)Instantiate(Resources.Load(cachedMeshPath, typeof(GameObject)), Groundhit.point, Quaternion.identity);
                            newAddedMesh.transform.SetParent(ObjectContainer);
                            newAddedMesh.transform.GetComponent<EquipObject>().myArea = cachedArea;
                            newAddedMesh.transform.GetComponent<EquipObject>().equipNo = cachedEquipEnumNo;
                            SwitchOutline(newAddedMesh.transform, false);
                            allObjects.Add(newAddedMesh);

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
                            PopUpInfoManager.Instance.ShowInfo("不能放在这个区域!");
                        }

                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        currentOpState = OperationState.none;
                        DestroyImmediate(currentProxyMesh);
                    }
                }
                break;
            case OperationState.delete:


                bool hitObject = Physics.Raycast(ray, out Objecthit, 10000.0f, objectLayerMask);

                //如果没有trace到任何物体
                if (!hitObject)
                {
                    //如果之前有物体，将其outline关闭
                    if (LastHitObject)
                    {
                        SwitchOutline(LastHitObject, false);
                        LastHitObject = null;
                    }
                }
                else
                {
                    if (LastHitObject != Objecthit.transform)
                    {
                        if (LastHitObject != null)
                        {
                            SwitchOutline(LastHitObject, false);
                        }
                        SwitchOutline(Objecthit.transform, true);
                        LastHitObject = Objecthit.transform;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        allObjects.Remove(LastHitObject.gameObject);
                        int tempEquipNo = LastHitObject.GetComponent<EquipObject>().equipNo;

                        switch (LastHitObject.GetComponent<EquipObject>().myArea)
                        {
                            case 0:
                                Blueprint.Instance.CurrentStorage.Remove((Equip)tempEquipNo);
                                break;
                            case 1:
                                Blueprint.Instance.CurrentMilling.Remove((Equip)tempEquipNo);
                                break;
                            case 2:
                                Blueprint.Instance.CurrentDetection.Remove((Equip)tempEquipNo);
                                break;
                            case 3:
                                Blueprint.Instance.CurrentAssembly.Remove((Equip)tempEquipNo);
                                break;
                            default:
                                break;
                        }
                        DeleteMesh();
                    }
                }

                break;
            default:
                break;
        }


    }

    public void SwitchDeleteMode()
    {
        if (currentOpState != OperationState.delete)
        {
            lastOpState = currentOpState;
            currentOpState = OperationState.delete;

            ColorBlock ActiveColorSet = new ColorBlock();
            ActiveColorSet.normalColor = new Color(0f, 0.44f, 0.75f);
            ActiveColorSet.highlightedColor = new Color(0f, 0.55f, 0.93f);
            ActiveColorSet.pressedColor = new Color(0f, 0.44f, 0.75f);
            ActiveColorSet.disabledColor = new Color(0.5f, 0.5f, 0.5f);
            ActiveColorSet.colorMultiplier = 1.0f;
            deleteButton.colors = ActiveColorSet;

            Cursor.SetCursor(deleteCursor, new Vector2(0, 0), CursorMode.Auto);
        }
        else
        {
            currentOpState = lastOpState;
            ColorBlock InActiveColorSet = new ColorBlock();
            InActiveColorSet.normalColor = new Color(0.5f, 0.5f, 0.5f);
            InActiveColorSet.highlightedColor = new Color(0f, 0.55f, 0.93f);
            InActiveColorSet.pressedColor = new Color(0f, 0.44f, 0.75f);
            InActiveColorSet.disabledColor = new Color(0.5f, 0.5f, 0.5f);
            InActiveColorSet.colorMultiplier = 1.0f;
            deleteButton.colors = InActiveColorSet;

            Cursor.SetCursor(normalCursor, new Vector2(0, 0), CursorMode.Auto);
        }
    }


    public void SwitchOutline(Transform trans, bool show)
    {
        Outline[] outlines = trans.GetComponentsInChildren<Outline>();
        foreach (var i in outlines)
        {
            i.enabled = show;
        }
    }

    public void DeleteMesh()
    {
        if (currentOpState == OperationState.delete && LastHitObject)
        {
            DestroyImmediate(LastHitObject.gameObject);
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

    public void ClearAllMeshes()
    {
        var objectCount = ObjectContainer.childCount;
        for (int i = objectCount - 1; i >= 0; i--)
        {
            DestroyImmediate(ObjectContainer.GetChild(i).gameObject);
        }
    }

    public void StartAnimation()
    {
        foreach (var item in allObjects)
        {
            try
            {
                item.GetComponent<Animation>().Play();


            }
            catch (Exception e)
            {
                print(e);
            }

            try
            {
                item.GetComponent<MoveComponent>().StartMove();
            }
            catch (Exception e)
            {
                print(e);
            }
        }
    }
}
