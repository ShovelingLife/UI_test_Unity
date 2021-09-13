using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Crafting_UI_manager : Singleton_local<Crafting_UI_manager>, IPointerEnterHandler, IPointerClickHandler
{
    Cursor_slot slot_cursor;
    // 아이콘 오브젝트
    public GameObject item_selected_obj;
    // 아이템 이동 패널
    //public Transform move_panel_trans;
    // 슬롯 오브젝트
    public GameObject slot_obj;

    // 제조 부모 오브젝트
    public Transform crafting_obj_parent;
    // 인벤토리 부모 오브젝트
    public Transform inventory_obj_parent;

    // ------- 슬롯 관련 -------

    // 슬롯 집합체
    public Crafting_setup crafting_setup;
    public Minecraft_inventory_setup inventory_setup;

    // 결과 슬롯 및 커서 슬롯
    Slot m_slot = new Slot();
    public Crafting_result_slot crafting_result_slot;

    // 레시피 관련
    Crafting_recipee crafting_recipee;
    string m_current_item_id;
    public string current_item_id_prop
    {
        set { m_current_item_id = value; }
    }


    private void Start()
    {
        Init_values();
    }

    private void Update()
    {
        slot_cursor.gameObject.transform.localPosition = Input.mousePosition;
    }    

    public void OnPointerClick(PointerEventData eventData)
    {
        List<RaycastResult> ray_result_list = new List<RaycastResult>();
        eventData.position = Input.mousePosition;

        //Debug.LogFormat($"클릭 위치: {Camera.main.ScreenToWorldPoint(Input.mousePosition)}");
        // 왼쪽 클릭, 아이템 이동
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            EventSystem.current.RaycastAll(eventData, ray_result_list);

            foreach (var item in ray_result_list)
            {
                m_slot = item.gameObject.GetComponent<Slot>();

                // 슬롯 정보를 가져왔을 시
                if (m_slot != null)
                {
                    if (m_slot == crafting_result_slot) // 현재 선택된 아이템이 없을 때만 교체
                    {
                        if (slot_cursor.current_image.sprite == null)
                        {
                            Get_crafted_item();
                            Swap();
                        }
                        return;
                    }
                    Sprite first_sprite = slot_cursor.current_image.sprite;
                    Sprite second_sprite = m_slot.current_image.sprite;

                    if (first_sprite == second_sprite) // 같으면 융합
                    {
                        crafting_result_slot.current_quantity_prop = 0;
                        Fuse();
                    }
                    else // 틀리면 교체
                        Swap();
                }
            }
        }
        // 오른쪽 클릭 아이템 놔둠
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("CLICKED UI 오른쪽");
            EventSystem.current.RaycastAll(eventData, ray_result_list);

            foreach (var item in ray_result_list)
            {
                m_slot = item.gameObject.GetComponent<Slot>();

                if (m_slot != null)
                    Set_one_item();
            }            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        List<RaycastResult> ray_result_list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, ray_result_list);

        foreach (var item in ray_result_list)
        {
            Debug.Log(item.gameObject.name);
            Slot tmp_slot = item.gameObject.GetComponent<Slot>();

            // 슬롯 정보를 가져왔을 시
            if (tmp_slot != null)
            {
                item_selected_obj.transform.SetParent(tmp_slot.transform.parent);
                item_selected_obj.transform.localPosition = tmp_slot.transform.localPosition;
            }
        }
    }

    // 초기화
    void Init_values()
    {
        slot_cursor = GameObject.FindObjectOfType<Cursor_slot>();
        crafting_result_slot = GameObject.FindObjectOfType<Crafting_result_slot>();
        crafting_recipee = GameObject.FindObjectOfType<Crafting_recipee>();
        crafting_setup = GameObject.FindObjectOfType<Crafting_setup>();
        inventory_setup = GameObject.FindObjectOfType<Minecraft_inventory_setup>();
    }

    // 부모 오브젝트를 반환
    public Transform Get_parent_obj(int _index)
    {
        Transform tmp_transform = null;

        switch (_index)
        {
            case 0:
                tmp_transform = crafting_obj_parent;
                break;

            case 1:
                tmp_transform = inventory_obj_parent;
                break;
        }
        return tmp_transform;
    }

    // 교체
    void Swap()
    {
        Debug.Log("교체");
        Sprite tmp_sprite = slot_cursor.current_image.sprite;
        int tmp_value = slot_cursor.current_quantity_prop;
        slot_cursor.current_image.sprite = m_slot.current_image.sprite;
        slot_cursor.current_quantity_prop = m_slot.current_quantity_prop;
        m_slot.GetComponent<Image>().sprite = tmp_sprite;
        m_slot.current_quantity_prop = tmp_value;
        crafting_result_slot.current_quantity_prop = 0;
    }

    // 융합
    void Fuse()
    {
        Debug.Log("융합");
        slot_cursor.current_quantity_prop += m_slot.current_quantity_prop;
        m_slot.current_quantity_prop=0;
        m_slot.current_image.sprite = null;
    }

    // 아이템 한개만 설정
    void Set_one_item()
    {
        // 예외 처리
        if (slot_cursor.current_image.sprite != m_slot.current_image.sprite)
        {
            if (m_slot.current_image.sprite!=null)
                return;
        }            
        slot_cursor.current_quantity_prop--;
        m_slot.current_quantity_prop++;
        m_slot.current_image.sprite = slot_cursor.current_image.sprite;

        if (slot_cursor.current_quantity_prop == 0)
            slot_cursor.current_image.sprite = null;
    }

    // 아이템 가져오기
    public void Get_crafted_item()
    {
        if (m_current_item_id == null)
            return;

        string[] final_item_arr = m_current_item_id.Split(',');

        for (int i = 0; i < final_item_arr.Length-1; i++)
        {
            if (final_item_arr[i]!="0") // 삭제
                crafting_setup.crafting_slot_list[i].current_quantity_prop = 0;
        }
        Crafting_recipee.instance.is_made = false;
    }

    // 현재 매칭되는 아이템 갯수 가져오기
    public int Get_crafted_item_quantity()
    {
        string[] final_item_arr = m_current_item_id.Split(',');
        int sum = 0;

        for (int i = 0; i < final_item_arr.Length - 1; i++)
        {
            if (final_item_arr[i] != "0") // 합산
                sum += crafting_setup.crafting_slot_list[i].current_quantity_prop;
        }
        return sum;
    }
}

//Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//RaycastHit hit;
//Physics.Raycast(ray,out hit, Mathf.Infinity, Global.Get_raycast_layermask_index("Slot"));\

// 교체 시작 부모 및 위치
//void Swap()
//{
//    Debug.Log("교체");
//    Slot_info tmp_slot_info = new Slot_info();
//    tmp_slot_info = m_first_slot;
//    m_first_slot.slot.transform.SetParent(m_last_slot.parent);
//    m_first_slot.slot.transform.localPosition = m_last_slot.current_pos;
//    m_last_slot.slot.transform.SetParent(tmp_slot_info.parent);
//    m_last_slot.slot.transform.localPosition = tmp_slot_info.current_pos;
//    m_click_count = 0;
//}