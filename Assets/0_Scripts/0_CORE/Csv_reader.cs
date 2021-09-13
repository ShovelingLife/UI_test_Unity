using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Csv_reader
{
    public Csv_loader_manager   csv_loader_manager;
    static string SPLIT_RE      = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS    = { '\"' };

    public static List<Dictionary<string, object>> Read(string _file)
    {
        var       list  = new List<Dictionary<string, object>>();
        TextAsset data  = Resources.Load(_file) as TextAsset;
        var       lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) 
            return list;

        var header = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            string[] values_arr = Regex.Split(lines[i], SPLIT_RE);

            if (values_arr.Length == 0 || 
                values_arr[0] == "") 
                continue;

            var entry = new Dictionary<string, object>();

            for (var j = 0; j < header.Length && j < values_arr.Length; j++)
            {
                string value = values_arr[j];

                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int    int_value;
                float  float_value;

                if (finalvalue.ToString() == "")
                    continue;

                if      (int.TryParse(value, out int_value))
                         finalvalue = int_value;

                else if (float.TryParse(value, out float_value))
                         finalvalue = float_value;

                if (!header[j].StartsWith("["))
                    entry[header[j]] = finalvalue;

            }
            list.Add(entry);
            //Csv_loader_manager.instance.Set_values_onto_dic(entry["MENU_TYPE"].ToString().ToLower(), i - 1);
        }
        return list;
    }
}