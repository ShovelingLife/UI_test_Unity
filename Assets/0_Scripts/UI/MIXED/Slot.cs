using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Slot : MonoBehaviour
{
    public Image current_image;
    public Text quantity_txt;
    int m_current_quantity = 0;
    public bool is_test = false;
    string m_current_obj_id;
    string m_current_obj_type_id;

    public string current_obj_id_prop
    {
        get { return m_current_obj_id; }
    }

    public string current_obj_type_id_prop
    {
        get { return m_current_obj_type_id; }
    }

    public int current_quantity_prop
    {
        get { return m_current_quantity;}
        set { m_current_quantity = value;}
    }


    private void Start()
    {
        if (is_test)
            current_quantity_prop = 1;

        Init_values();
    }
    
    void Update()
    {
        Show_ui();

        if (current_image.sprite!=null)
        {
            Item_id_update();
            Update_item_type_id();
        }
    }

    // 값 초기화
    protected void Init_values()
    {
        current_image = this.GetComponent<Image>();

        if (current_image.sprite == null)
            current_image.color = Global.sprite_fade_color;

        else
            current_image.color = Global.original_color;
    }

    // 값 다시 세팅
    public void Reset()
    {
        current_image.sprite = null;
        quantity_txt.text = "";
        m_current_quantity = 0;
    }

    // 현재 갯수를 보여줌
    public void Show_ui()
    {
        if (m_current_quantity == 0)
        {
            current_image.color = Global.sprite_fade_color;
            current_image.sprite = null;
            quantity_txt.text = null;
        }
        else
        {
            current_image.color = Global.original_color;
            quantity_txt.text   = m_current_quantity.ToString();
        }
    }

    // 아이디 업데이트
    void Item_id_update()
    {
        string current_image_name;

        if (current_image.sprite == null)
        {
            m_current_obj_id = "0";
            return;
        }
        else
            current_image_name = current_image.sprite.name;

        // 약자
        // w = wood
        // sw= spider web
        // g = gold
        // fr = fishing rod

        // EX, not final
        switch (current_image_name)
        {
            case "spritesheet_107": // 기본 나무

                m_current_obj_id = "w1";
                break;

            case "spritesheet_109": // 기본 나무 막대 

                m_current_obj_id = "w2";
                break;

            case "spritesheet_187": // 거미줄

                m_current_obj_id = "sw1";
                break;

            case "spritesheet_113": // 금 소형 덩어리

                m_current_obj_id = "g1";
                break;

            case "spritesheet_205": // 기본 낚싯대

                m_current_obj_id = "fr1";
                break;

            default:

                m_current_obj_id = "0";
                break;
        }
    }

    // 아이템 타입 업데이트
    void Update_item_type_id()
    {
        // 장비 창
        // 장신구 헬멧 장갑
        // 배낭   자킷 작전배낭
        // 총기1  바지 신발     총기2

        string current_sprite_name = GetComponent<Image>().sprite.name;

        // 테스트용
        // 직관성과 하드코딩 우려로 인해 보다 더
        // 유연한 시스템 구현 필요
        switch (current_sprite_name)
        {
            case "spritesheet_127":

                m_current_obj_type_id = "eyewear";
                break;

            case "spritesheet_192":

                m_current_obj_type_id = "helmet";
                break;

            case "spritesheet_167":

                m_current_obj_type_id = "gloves";
                break;

            case "spritesheet_180":

                m_current_obj_type_id = "backpack";
                break;

            case "spritesheet_120":

                m_current_obj_type_id = "shirt";
                break;

            case "spritesheet_282":

                m_current_obj_type_id = "tactical";
                break;

            case "spritesheet_17":
            case "spritesheet_22":

                m_current_obj_type_id = "primary";
                break;

            case "spritesheet_232":

                m_current_obj_type_id = "pants";
                break;
                
            case "spritesheet_206":

                m_current_obj_type_id = "shoes";
                break;

            case "spritesheet_60":

                m_current_obj_type_id = "secondary";
                break;
        }
    }
}