using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlowManager : Singleton<FlowManager> {
    public Transform[] pages;
    public RatingTemplate rate;

    void Start()
    {
        GoToPage(0);
        DontDestroyOnLoad(this.gameObject);
    }

    public void GoToPage(int number)
    {
        pages[number].gameObject.SetActive(true);
        for(var i = 0; i< pages.Length; i++)
        {
            if(i!=number)
            {
                pages[i].gameObject.SetActive(false);
            }
        }        
    }

    public void ExcutePlan()
    {
        rate = Blueprint.Instance.CheckScore();
        GoToPage(6);
        
    }

}
