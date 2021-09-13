using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Equipment_slot : MonoBehaviour
{
    public Image current_image;

    [SerializeField] 
    string       m_obj_id;

    public string obj_id_prop
    {
        get { return m_obj_id; }
    }
    
    // 사용 여부 관련
    [SerializeField] 
    GameObject m_mask_obj;

    [SerializeField] 
    GameObject m_current_equipped_slot_obj;  

    bool       m_is_equipped = false;

    public bool is_equipped_prop
    {
        get { return m_is_equipped; }
        set { m_is_equipped = value; }
    }

    private void Start()
    {
        Init_values();
    }

    // Update is called once per frame
    void Update()
    {
        Check_if_equipped();
    }

    // 값 초기화
    void Init_values()
    {
        m_mask_obj = transform.GetChild(0).gameObject;
        m_current_equipped_slot_obj = transform.GetChild(1).gameObject;
        m_mask_obj.SetActive(true);
        m_current_equipped_slot_obj.SetActive(false);

        current_image = m_current_equipped_slot_obj.GetComponent<Image>();

        string tmp_str = this.gameObject.name.ToLower();

        foreach (var item in tmp_str.ToCharArray())
        {
            if (item == '_')
                break;

            m_obj_id += item;
        }
    }

    // 장착했는지 확인
    void Check_if_equipped()
    {
        if (!m_is_equipped ||
            !current_image.sprite) // 장착 되어있지 않음
        {
            m_mask_obj.SetActive(true);
            m_current_equipped_slot_obj.SetActive(false);
        }
        else // 장착 되어있음
        {
            m_mask_obj.SetActive(false);
            m_current_equipped_slot_obj.SetActive(true);
        }
    }
}