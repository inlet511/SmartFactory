using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlowManager : Singleton<FlowManager>
{

    //一级分页，0为封面
    public Transform[] pages;

    public Transform header;

    public Camera MainCam;

    public Camera SimulateCam;

    //实验总览单元的页面
    public Transform Exp_Index;
    public Transform Exp_Sub;
    public Transform[] ExpSubpages;


    public TMPro.TMP_Text TaskTitle;
    //当前任务的文字
    public TMPro.TMP_Text TaskText;
    //进入下一期的按钮文字
    public TMPro.TMP_Text NextPhaseButtonText;


    //当前实验的阶段，共有三个阶段，0，1，2
    public int currentPhase = 0;

    public RatingTemplate rate;

    void Start()
    {
        GoToPage(0);
        DontDestroyOnLoad(this.gameObject);
    }

    public void GoToPage(int number)
    {
        if (number == 0)
        {
            header.gameObject.SetActive(false);
        }
        else
        {
            header.gameObject.SetActive(true);
        }

        for (var i = 0; i < pages.Length; i++)
        {
            pages[i].gameObject.SetActive(false);
        }

        pages[number].gameObject.SetActive(true);
    }

    public void Exp_SwitchTab(int index)
    {
        if (index >= ExpSubpages.Length)
        {
            return;
        }

        Exp_Index.gameObject.SetActive(false);
        Exp_Sub.gameObject.SetActive(true);


        for (var i = 0; i < ExpSubpages.Length; i++)
        {
            ExpSubpages[i].gameObject.SetActive(false);
        }

        ExpSubpages[index].gameObject.SetActive(true);

    }

    public void ExcutePlan()
    {
        rate = Blueprint.Instance.CheckScore();

        if (rate.ProductionRating == 0.0f && rate.TotalCostRating == 0.0f && rate.TotalRating == 0.0f)
        {
            PopUpInfoManager.Instance.ShowInfo("没有装配完成");
            return;
        }
        //播放一些动画
        MeshDropper.Instance.StartAnimation();
        MainCam.enabled = false;
        SimulateCam.enabled = true;
        Invoke("ShowResult", 10.0f);
    }

    public void ShowResult()
    {
        GoToPage(3);
        MainCam.enabled = true;
        SimulateCam.enabled = false;
        ScorePanel.Instance.SetupGraph(rate.ProductionRating, rate.TotalCostRating, rate.TotalRating);
    }

    public void NextPhase()
    {
        //清理所有场景中的模型
        MeshDropper.Instance.ClearAllMeshes();

        //重置记分板
        ScorePanel.Instance.ResetScore();


        //增加Phase的索引
        if (currentPhase == 2)
        {
            currentPhase = 0;
        }
        else
        {
            currentPhase++;
        }

        //充值任务文字和按钮文字
        if (currentPhase == 0)
        {
            TaskTitle.SetText("产量最高");
            TaskText.SetText("集团目前处于快速扩张期，总部希望我们尽可能满足市场需求，一期生产模式及流程需要达到最大的产能。具体方案请您来设计吧");
            NextPhaseButtonText.SetText("进入二期");
        }
        else if (currentPhase == 1)
        {
            TaskTitle.SetText("综合最优");
            TaskText.SetText("综合考虑成本和产量两个指标，优化生产模式及流程。在保证高产量的同时，控制生产成本。具体方案请您来设计。");
            NextPhaseButtonText.SetText("进入三期");
        }
        else if (currentPhase == 2)
        {
            TaskTitle.SetText("指标再平衡");
            TaskText.SetText("集团对成本和产量综合最优的方案绩效非常满意，决定追加300000元投资，要求进一步扩大产量。请您来设计新的方案。");
            NextPhaseButtonText.SetText("完成");
        }

        GoToPage(2);
    }

    public void ShowBaoZhaTu()
    {
        ExpSubpages[1].GetComponent<Image>().enabled = false;
        ExpSubpages[1].Find("EnterFactory").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button2").gameObject.SetActive(false);
        ExpSubpages[1].Find("Bom").gameObject.SetActive(true);
        ExpSubpages[1].Find("List").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button_back").gameObject.SetActive(true);
        ExpSubpages[1].Find("Button_back2").gameObject.SetActive(false);
    }
    public void HideBaoZhaTu()
    {
        ExpSubpages[1].GetComponent<Image>().enabled = true;
        ExpSubpages[1].Find("EnterFactory").gameObject.SetActive(true);
        ExpSubpages[1].Find("Button").gameObject.SetActive(true);
        ExpSubpages[1].Find("Button2").gameObject.SetActive(true);
        ExpSubpages[1].Find("List").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button_back").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button_back2").gameObject.SetActive(false);
        ExpSubpages[1].Find("Bom").gameObject.SetActive(false);
    }

    public void ShowWuLiao()
    {
        ExpSubpages[1].GetComponent<Image>().enabled = false;
        ExpSubpages[1].Find("EnterFactory").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button2").gameObject.SetActive(false);
        ExpSubpages[1].Find("Bom").gameObject.SetActive(false);
        ExpSubpages[1].Find("List").gameObject.SetActive(true);
        ExpSubpages[1].Find("Button_back").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button_back2").gameObject.SetActive(true);
    }

    public void HideWuLiao()
    {
        ExpSubpages[1].GetComponent<Image>().enabled = true;
        ExpSubpages[1].Find("EnterFactory").gameObject.SetActive(true);
        ExpSubpages[1].Find("Button").gameObject.SetActive(true);
        ExpSubpages[1].Find("Button2").gameObject.SetActive(true);
        ExpSubpages[1].Find("List").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button_back").gameObject.SetActive(false);
        ExpSubpages[1].Find("Button_back2").gameObject.SetActive(false);
        ExpSubpages[1].Find("Bom").gameObject.SetActive(false);
    }

}
