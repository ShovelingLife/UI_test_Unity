using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting_result_slot : Slot
{
    void Start()
    {
        current_image = GetComponent<Image>();
    }

    void Update()
    {
        base.Show_ui();
    }
}