using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Global_recipe_data", menuName = "Create_scriptable_global_recipe_data/global_recipe_data", order = 0)]
public class Global_recipe_data : ScriptableObject
{
    public Sprite item_sprite;
    public string item_id;
    public int    item_quantity;
    public int    item_craft_count;

    //[EX_ITEM_ID]
    //[0] 1	 [3] 4]	 [6] 7]
    //[1] 2	 [4] 5]	 [7] 8]
    //[2] 3	 [5] 6]	 [8] 9]
}