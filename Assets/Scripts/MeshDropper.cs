using System.Collections;
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
    
    private int cachedArea;

    RaycastHit hit;
    Ray ray;

    public void StartTracing(string proxyMesh, string mesh,int area)
    {
        if(currentProxyMesh!=null)
        {
            DestroyImmediate(currentProxyMesh);
        }
        currentProxyMesh = (GameObject)Instantiate(Resources.Load(proxyMesh, typeof(GameObject)), Vector3.one*100.0f, Quaternion.identity);
        cachedMeshPath = mesh;
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
                if(FindElementIndex(ref areas, hit.collider.transform)==cachedArea)
                {
                    //在正确的区域
                    GameObject newAddedMesh = (GameObject)Instantiate(Resources.Load(cachedMeshPath, typeof(GameObject)), hit.point, Quaternion.identity);
                    newAddedMesh.transform.SetParent(ObjectContainer);
                    StopTracing();
                }
                else{
                    PopUpInfoManager.Instance.ShowInfo();
                }                
                
            }
        }

    }

    public int FindElementIndex(ref Transform[] array, Transform item)
    {
        for (var i = 0 ; i<array.Length;i++)
        {
            if(item == array[i])
            return i;
        }

        return -1;
    }
}
