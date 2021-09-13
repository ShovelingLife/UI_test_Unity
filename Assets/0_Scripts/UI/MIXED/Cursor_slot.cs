using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor_slot : MonoBehaviour
{
    // 현재 사용하고 있음
    Canvas       m_ui_canvas;
    public Image current_image;
    public Text  current_txt;
    int          m_current_quantity = 0;

    [SerializeField] 
    string       m_object_type_id;

    Vector2      m_mouse_pos;
    public float mouse_speed = 500f;

    public int current_quantity_prop
    {
        get { return m_current_quantity;}
        set { m_current_quantity = value; } 
    }

    public string object_type_id_prop
    {
        get { return m_object_type_id; }
        set { m_object_type_id = value; }
    }


    private void Start()
    {
        current_image = GetComponent<Image>();
        m_ui_canvas   = transform.parent.GetComponent<Canvas>();
    }

    private void Update()
    {
        Show_image();
        Show_text();
        Follow_mouse();
    }

    // 이미지 표시
    void Show_image()
    {
        if (current_image.sprite == null)
        {
            current_image.color = Global.sprite_fade_color;
            m_current_quantity = 0;
        }
        else
            current_image.color = Global.original_color;
    }

    // 텍스트 표시
    void Show_text()
    {
        if (m_current_quantity == 0)
            current_txt.text = "";

        else
            current_txt.text = m_current_quantity.ToString();
    }

    // 마우스를 쫒아감
    void Follow_mouse()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_ui_canvas.transform as RectTransform, Input.mousePosition, m_ui_canvas.worldCamera, out m_mouse_pos);
        transform.position = m_ui_canvas.transform.TransformPoint(m_mouse_pos);
    }
}
