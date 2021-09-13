using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_slot_samurai : MonoBehaviour
{
    public Text current_level_text;
    [SerializeField]
    private Image skill_img;
    // 스킬포인트 관련
    e_samurai_skill m_samurai_skill_stage;
    public int max_skill_level=0;
    public int current_skill_level;
    public bool is_unlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        Init_values();
    }

    // Update is called once per frame
    void Update()
    {
        Update_skill_info();
    }

    // 값 초기화
    void Init_values()
    {
        skill_img = GetComponent<Image>();

        foreach (var item in Skill_slot_manager.instance.samurai_skill_inst.samurai_skill_dic)
        {
            if (item.Value.name==gameObject.name)
            {
                m_samurai_skill_stage = item.Key;
                return;
            }
        }
    }

    // 스킬 해방됐는지 체크
    void Update_skill_info()
    {
        if (is_unlocked) skill_img.color = Global.original_color;
        if (current_skill_level != max_skill_level)
            current_level_text.text = current_skill_level.ToString() + "/" + max_skill_level.ToString();
        else current_level_text.text = "최대";
    }
}

//public class Skill_slot_samurai : MonoBehaviour
//{
//    public Text current_level_text;
//    // 스킬포인트 관련
//    e_samurai_skill m_samurai_skill_stage;
//    public int max_skill_level = 0;
//    public int current_skill_level;


//    // Start is called before the first frame update
//    void Start()
//    {
//        Init_values();
//        Debug.Log(m_samurai_skill_stage);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        current_level_text.text = current_skill_level.ToString() + "/" + max_skill_level.ToString();
//    }

//    // 값 초기화
//    void Init_values()
//    {
//        foreach (var item in Skill_slot_manager.instance.samurai_skill_inst.samurai_skill_dic)
//        {
//            if (item.Value.name == gameObject.name)
//            {
//                m_samurai_skill_stage = item.Key;
//                return;
//            }
//        }
//    }

//    public void Upgrade_skill()
//    {
//        Skill_slot_manager.instance.Upgrade_samurai_skill(m_samurai_skill_stage);
//    }
//}