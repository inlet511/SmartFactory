using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDropper : Singleton<MeshDropper>
{

    public Transform groundPlane;
    private GameObject currentProxyMesh;
    private GameObject currentMesh;

    //wether should we update the proxy mesh location
    private bool bTracing;

    private Camera mainCamera;

    private string cachedMeshPath;

    RaycastHit hit;
    Ray ray;

    public void StartTracing(string proxyMesh, string mesh)
    {
        currentProxyMesh = (GameObject)Instantiate(Resources.Load(proxyMesh, typeof(GameObject)), Vector3.one*100.0f, Quaternion.identity);
        cachedMeshPath = mesh;
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
                GameObject newAddedMesh = (GameObject)Instantiate(Resources.Load(cachedMeshPath, typeof(GameObject)), hit.point, Quaternion.identity);
                newAddedMesh.transform.SetParent(groundPlane);
                StopTracing();
            }
        }

    }
}
