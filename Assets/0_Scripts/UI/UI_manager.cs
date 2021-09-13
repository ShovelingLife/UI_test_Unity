using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_manager : Singleton_local<UI_manager>
{
    public Text skill_point_alert_msg_txt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 스킬 포인트 부족 코루틴 실행
    public void Run_no_skill_points_left_coroutine()
    {
        StartCoroutine(IE_no_skill_points_left());
    }

    // 이전 스킬 해지하지 않았음 코루틴 실행
    public void Run_not_unlocked_last_skill_coroutine()
    {
        StartCoroutine(IE_not_unlocked_last_skill());
    }

    // 스킬 포인트 부족 코루틴
    IEnumerator IE_no_skill_points_left()
    {
        skill_point_alert_msg_txt.text = Global.no_skill_points_left_msg;
        yield return new WaitForSeconds(2f);
        skill_point_alert_msg_txt.text = "";
    }

    // 이전 스킬 해지하지 않았음 코루틴
    IEnumerator IE_not_unlocked_last_skill()
    {
        skill_point_alert_msg_txt.text = Global.not_unlocked_skill_msg;
        yield return new WaitForSeconds(2f);
        skill_point_alert_msg_txt.text = "";
    }
}