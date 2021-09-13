using System;
using System.Collections.Generic;
using UnityEngine;

public class Object_pooling : Singleton_local<Object_pooling>
{
    // 플레이어 파워업 아이템 관련
    [Header("플레이어 드랍 아이템 관련")]
    public Player_dropped_item_data dropped_item_data;

    void Start()
    {
        Init_class_members();
        Init_obj_pooling();
    }

    // 클래스 변수 초기화
    void Init_class_members()
    {
    }

    // 풀링 오브젝트 초기화
    void Init_obj_pooling()
    {
        Init_player_dropped_items_obj();
    }

    // 캐릭터가 드랍한 아이템 컨테이너 초기화
    public void Init_player_dropped_items_obj()
    {
        for (int i = 0; i < dropped_item_data.dropped_item_quantity; i++)
        {
            GameObject tmp_obj = Instantiate(dropped_item_data.dropped_item_prefab, dropped_item_data.dropped_item_container);

            tmp_obj.GetComponent<Dropped_item>().Init_values();
            tmp_obj.SetActive(false);
            dropped_item_data.player_dropped_item_list.Add(tmp_obj);
        }
    }
}

[Serializable]
public class Player_dropped_item_data // 플레이어가 드랍한 아이템 클래스
{
    [Header("플레이어가 드랍한 아이템")]
    public int dropped_item_quantity = 20;
    public List<GameObject> player_dropped_item_list = new List<GameObject>();
    public GameObject dropped_item_prefab;
    public Transform dropped_item_container;


    // 드랍한 아이템 가져옴
    public GameObject Get_obj()
    {
        foreach (var item in player_dropped_item_list)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                return item;
            }
        }
        return null;
    }

    // 드랍한 아이템을 다시 컨테이너에 넣음
    public void Set_item_obj(Dropped_item item_data)
    {
        item_data.Reset();
        item_data.gameObject.SetActive(false);
    }
}