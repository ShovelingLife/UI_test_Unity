using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Skill_slot_manager : Singleton_local<Skill_slot_manager>
{
    public Text skill_points_txt;
    private int m_skill_points=20;

    // 사무라이 스킬 관련
    [Header("사무라이 스킬")]
    public Samurai_skill samurai_skill_inst;

    [Header("아처 스킬")]
    public Archer_skill archer_skill_inst;

    // Start is called before the first frame update
    void Awake()
    {
        Init_skill_obj_dic();
    }

    // Update is called once per frame
    void Update()
    {
        skill_points_txt.text = m_skill_points.ToString();
    }

    // 스킬 오브젝트 배열 초기화
    void Init_skill_obj_dic()
    {
        Skill_slot_samurai[] samurai_skill_obj_arr = samurai_skill_inst.samurai_skill_group_parent.GetComponentsInChildren<Skill_slot_samurai>();
        Skill_slot_archer[] archer_skill_obj_arr = archer_skill_inst.archer_skill_group_parent.GetComponentsInChildren<Skill_slot_archer>();
        int samurai_skill_level = 0;
        int archer_skill_level = 0;
        
        // 사무라이 스킬 오브젝트 초기화
        foreach (var item in samurai_skill_obj_arr)
        {
            if (samurai_skill_level > (int) e_samurai_skill.MAX) break; // 예외처리
            samurai_skill_inst.samurai_skill_dic.Add((e_samurai_skill)samurai_skill_level,item.gameObject);
            samurai_skill_level++;
        }
        // 아처 스킬 오브젝트 초기화
        foreach (var item in archer_skill_obj_arr)
        {
            if (archer_skill_level > (int)e_archer_skill.MAX) break; // 예외처리
            archer_skill_inst.archer_skill_dic.Add((e_archer_skill)archer_skill_level, item.gameObject);
            archer_skill_level++;
        }
    }

    // 사무라이 스킬 업그레이드
    public void Upgrade_samurai_skill(int _samurai_skill_stage)
    {
        Skill_slot_samurai slot_samurai;
        Skill_slot_samurai before_slot_samurai;
        int before_slot_samurai_value = _samurai_skill_stage - 1;

        slot_samurai = samurai_skill_inst.samurai_skill_dic[(e_samurai_skill)_samurai_skill_stage]
            .GetComponent<Skill_slot_samurai>();

        if (before_slot_samurai_value < 0) // 예외처리
            before_slot_samurai = null;
        else
            before_slot_samurai = samurai_skill_inst.samurai_skill_dic[(e_samurai_skill)before_slot_samurai_value]
                .GetComponent<Skill_slot_samurai>();

        // 스킬 포인트 부족
        if (m_skill_points == 0)
        {
            UI_manager.instance.Run_no_skill_points_left_coroutine();
            return;
        }
        // 이전 스킬을 해방하지 않았음
        if (before_slot_samurai != null) // 예외 처리
        {
            if (!before_slot_samurai.is_unlocked)   // 스킬 잠금 해제
            {
                UI_manager.instance.Run_not_unlocked_last_skill_coroutine();
                return;
            }
        }
        // 스킬 레벨 예외처리
        if (slot_samurai.current_skill_level != slot_samurai.max_skill_level)
        {
            slot_samurai.is_unlocked = true;
            slot_samurai.current_skill_level++;
            m_skill_points--;
        }
    }
    // 아처 스킬 업그레이드
    public void Upgrade_archer_skill(int _archer_skill_stage)
    {
        Skill_slot_archer slot_archer;
        Skill_slot_archer before_slot_archer;
        int before_slot_samurai_value = _archer_skill_stage - 1;

        slot_archer = archer_skill_inst.archer_skill_dic[(e_archer_skill)_archer_skill_stage]
            .GetComponent<Skill_slot_archer>();

        if (before_slot_samurai_value < 0) // 예외처리
            before_slot_archer = null;
        else
            before_slot_archer = archer_skill_inst.archer_skill_dic[(e_archer_skill)before_slot_samurai_value]
                .GetComponent<Skill_slot_archer>();

        // 스킬 포인트 부족
        if (m_skill_points == 0)
        {
            UI_manager.instance.Run_no_skill_points_left_coroutine();
            return;
        }
        // 이전 스킬을 해방하지 않았음
        if (before_slot_archer != null) // 예외 처리
        {
            if (!before_slot_archer.is_unlocked)   // 스킬 잠금 해제
            {
                UI_manager.instance.Run_not_unlocked_last_skill_coroutine();
                return;
            }
        }
        // 스킬 레벨 예외처리
        if (slot_archer.current_skill_level != slot_archer.max_skill_level)
        {
            slot_archer.is_unlocked = true;
            slot_archer.current_skill_level++;
            m_skill_points--;
        }
    }
}

[System.Serializable]
public class Samurai_skill
{
    // 사무라이 스킬 부모
    public Transform samurai_skill_group_parent;
    // 사무라이 스킬 오브젝트 저장소
    public Dictionary<e_samurai_skill, GameObject> samurai_skill_dic = new Dictionary<e_samurai_skill, GameObject>();
}

[System.Serializable]
public class Archer_skill
{
    // 아처 스킬 부모
    public Transform archer_skill_group_parent;
    // 아처 스킬 오브젝트 저장소
    public Dictionary<e_archer_skill, GameObject> archer_skill_dic = new Dictionary<e_archer_skill, GameObject>();
}