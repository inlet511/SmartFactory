using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInfoManager : Singleton<PopUpInfoManager> {

    public Transform PopUpInfo;
	// Use this for initialization
	void Start () {
		PopUpInfo.gameObject.SetActive(false);

	}
	
    public void ShowInfo()
    {
        PopUpInfo.gameObject.SetActive(true);
        Invoke("HideInfo",1);
    }

    private void HideInfo()
    {
        PopUpInfo.gameObject.SetActive(false);
    }

}
