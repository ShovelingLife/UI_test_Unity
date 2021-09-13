using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dropped_item : MonoBehaviour
{
    public RectTransform current_pos;
    public Image         current_image;
    public Text          quantity_txt;
    int                  m_current_quantity = 0;
    string               m_current_obj_type_id;

    public string current_obj_type_id_prop
    {
        get { return m_current_obj_type_id; }
        set { m_current_obj_type_id=value; }
    }

    public int current_quantity_prop
    {
        get { return m_current_quantity; }
        set { m_current_quantity = value; }
    }

    void Update()
    {
        if (GetComponent<Image>().sprite == null)
            gameObject.SetActive(false);

        if (m_current_quantity>0)
            quantity_txt.text = m_current_quantity.ToString();
    }

    // 값 다시 세팅
    public void Reset()
    {
        current_image.sprite = null;
        quantity_txt.text = "";
        m_current_quantity = 0;
    }

    // 초기화
    public void Init_values()
    {
        current_pos   = GetComponent<RectTransform>();
        current_image = GetComponent<Image>();
    }
}