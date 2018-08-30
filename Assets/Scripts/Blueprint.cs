using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 工人：仓储，机加工，检测，装配
 * 
 * 工程师：仓储，机加工，检测
 */

public enum Equip
{
    //货架
    HuoJia,

    //叉车
    ChaChe,

    //立体仓库
    LiTiCangKu,

    //AGV
    AGV,

    //车床
    CheChuang,

    //铣床
    XiChuang,

    //机加工机器人
    JiJiaGongJiQiRen,

    //质检工业机器人
    ZhiJianGongYeJiQiRen,

    //比对仪
    BiDuiYi,

    //手工量具
    ShouGongLiangJu,

    //装配设备
    ZhuangPeiSheBei,

    //工人
    GongRen,

    //工程师
    GongChengShi,

    //新手装配工
    XinShouZhuangPeiGong,

    //熟练装配工
    ShuLianZhuangPeiGong
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
    private Equip[][] set1 = new Equip[4][];
    private Equip[][] set2 = new Equip[4][];

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

    }
}