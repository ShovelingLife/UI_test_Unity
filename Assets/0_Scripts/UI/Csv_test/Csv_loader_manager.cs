using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Csv_loader_manager : Singleton_local<Csv_loader_manager>
{
    // ------- Variables -------
    List<Dictionary<string, object>> m_csv_list         = new List<Dictionary<string, object>>();
    string                           m_current_language = "english";

    // Accessing to m_csv_list by value
    public object this[int _hash]
    {
        get { return m_csv_list[_hash][m_current_language.ToUpper()]; }
    }


    void Start()
    {
        m_csv_list = Csv_reader.Read("Csv_files/Growing_city_translation");

        //foreach (var item in m_csv_list)
        //{
        //    Debug.Log($"English : {item["ENGLISH"]}");
        //    Debug.Log($"Spanish : {item["SPANISH"]}");
        //    Debug.Log($"Korean : {item["KOREAN"]}");
        //}
    }

    // Set current language
    public void Set_language(string _language)
    {
        m_current_language = _language;
    }
}
