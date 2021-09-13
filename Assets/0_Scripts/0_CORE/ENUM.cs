using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----- enum 전용 -----

public enum e_chat_state
{
    NONE=-1,
    INTRO_SCREEN,
    SIGN_UP_SCREEN,
    CHECK_SIGN_UP,
    EDIT_SCREEN,
    CHECK_EDIT_ID,
    CHECK_EDIT_INFO,
    LOGIN_SCREEN,
    CHATTING_SCREEN,
    LOGOUT,
    MAX
}

// 사무라이 스킬
public enum e_samurai_skill
{
    FIRST,
    FIRST_SUB,
    SECOND,
    THIRD,
    FOURTH,
    LAST,
    MAX
};

// 아처 스킬
public enum e_archer_skill
{
    FIRST,
    SECOND,
    THIRD,
    FOURTH,
    LAST,
    MAX
};

// 슬롯 종류
public enum e_slot_type
{
    FIRST_LEFT_CLICK,
    SECOND_LEFT_CLICK,
    RIGHT_CLICK
};