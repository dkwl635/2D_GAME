using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameOver : MonoBehaviour
{//���ӿ����� ����� ������Ʈ
    [Header("UI")]
    public Button reStartBtn;                     //�ٽý��۹�ư
    public TextMeshProUGUI lable;             //����Ŭ����,���ӿ���
    public TextMeshProUGUI bestScoreTxt; //�ְ�����
    public TextMeshProUGUI currScoreTxt; //��������
    
    public GameObject newTxtObject;     //�ְ����� ���Ž�

     AudioSource source; 
    public AudioClip[] audioClips;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (GameMgr.Inst.gameClear) //���� Ŭ���� Ȯ��
        {
            lable.text = "GameClear!!";
            source.clip = audioClips[0]; //Ŭ����� ����
        }
        else
            source.clip = audioClips[1]; //���ӽ��н� ����

        //�ְ� ��� Ȯ��
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

    void ReStartGame() //�ٽý���
    {
        //���� ���� �ٽ� ����
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
