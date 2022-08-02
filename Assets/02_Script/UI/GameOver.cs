using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameOver : MonoBehaviour
{
    public Button reStartBtn;

    public TextMeshProUGUI bestScoreTxt;
    public TextMeshProUGUI currScoreTxt;

    public GameObject newTxtObject;

    private void Start()
    {
        if(GameMgr.BestStage < GameMgr.Inst.stage)
        {
            GameMgr.BestStage = GameMgr.Inst.stage;
            PlayerPrefs.SetInt("BestStage", GameMgr.Inst.stage);
        }

        bestScoreTxt.text = (GameMgr.BestStage / 4 + 1) + " - " + (GameMgr.BestStage % 4 + 1);
        currScoreTxt.text = (GameMgr.Inst.stage /4  +1) + " - " + (GameMgr.Inst.stage % 4 + 1);

        reStartBtn.onClick.AddListener(ReStartGame);

    }

    void ReStartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
