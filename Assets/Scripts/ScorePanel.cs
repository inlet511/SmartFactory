using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : Singleton<ScorePanel>
{

    public RectTransform productionScore;
    public RectTransform totalCostScore;
    public RectTransform totalScore;


    public void SetupGraph(float ps, float tcs, float ts)
    {

		productionScore.localScale = new Vector3(1.0f,ps/100.0f,1.0f);
		totalCostScore.localScale = new Vector3(1.0f,tcs/100.0f,1.0f);
		totalScore.localScale = new Vector3(1.0f,ts/100.0f,1.0f);
    }

	public void ResetScore()
	{
		productionScore.localScale = Vector3.one;
		totalCostScore.localScale = Vector3.one;
		totalScore.localScale = Vector3.one;
	}
}
