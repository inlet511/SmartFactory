using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public struct RatingTemplate
{
    //public Equip[][] Combination;
    public int[] Combination;
    public float ProductionRating;
    public float TotalCostRating;
    public float TotalRating;

    public RatingTemplate(int[] combination, float productionRating, float totalCostRating, float totalRating)
    {
        this.Combination = combination;
        this.ProductionRating = productionRating;
        this.TotalCostRating = totalCostRating;
        this.TotalRating = totalRating;
    }
}

public class Blueprint : Singleton<Blueprint>
{
    //当前的组合
    public List<Equip> CurrentStorage;
    public List<Equip> CurrentMilling;
    public List<Equip> CurrentDetection;
    public List<Equip> CurrentAssembly;

    //各个模块中正确的装配选项
    private Equip[] storage1, storage2, milling1, milling2, detection1, detection2, assembly1, assembly2;

    //评分模板
    private RatingTemplate[] RatingSets;


    void Start()
    {
        storage1 = new Equip[] { Equip.HuoJia, Equip.ChaChe, Equip.GongRen };
        storage2 = new Equip[] { Equip.LiTiCangKu, Equip.AGV, Equip.GongChengShi };
        milling1 = new Equip[] { Equip.CheChuang, Equip.XiChuang, Equip.GongRen };
        milling2 = new Equip[] { Equip.CheChuang, Equip.XiChuang, Equip.JiJiaGongJiQiRen, Equip.GongChengShi };
        detection1 = new Equip[] { Equip.GongRen, Equip.ShouGongLiangJu };
        detection2 = new Equip[] { Equip.ZhiJianGongYeJiQiRen, Equip.BiDuiYi, Equip.GongChengShi };
        assembly1 = new Equip[] { Equip.ZhuangPeiSheBei, Equip.XinShouZhuangPeiGong };
        assembly2 = new Equip[] { Equip.ZhuangPeiSheBei, Equip.ShuLianZhuangPeiGong };

        SetupRatingSets();
    }

    public void SetupRatingSets()
    {
        RatingSets = new RatingTemplate[16];

        RatingSets[0] = new RatingTemplate(new int[4] { 1, 1, 1, 1 }, 55.70f, 100.0f, 77.85f);
        RatingSets[1] = new RatingTemplate(new int[4] { 2, 1, 1, 1 }, 59.96f, 78.99f, 67.98f);
        RatingSets[2] = new RatingTemplate(new int[4] { 2, 2, 1, 1 }, 56.96f, 67.17f, 62.07f);
        RatingSets[3] = new RatingTemplate(new int[4] { 2, 2, 2, 1 }, 56.96f, 44.06f, 50.51f);
        RatingSets[4] = new RatingTemplate(new int[4] { 2, 2, 2, 2 }, 100.0f, 44.04f, 72.02f);
        RatingSets[5] = new RatingTemplate(new int[4] { 1, 2, 2, 2 }, 98.73f, 65.06f, 81.90f);
        RatingSets[6] = new RatingTemplate(new int[4] { 1, 1, 2, 2 }, 88.61f, 76.87f, 82.74f);
        RatingSets[7] = new RatingTemplate(new int[4] { 1, 1, 1, 2 }, 88.61f, 99.99f, 94.30f);
        RatingSets[8] = new RatingTemplate(new int[4] { 1, 2, 1, 1 }, 55.70f, 88.19f, 71.94f);
        RatingSets[9] = new RatingTemplate(new int[4] { 1, 2, 2, 1 }, 59.69f, 66.07f, 61.02f);
        RatingSets[10] = new RatingTemplate(new int[4] { 1, 1, 2, 1 }, 55.70f, 76.88f, 66.29f);
        RatingSets[11] = new RatingTemplate(new int[4] { 2, 1, 2, 1 }, 56.96f, 55.87f, 56.42f);
        RatingSets[12] = new RatingTemplate(new int[4] { 2, 1, 1, 2 }, 81.01f, 78.97f, 79.99f);
        RatingSets[13] = new RatingTemplate(new int[4] { 1, 2, 1, 2 }, 93.67f, 88.17f, 90.92f);
        RatingSets[14] = new RatingTemplate(new int[4] { 2, 1, 2, 2 }, 82.28f, 55.86f, 69.07f);
        RatingSets[15] = new RatingTemplate(new int[4] { 2, 2, 1, 2 }, 94.94f, 67.16f, 81.05f);

    }

    //根据当前的摆放，判断属于哪种组合
    //需要提供一个int[]数组变量（可以不用初始化）
    //返回true表示匹配成功，返回false表示没有任何可用组合可以匹配
    public bool CheckCombination(out int[] combination)
    {

        combination = new int[4];

        var compare1 = CurrentStorage.ToArray().Except(storage1);
        var compare2 = CurrentStorage.ToArray().Except(storage2);
        var compare3 = CurrentMilling.ToArray().Except(milling1);
        var compare4 = CurrentMilling.ToArray().Except(milling2);
        var compare5 = CurrentDetection.ToArray().Except(detection1);
        var compare6 = CurrentDetection.ToArray().Except(detection2);
        var compare7 = CurrentAssembly.ToArray().Except(assembly1);
        var compare8 = CurrentAssembly.ToArray().Except(assembly2);

        if (compare1.Count() == 0)
        {
            combination[0] = 1;
        }
        else if (compare2.Count() == 0)
        {
            combination[0] = 2;
        }
        else
        {
            return false;
        }

        if (compare3.Count() == 0)
        {
            combination[1] = 1;
        }
        else if (compare4.Count() == 0)
        {
            combination[1] = 2;
        }
        else
        {
            return false;
        }

        if (compare5.Count() == 0)
        {
            combination[2] = 1;
        }
        else if (compare6.Count() == 0)
        {
            combination[2] = 2;
        }
        else
        {
            return false;
        }

         if (compare7.Count() == 0)
        {
            combination[3] = 1;
        }
        else if (compare8.Count() == 0)
        {
            combination[3] = 2;
        }
        else
        {
            return false;
        }

        return true;

    }

    //检查最终得分
    public RatingTemplate CheckScore()
    {
        int[] combination;
        if(CheckCombination(out combination))
        {
            foreach(var ratingTemplate in RatingSets)
            {
                if(Enumerable.SequenceEqual(combination,ratingTemplate.Combination))
                    return ratingTemplate;
            }
        }
        return (new RatingTemplate(new int[4]{0,0,0,0},0.0f,0.0f,0.0f));
    }
}