using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_property : MonoBehaviour
{
    Text       m_current_txt;
    public int hash_code;


    void Start()
    {
        m_current_txt = GetComponent<Text>();
    }

    void Update()
    {
        m_current_txt.text = Csv_loader_manager.instance[hash_code].ToString();
    }
}

// ------- HASH CODE -------

// 0 = INTRO_BUTTON	
// 1 = PARK_BUTTON
// 2 = CITY_BUTTON
// 3 = RESIDENT_BUTTON
// 4 = AUTOMATION_BUTTON
// 5 = UPGRADE
// 6 = PER_SECOND
// 7 = LEVEL
// 8 = MAX_LEVEL
// 9 = CLAIM
// 10 = DAILY_REWARD
// 11 = WELCOME_BACK
// 12 = REWARD
// 13 = MAX_MONEY