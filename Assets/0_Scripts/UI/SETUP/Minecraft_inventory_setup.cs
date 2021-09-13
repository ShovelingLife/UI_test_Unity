using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minecraft_inventory_setup : MonoBehaviour
{
    // 슬롯 오브젝트 담음
    List<Slot> m_inventory_slot_list = new List<Slot>();


    [ContextMenu("UI 인벤토리창 생성")]
    public void Create_crafting_set()
    {
        // 클리어 후 슬롯 생성
        m_inventory_slot_list.Clear();

        for (int i = 0; i < 27; i++)
        {
            // 슬롯 오브젝트 세팅
            GameObject tmp_obj = Instantiate(Crafting_UI_manager.instance.slot_obj, Crafting_UI_manager.instance.Get_parent_obj(1));
            tmp_obj.name = string.Format("Inventory_slot_{0}", i+1);

            // 슬롯 오브젝트 자식 세팅
            tmp_obj.transform.GetChild(0).name = string.Format("Inventory_child_slot_{0}", i + 1);
            m_inventory_slot_list.Add(tmp_obj.transform.GetChild(0).GetComponent<Slot>());
        }
    }

    [ContextMenu("UI 인벤토리창 삭제")]
    public void Delete_crafting_set()
    {
#if UNITY_EDITOR
        // 삭제 후 클리어
        for (int i = 0; i < m_inventory_slot_list.Count; i++)
             DestroyImmediate(m_inventory_slot_list[i]);

        m_inventory_slot_list.Clear();
#endif
    }
}