using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----- 전역 변수 및 전역 함수 클래스 -----
//        상속 금지, 인스턴스화 금지

public class Global
{
    // 색상
    readonly public static Color original_color = new Color(255, 255, 255, 255);
    readonly public static Color sprite_fade_color = new Color(255, 255, 255, 0);
    readonly public static Color icon_fade_color = new Color(255, 255, 255, 175);
    readonly public static Color equipment_fade_color = new Color(255, 255, 255, 100);

    // 스킬
    //readonly public static float skill_not_unlocked_value = 0.686f;

    // 텍스트
    readonly public static string no_skill_points_left_msg = "스킬 포인트 부족";
    readonly public static string not_unlocked_skill_msg =   "이전 스킬 해방 필요";

    // 방향
    readonly public static Vector3 left_up_diagonal_direction    = new Vector3(-1f, 1f);
    readonly public static Vector3 right_up_diagonal_direction   = new Vector3(1f, 1f);
    readonly public static Vector3 left_down_diagonal_direction  = new Vector3(-1f, -1f);
    readonly public static Vector3 right_down_diagonal_direction = new Vector3(1f, -1f);

    // 회전
    readonly public static Quaternion zero_rotation = new Quaternion();
    readonly public static Quaternion half_rotation = new Quaternion(0f,180f,0f,0f);

    // Z축 회전
    readonly public static Vector3 left_z_rotation = new Vector3(0f, 0f, 90f);
    readonly public static Vector3 down_z_rotation = new Vector3(0f, 0f, 180f);
    readonly public static Vector3 right_z_rotation = new Vector3(0f, 0f, 270f);

    // 레이캐스트 레이어 반환
    public static int Get_raycast_layermask_index(string _layer_mask)
    {
        return 1 << LayerMask.NameToLayer(_layer_mask);
    }

    // 전 방향 레이캐스트 테스트
    public static void Test_ray_all_direction(Transform _ray_target_trans)
    {
        // 여덟 방향 레이캐스트
        // \ ^ /       0 1 2
        // <   >       3   4
        // / v \       5 6 7
        Debug.DrawLine(_ray_target_trans.localPosition, left_up_diagonal_direction,Color.red); // -1, 1 (상단 왼쪽)
        Debug.DrawLine(_ray_target_trans.localPosition, right_up_diagonal_direction, Color.red); // 1 , 1 (상단 오른쪽)
        Debug.DrawLine(_ray_target_trans.localPosition, left_down_diagonal_direction, Color.red); // -1 , -1 (하단 왼쪽)
        Debug.DrawLine(_ray_target_trans.localPosition, right_down_diagonal_direction, Color.red); // 1 , -1 (하단 오른쪽)
        Debug.DrawLine(_ray_target_trans.localPosition, Vector3.up, Color.red); // 윗 방향
        Debug.DrawLine(_ray_target_trans.localPosition, Vector3.left, Color.red); // 왼쪽 방향
        Debug.DrawLine(_ray_target_trans.localPosition, Vector3.right, Color.red); // 오른쪽 방향
        Debug.DrawLine(_ray_target_trans.localPosition, Vector3.down, Color.red); // 아래 방향
    }

    // 전 방향 레이캐스트 지정
    public static void Get_raycast_all_direction(out Vector3[] _ray_pos_arr)
    {
         // 여덟 방향 레이캐스트
         // \ ^ /       0 1 2
         // <   >       3   4
         // / v \       5 6 7
        _ray_pos_arr = new Vector3[8];
        _ray_pos_arr[0] = left_up_diagonal_direction;
        _ray_pos_arr[1] = Vector3.up;
        _ray_pos_arr[2] = right_up_diagonal_direction;
        _ray_pos_arr[3] = Vector3.left;
        _ray_pos_arr[4] = Vector3.right;
        _ray_pos_arr[5] = left_down_diagonal_direction;
        _ray_pos_arr[6] = Vector3.down;
        _ray_pos_arr[7] = right_down_diagonal_direction;
    } 
}