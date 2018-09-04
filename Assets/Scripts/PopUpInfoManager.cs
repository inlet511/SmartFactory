using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInfoManager : Singleton<PopUpInfoManager> {

    public Transform PopUpInfo;
    public TMPro.TMP_Text text;
	// Use this for initialization
	void Start () {
		PopUpInfo.gameObject.SetActive(false);

	}
	
    public void ShowInfo(string txt)
    {
        text.SetText(txt);
        PopUpInfo.gameObject.SetActive(true);
        Invoke("HideInfo",1);
    }

    private void HideInfo()
    {
        PopUpInfo.gameObject.SetActive(false);
    }

}
