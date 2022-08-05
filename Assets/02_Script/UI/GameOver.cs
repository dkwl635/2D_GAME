using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameOver : MonoBehaviour
{
    public Button reStartBtn;

    public TextMeshProUGUI lable;
    public TextMeshProUGUI bestScoreTxt;
    public TextMeshProUGUI currScoreTxt;

    
    public GameObject newTxtObject;

    public AudioSource source;
    public AudioClip[] audioClips;
    private void Start()
    {
        if (GameMgr.Inst.gameClear)
        {
            lable.text = "GameClear!!";
            source.clip = audioClips[0];
        }
        source.clip = audioClips[1];


        if (GameMgr.BestStage < GameMgr.Inst.stage)
        {
            Debug.Log(GameMgr.Inst.stage);
            GameMgr.BestStage = GameMgr.Inst.stage;
            PlayerPrefs.SetInt("BestStage", GameMgr.Inst.stage);
            newTxtObject.SetActive(true);
        }

        bestScoreTxt.text = (GameMgr.BestStage / 5 + 1) + " - " + (GameMgr.BestStage % 5 + 1);
        currScoreTxt.text = (GameMgr.Inst.stage /5  +1) + " - " + (GameMgr.Inst.stage % 5 + 1);

        reStartBtn.onClick.AddListener(ReStartGame);

    }

    void ReStartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
