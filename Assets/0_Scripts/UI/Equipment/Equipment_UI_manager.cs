using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Equipment_UI_manager : Singleton_local<Equipment_UI_manager>, IPointerClickHandler, IPointerExitHandler
{
    Canvas            m_ui_canvas;
    GraphicRaycaster  m_gp_raycaster;
    // 아이콘 오브젝트
    public GameObject item_selected_obj;

    // 인벤토리 부모 오브젝트
    public Transform  inventory_obj_parent;

    // ------- 슬롯 관련 -------
    public GameObject            slot_obj;
    // 슬롯 집합체
    public RPG_inventory_setup         inventory_setup;
    Cursor_slot                  m_slot_cursor;
    Slot                         m_inventory_slot;

    // 장비창 슬롯
    Equipment_slot m_equipment_slot;
    Dropped_item   m_dropped_item;


    private void Start()
    {
        Init_values();
    }

    private void Update()
    {
        Inventory_select();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_slot_cursor.gameObject.transform.localPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<RaycastResult> ray_result_list = new List<RaycastResult>();
        eventData.position                  = Input.mousePosition;

        // 왼쪽 클릭, 아이템 이동
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            EventSystem.current.RaycastAll(eventData, ray_result_list);

            if (ray_result_list.Count == 0 &&
                m_slot_cursor.current_image.sprite != null) // 빈 공간 클릭 시
                Drop();

            foreach (var item in ray_result_list)
            {
                m_inventory_slot = item.gameObject.GetComponent<Slot>();
                m_equipment_slot = item.gameObject.GetComponent<Equipment_slot>();
                m_dropped_item   = item.gameObject.GetComponent<Dropped_item>();

                // 슬롯 정보를 가져왔을 시
                if (m_inventory_slot != null)
                {
                    Sprite first_sprite  = m_slot_cursor.current_image.sprite;
                    Sprite second_sprite = m_inventory_slot.current_image.sprite;

                    if (first_sprite == second_sprite) // 같으면 융합
                        Fuse();

                    else // 틀리면 교체
                        Swap();
                }
                if (m_equipment_slot != null) // 장비창 정보를 가져왔을 시
                {
                    if (!m_equipment_slot.is_equipped_prop) // 장비가 장착 되어있는지 판별
                        Equip_item();

                    else
                    {
                        // 아이디 같지 않고 커서가 널일때만 장비 해제
                        if (m_equipment_slot.obj_id_prop != m_slot_cursor.object_type_id_prop && 
                            m_slot_cursor.current_image.sprite==null) 
                            Unequip_item();

                        else
                            Swap_equipped_items();
                    }
                }
                if(m_dropped_item!=null) // 만약에 떨군 아이템이 있다면
                    Pick_up();
            }
        }
        // 오른쪽 클릭 아이템 놔둠
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("CLICKED UI 오른쪽");
            EventSystem.current.RaycastAll(eventData, ray_result_list);

            foreach (var item in ray_result_list)
            {
                m_inventory_slot = item.gameObject.GetComponent<Slot>();

                if (m_inventory_slot != null)
                    Set_one_item();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        item_selected_obj.SetActive(false);
    }

    // 초기화
    void Init_values()
    {
        m_slot_cursor   = GameObject.FindObjectOfType<Cursor_slot>();
        inventory_setup = GameObject.FindObjectOfType<RPG_inventory_setup>();
        m_ui_canvas     = GetComponent<Canvas>();
    }

    // 부모 오브젝트를 반환
    public Transform Get_parent_obj(int _index)
    {
        Transform tmp_transform = null;

        switch (_index)
        {
            case 0:
                tmp_transform = inventory_obj_parent;
                break;
        }
        return tmp_transform;
    }

    // 인벤토리에서 아이템 선택
    void Inventory_select()
    {
        PointerEventData pointer_data       = new PointerEventData(EventSystem.current);
        List<RaycastResult> ray_result_list = new List<RaycastResult>();
        pointer_data.position               = Input.mousePosition;
        EventSystem.current.RaycastAll(pointer_data, ray_result_list);

        //Debug.Log($"Pointer data : {ray_result_list.Count}");

        if (ray_result_list.Count <= 4)
        {
            item_selected_obj.SetActive(false);
            return;
        }
        foreach (var item in ray_result_list)
        {
            Slot tmp_slot = item.gameObject.GetComponent<Slot>();

            // 슬롯 정보를 가져왔을 시
            if (tmp_slot != null)
            {
                item_selected_obj.SetActive(true);
                item_selected_obj.transform.SetParent(tmp_slot.transform.parent);
                item_selected_obj.transform.localPosition = tmp_slot.transform.localPosition;
            }
        }
    }

    // 교체
    void Swap()
    {
        Debug.Log("Swap");
        Sprite tmp_sprite = m_slot_cursor.current_image.sprite;
        int tmp_value     = m_slot_cursor.current_quantity_prop;

        m_slot_cursor.current_image.sprite            = m_inventory_slot.current_image.sprite;
        m_slot_cursor.current_quantity_prop           = m_inventory_slot.current_quantity_prop;
        m_slot_cursor.object_type_id_prop             = m_inventory_slot.current_obj_type_id_prop;
        m_inventory_slot.GetComponent<Image>().sprite = tmp_sprite;
        m_inventory_slot.current_quantity_prop        = tmp_value;
    }

    // 융합
    void Fuse()
    {
        Debug.Log("Fuse");
        m_slot_cursor.current_quantity_prop    += m_inventory_slot.current_quantity_prop;
        m_inventory_slot.current_quantity_prop = 0;
        m_inventory_slot.current_image.sprite  = null;
    }

    // 아이템 한개만 설정
    void Set_one_item()
    {
        // 예외 처리
        if (m_slot_cursor.current_image.sprite != m_inventory_slot.current_image.sprite)
        {
            if (!m_inventory_slot.current_image.sprite)
                return;
        }
        m_slot_cursor.current_quantity_prop--;
        m_inventory_slot.current_quantity_prop++;
        m_inventory_slot.current_image.sprite = m_slot_cursor.current_image.sprite;

        if (m_slot_cursor.current_quantity_prop == 0)
            m_slot_cursor.current_image.sprite = null;
    }    

    // 아이템 떨굼
    void Drop()
    {
        Vector2 mouse_pos;
        Debug.Log("Drop");

        m_dropped_item = Object_pooling.instance.dropped_item_data.Get_obj().GetComponent<Dropped_item>();

        if (!m_dropped_item)
            return;

        // 설정 작업
        m_dropped_item.gameObject.SetActive(true);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ui_canvas.transform as RectTransform, Input.mousePosition, m_ui_canvas.worldCamera, out mouse_pos);
        m_dropped_item.current_pos.position     = m_ui_canvas.transform.TransformPoint(mouse_pos);
        m_dropped_item.current_image.sprite     = m_slot_cursor.current_image.sprite;
        m_dropped_item.current_quantity_prop    = m_slot_cursor.current_quantity_prop;
        m_dropped_item.current_obj_type_id_prop = m_slot_cursor.object_type_id_prop;

        // 초기화 작업
        m_slot_cursor.current_image.sprite = null;
        m_dropped_item                     = null;
    }

    // 아이템 가져옴
    void Pick_up()
    {
        Debug.Log("Pick_up");
        m_slot_cursor.current_image.sprite  = m_dropped_item.current_image.sprite;
        m_slot_cursor.current_quantity_prop = m_dropped_item.current_quantity_prop;
        m_slot_cursor.object_type_id_prop   = m_dropped_item.current_obj_type_id_prop;
        m_dropped_item.Reset();
    }

    // 아이템을 장비창에 등록
    void Equip_item()
    {
        Debug.Log("Equip_item");

        if (m_equipment_slot.obj_id_prop == m_slot_cursor.object_type_id_prop)
        {
            m_equipment_slot.current_image.sprite = m_slot_cursor.current_image.sprite;
            m_equipment_slot.is_equipped_prop     = true;
            m_slot_cursor.current_image.sprite    = null;
            m_slot_cursor.object_type_id_prop     = null;
        }
    }

    // 아이템을 장비창에서 꺼냄
    void Unequip_item()
    {
        Debug.Log("Unequip_item");
        m_slot_cursor.current_image.sprite  = m_equipment_slot.current_image.sprite;
        m_slot_cursor.object_type_id_prop   = m_equipment_slot.obj_id_prop;
        m_slot_cursor.current_quantity_prop = 1;
        m_equipment_slot.is_equipped_prop   = false;
    }

    // 아이템이 이미 장착 되어있을 시 교체
    void Swap_equipped_items()
    {
        Debug.Log("Equipment swap");
        Sprite tmp_sprite                     = m_slot_cursor.current_image.sprite;
        m_slot_cursor.current_image.sprite    = m_equipment_slot.current_image.sprite;
        m_equipment_slot.current_image.sprite = tmp_sprite;
        m_slot_cursor.current_quantity_prop   = 1;
    }
}