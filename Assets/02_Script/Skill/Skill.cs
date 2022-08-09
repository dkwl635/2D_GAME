using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CardHelp;
using MonsterHelper;

public abstract class Skill : MonoBehaviour , ICardLvUp
{//스킬의 베이스가 되는 클래스

    protected HeroCtrl hero; //캐릭터
    public int skill_Lv = 0; //스킬레벨 
    public int skill_MaxLv = 0; //스킬 최대 레벨 기본적으로 7단계
    public bool getSkill = false; //스킬을 얻은 스킬인지

    public string[] skillLvInfo = new string[7]; //스킬 레벨별 상승 설명
    public int[] skillPw = new int[7];               //스킬 레벨별 데미지
    public Sprite skillSprite;
   
    protected AudioSource audioSource;
    [SerializeField] protected float skillCool; //스킬 쿨타임   
    protected Coroutine skill_Co; //실행중이 스킬 코루틴 함수 저장용

    string CardInfo  //카드에 사용할 설명문
    {
        get
        {
            if (getSkill) //이미 얻은 스킬이면 다음레벨 설명
               return skillLvInfo[skill_Lv + 1];
            else 
                return skillLvInfo[0];
        }
    }

    //적용 스킬 쿨타임 (스킬 쿨타임 * 캐릭터의 스킬 쿨타임 적용 100 => 1 적용 80 => 0.8)
    public float SkillCool { get { return skillCool * (hero.SkillCool *0.01f); } }

    //실제 스킬 데미지 캐릭터으 추가 스킬데미지도 합산하여 계산
    public int SkillDamage { get { return skillPw[skill_Lv] + hero.skillPower; } }

    private void Awake()
    {
        hero = FindObjectOfType<HeroCtrl>();
        audioSource =GetComponent<AudioSource>();
    }

    private void Start()
    {
        Skill_Init(); //스킬 초기 설정을 하는
    }
    public virtual void Skill_Init(){ }//스킬 초기 설정을 하는
    public void SkillStart() //스킬을 실행하는 함수
    {
        if(skill_Co != null) //만약 기존 스킬이 돌아가는 중이면
        {//중단 후 다시 시작
           StopCoroutine(skill_Co);
            skill_Co = null;
        }
       //스킬 시작하기
       skill_Co =  StartCoroutine(SkillStart_Co());
    }

    public void TakeMonsterDamage(ITakeDamage monster)
    {//몬스터에게 데미지를 주는 콜라이더에서 이 함수를 호출
        monster.TakeDamage(SkillDamage);
    }

    public abstract IEnumerator SkillStart_Co();//스킬이 돌아가는 코루틴

   
    
    public void SkillLvUp() //스킬 레벨업 시켜주는
    {        
        skill_Lv++;
        SkillRefresh();
        SkillStart();
    }

    public abstract void SkillRefresh();//스킬 상태를 초기화 해줌 //스킬 정지와 같은 효과

    protected GameObject FindNearestObjectByTag(string tag)//가장 가까운 태그로 검색해서 오브젝트를 찾아줌
    {
        // 탐색할 오브젝트 목록을 List 로 저장합니다.
        var objects = GameObject.FindGameObjectsWithTag(tag).ToList();
        // LINQ 메소드를 이용해 가장 가까운 적을 찾습니다.
        var neareastObject = objects
            .OrderBy(obj =>
            {//거리를 계산하여
                return Vector3.Distance(hero.transform.position, obj.transform.position);
            })
        .FirstOrDefault(); //첫번째 요소 반환 없으면 null;

        return neareastObject;
    }

    public CardData GetCard() //카드데이터 반환
    {
        CardData card;
        card.img = skillSprite;
        card.info = CardInfo;
        return card;
    }

    public bool LevelPossible() //레벨업이 가능한지
    {
        if (!getSkill || skill_Lv == skill_MaxLv)
            return false;
        else
            return true;
    }

    public void LevelUp() //카드를 통한 레벨업
    {
        if (getSkill)
            SkillLvUp();
        else
        {
            getSkill = true;
            this.gameObject.SetActive(true);         
        }
    }
}
