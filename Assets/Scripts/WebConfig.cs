using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class WebConfig : Singleton<WebConfig>
{

    [HideInInspector]
    public string serverRootAddress = "";

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void RetrieveAddress();
#endif

    void Start()
    {

#if UNITY_EDITOR
        //本地测试
        serverRootAddress = "http://localhost:8080";
        print("InEditor");
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        RetrieveAddress();
#endif

    }


    public void SetRootAddress(string str)
    {
        serverRootAddress = str;
    }
}
