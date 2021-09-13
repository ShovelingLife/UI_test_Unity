using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Crafting_recipee_data
{
    public Global_recipe_data[] arr;
}

public class Crafting_recipee : MonoBehaviour
{
    // 싱글톤
    static Crafting_recipee        m_instance;
    public static Crafting_recipee instance
    {
        get { return m_instance; }
    }
    public Dictionary<string, Global_recipe_data> current_items_data_dic = new Dictionary<string, Global_recipe_data>();
    // 전체 sprite 업로드
    //[SerializeField] Sprite[] all_item_sprites_arr;
    [Header("현재 제조 아이템 데이터")] 
    public Crafting_recipee_data global_recipe_data;

    // 최종 슬롯 관련
    Sprite               m_tmp_sprite;
    Crafting_result_slot m_tmp_slot;
    Crafting_setup       m_tmp_setup;
    const int k_total_crafting_slot_count = 9;
    string m_tmp_item_id = null;
    string m_current_item_id=null;
    string m_last_item_id = null;
    public string current_item_id_prop
    {
        get { return m_current_item_id; }
    }

    int m_current_item_quantity = 0;
    public int current_item_quantity_prop
    {
        get { return m_current_item_quantity; }
    }
    int m_quantity_needed_to_craft = 0;
    public bool is_made = false;


    private void Awake()
    {
        if (m_instance == null)
            m_instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Init_values();
    }

    // Update is called once per frame
    void Update()
    {
        Update_current_slot();
        Check_crafting_slot();
    }

    // 값 초기화
    void Init_values()
    {
        m_tmp_slot = Crafting_UI_manager.instance.crafting_result_slot;
        m_tmp_setup = Crafting_UI_manager.instance.crafting_setup;

        for (int i = 0; i < global_recipe_data.arr.Length; i++)
             current_items_data_dic.Add(global_recipe_data.arr[i].item_id, global_recipe_data.arr[i]);
    }

    // 제조창 감시
    void Check_crafting_slot()
    {
        for (int i = 0; i < k_total_crafting_slot_count; i++)
        {
            m_current_item_id += m_tmp_setup.crafting_slot_list[i].current_obj_id_prop;
            m_tmp_item_id += m_tmp_setup.crafting_slot_list[i].current_obj_id_prop + ",";
        }
    }

    // 최종 슬롯 업데이트 [아이템 아이디 바탕으로]
    void Update_current_slot()
    {
        if (!is_made) // 아이템이 제작 되었으면
        {
            m_tmp_slot.current_quantity_prop = 0;

            for (int i = 0; i < global_recipe_data.arr.Length; i++)
            {
                if (m_current_item_id == global_recipe_data.arr[i].item_id)
                {
                    m_tmp_sprite = current_items_data_dic[m_current_item_id].item_sprite;
                    m_current_item_quantity = current_items_data_dic[m_current_item_id].item_quantity;
                    m_quantity_needed_to_craft = current_items_data_dic[m_current_item_id].item_craft_count;
                    Crafting_UI_manager.instance.current_item_id_prop = m_tmp_item_id;
                    m_last_item_id = m_current_item_id;
                    is_made = true;
                }
            }
        }
        else
        {
            if (m_current_item_id != m_last_item_id) // 다른 아이템을 올렸을 시
            {
                is_made = false;
                m_tmp_slot.current_quantity_prop = 0;
            }
            else
            {
                int sum = Crafting_UI_manager.instance.Get_crafted_item_quantity();
                m_tmp_slot.current_image.sprite = m_tmp_sprite;
                m_tmp_slot.current_quantity_prop = m_current_item_quantity * (sum / m_quantity_needed_to_craft);
            }
        }
        m_current_item_id = null;
        m_tmp_item_id = null;
    }
}