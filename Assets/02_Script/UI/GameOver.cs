using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameOver : MonoBehaviour
{//게임오버시 사용할 오브젝트
    [Header("UI")]
    public Button reStartBtn;                     //다시시작버튼
    public TextMeshProUGUI lable;             //게임클리어,게임오버
    public TextMeshProUGUI bestScoreTxt; //최고점수
    public TextMeshProUGUI currScoreTxt; //현재점수
    
    public GameObject newTxtObject;     //최고점수 갱신시

     AudioSource source; 
    public AudioClip[] audioClips;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (GameMgr.Inst.gameClear) //게임 클리어 확인
        {
            lable.text = "GameClear!!";
            source.clip = audioClips[0]; //클리어시 음악
        }
        else
            source.clip = audioClips[1]; //게임실패시 음악

        //최고 기록 확인
        if (GameMgr.BestStage < GameMgr.Inst.stage)
        {        
            GameMgr.BestStage = GameMgr.Inst.stage;
            PlayerPrefs.SetInt("BestStage", GameMgr.Inst.stage);
            newTxtObject.SetActive(true);
        }

        bestScoreTxt.text = (GameMgr.BestStage / 5 + 1) + " - " + (GameMgr.BestStage % 5 + 1);
        currScoreTxt.text = (GameMgr.Inst.stage /5  +1) + " - " + (GameMgr.Inst.stage % 5 + 1);

        reStartBtn.onClick.AddListener(ReStartGame);
    }

    void ReStartGame() //다시시작
    {
        //현재 씬을 다시 실행
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
