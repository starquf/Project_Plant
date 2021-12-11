using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreateHandler : MonoBehaviour
{
    public List<StageVO> stages = new List<StageVO>();
    public int currentStage = 0;

    private void Start()
    {
        currentStage = GameManager.Instance.stageIdx;

        if (currentStage > stages.Count - 1) return;

        CreateStage(currentStage);
    }

    public void CreateStage(int stageIdx)
    {
        StageVO stage = stages[stageIdx];

        GameManager.Instance.SetFruitCnt(stage.waterMelonCnt, stage.appleCnt, stage.bananaCnt);
        Camera.main.orthographicSize = stage.camSize;

        Instantiate(stage.stagePrefab);

        StartCoroutine(Refresh());
    }

    private IEnumerator Refresh()
    {
        yield return null;

        GameManager.Instance.OnRefresh();
    }
}
