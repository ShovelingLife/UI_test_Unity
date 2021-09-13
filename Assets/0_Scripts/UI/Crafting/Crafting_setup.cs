using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting_setup : MonoBehaviour
{
    readonly int k_total_slots = 9;
    // 슬롯 오브젝트 담음
    public List<Slot> crafting_slot_list = new List<Slot>();

    [ContextMenu("UI 제작창 생성")]
    public void Create_crafting_set()
    {
        // 클리어 후 슬롯 생성
        crafting_slot_list.Clear();

        for (int i = 0; i < k_total_slots; i++)
        {
            // 슬롯 오브젝트 세팅
            GameObject tmp_obj = Instantiate(Crafting_UI_manager.instance.slot_obj, Crafting_UI_manager.instance.Get_parent_obj(0));
            tmp_obj.name = string.Format("Crafting_parent_slot_{0}", i+1);

            // 슬롯 오브젝트 자식 세팅
            tmp_obj.transform.GetChild(0).name= string.Format("Crafting_child_slot_{0}", i+1);

            crafting_slot_list.Add(tmp_obj.transform.GetChild(0).GetComponent<Slot>());
        }
    }

    [ContextMenu("UI 제작창 삭제")]
    public void Delete_crafting_set()
    {
#if UNITY_EDITOR
        // 삭제 후 클리어
        for (int i = 0; i < crafting_slot_list.Count; i++)
        {
            DestroyImmediate(crafting_slot_list[i]);
        }
        crafting_slot_list.Clear();
#endif
    }
}