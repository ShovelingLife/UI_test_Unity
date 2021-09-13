using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_slot_archer : MonoBehaviour
{
    public Text current_level_text;
    [SerializeField]
    private Image skill_img;
    // 스킬포인트 관련
    e_archer_skill m_archer_skill_stage;
    public int max_skill_level = 0;
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

        foreach (var item in Skill_slot_manager.instance.archer_skill_inst.archer_skill_dic)
        {
            if (item.Value.name == gameObject.name)
            {
                m_archer_skill_stage = item.Key;
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
