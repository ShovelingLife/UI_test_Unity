using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chatting_manager : Singleton_local<Chatting_manager>
{
    public e_chat_state current_state = e_chat_state.INTRO_SCREEN;

    // ------- BASE VARIABLES -------
    [Header("Current_info")]
    public User_info    user_info;
    public Canvas       sign_up_canvas;
    public Canvas       login_canvas;
    public Canvas       edit_canvas;
    public Canvas       chat_canvas;

    // ------- SIGN UP -------
    [Header("Sign_up_info")]
    public InputField sign_up_id_field;
    public InputField sign_up_pw_field;
    public InputField nickname_field;
    public Button     sign_up_accept_button;
    public Button     sign_up_cancel_button;

    // ------- LOGIN -------
    [Header("Login_info")]
    public InputField login_id_field;
    public InputField login_pw_field;
    public Button     sign_up_button;
    public Button     edit_button;
    public Button     login_button;

    // ------- EDIT -------
    [Header("Edit_info")]
    public InputField[] pw_edit_field_arr=new InputField[3];
    public Button       edit_accept_button;
    public Button       edit_cancel_button;

    // ------- CHAT -------
    [Header("Chat_info")]
    public Text       chat_txt;
    public InputField chat_field;
    public Button     intro_button;
    public Button     exit_button;



    void Awake()
    {
        Set_up_buttons();
        user_info.Init();
    }

    void Start()
    {
        Load_login_screen();
        
    }

    private void Update()
    {
        user_info.alert_txt_obj.transform.localPosition = new Vector3(0f, 823f, 0f);
        user_info.alert_txt_obj.transform.localScale    = new Vector3(1f, 1f, 1f);

        if (user_info.alert_msg_prop != "")
            StartCoroutine(user_info.IE_show_msg());

        if (current_state != e_chat_state.NONE)
        {
            user_info.chat_state_dic[current_state].Invoke();
            Reset_id_pw_inputfield();
            Reset_sign_up_inputfield();
            current_state = e_chat_state.NONE;
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Account_count", user_info.account_count_prop);
        PlayerPrefs.Save();
    }

    // Setting buttons
    void Set_up_buttons()
    {
        // Sign_up
        sign_up_accept_button.onClick.AddListener(() => current_state = e_chat_state.CHECK_SIGN_UP);
        sign_up_cancel_button.onClick.AddListener(() => current_state = e_chat_state.INTRO_SCREEN);

        // Login
        sign_up_button.onClick.AddListener(() => current_state = e_chat_state.SIGN_UP_SCREEN);
        edit_button.onClick.AddListener(() => current_state = e_chat_state.CHECK_EDIT_ID);
        login_button.onClick.AddListener(() => current_state = e_chat_state.LOGIN_SCREEN);

        // Edit
        edit_accept_button.onClick.AddListener(() => current_state = e_chat_state.CHECK_EDIT_INFO);
        edit_cancel_button.onClick.AddListener(() => current_state = e_chat_state.INTRO_SCREEN);

        // Chat
        intro_button.onClick.AddListener(() => current_state = e_chat_state.INTRO_SCREEN);
        exit_button.onClick.AddListener(() => current_state = e_chat_state.LOGOUT);
    }

    // Check Id inputfield (sign_up)
    public bool Is_sign_up_id_field_empty()
    {
        return sign_up_id_field.text == null || sign_up_id_field.text == "";
    }

    // Check Pw inputfield (sign_up)
    public bool Is_sign_up_pw_field_empty()
    {
        return sign_up_pw_field.text == null || sign_up_pw_field.text == "";
    }

    // Check Nickname inputfield (sign_up)
    public bool Is_sign_up_nickname_field_field_empty()
    {
        return nickname_field.text == null || nickname_field.text == "";
    }

    // Check Id inputfield (login)
    public bool Is_login_id_field_empty()
    {
        return login_id_field.text == null || login_id_field.text == "";
    }

    // Check Pw inputfield (login)
    public bool Is_login_pw_field_empty()
    {
        return login_pw_field.text == null || login_pw_field.text == "";
    }

    // Update inputfield and text
    public void Update_text()
    {
        chat_txt.text += string.Format("{0} : {1} \n", user_info.nickname_prop, chat_field.text);
        chat_field.text = null;
        chat_field.ActivateInputField();
    }

    // Turning off all canvas
    void Turn_off_all_canvas()
    {
        sign_up_canvas.gameObject.SetActive(false);
        login_canvas.gameObject.SetActive(false);
        edit_canvas.gameObject.SetActive(false);
        chat_canvas.gameObject.SetActive(false);
    }

    // Sign_up screen
    public void Load_sign_up_screen()
    {
        Turn_off_all_canvas();
        sign_up_canvas.gameObject.SetActive(true);
        user_info.alert_txt_obj.transform.SetParent(sign_up_canvas.transform);
    }

    // Login screen
    public void Load_login_screen()
    {
        Turn_off_all_canvas();
        login_canvas.gameObject.SetActive(true);
        user_info.alert_txt_obj.transform.SetParent(login_canvas.transform);
        chat_field.text = "";
        chat_txt.text   = "";
    }

    // Edit screen
    public void Load_edit_screen()
    {
        Turn_off_all_canvas();
        edit_canvas.gameObject.SetActive(true);
        user_info.alert_txt_obj.transform.SetParent(edit_canvas.transform);
    }

    // Chat screen
    public void Load_chat_screen()
    {
        Turn_off_all_canvas();
        chat_canvas.gameObject.SetActive(true);
        user_info.alert_txt_obj.transform.SetParent(chat_canvas.transform);
    }

    // Reset id,pw inputfield
    public void Reset_id_pw_inputfield()
    {
        login_id_field.text = "";
        login_pw_field.text = "";
    }

    // Reset sign_up inputfield
    public void Reset_sign_up_inputfield()
    {
        sign_up_id_field.text = "";
        sign_up_pw_field.text = "";
        nickname_field.text   = "";
    }

    // Logout
    public void Log_out()
    {
#if   UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }

}

[System.Serializable]
public class User_info
{
    public Dictionary<e_chat_state, Action> chat_state_dic      = new Dictionary<e_chat_state, Action>();
    Dictionary<string, string>              m_account_info_dic  = new Dictionary<string, string>();
    Dictionary<string, string>              m_nickname_info_dic = new Dictionary<string, string>();

    // Current id and password
    string m_id, m_pw, m_nickname;   
    public string nickname_prop
    {
        get { return m_nickname; }
    }

    // ------- ALERT RELATED -------
    public GameObject  alert_txt_obj;
    [SerializeField]
    string             m_alert_msg;

    public string alert_msg_prop
    {
        get { return m_alert_msg; }
    }
    float m_time          = 4f;

    // Account quantity
    int   m_account_count = 0;
    public int account_count_prop
    {
        get { return m_account_count; }
    }

    // Initialize chat_state_dic
    public void Init()
    {
        //chat_state_dic[e_chat_state.NONE]          = ()=> { Debug.Log("Waiting"); };
        chat_state_dic[e_chat_state.INTRO_SCREEN]    = Chatting_manager.instance.Load_login_screen;
        chat_state_dic[e_chat_state.SIGN_UP_SCREEN]  = Chatting_manager.instance.Load_sign_up_screen;
        chat_state_dic[e_chat_state.CHECK_SIGN_UP]   = Sign_up;
        chat_state_dic[e_chat_state.EDIT_SCREEN]     = Chatting_manager.instance.Load_edit_screen;
        chat_state_dic[e_chat_state.CHECK_EDIT_ID]   = Check_if_id_exists;
        chat_state_dic[e_chat_state.CHECK_EDIT_INFO] = Edit;
        chat_state_dic[e_chat_state.LOGIN_SCREEN]    = Log_in;
        chat_state_dic[e_chat_state.CHATTING_SCREEN] = Chatting_manager.instance.Load_chat_screen;
        chat_state_dic[e_chat_state.LOGOUT]          = Chatting_manager.instance.Log_out;

        m_account_count = PlayerPrefs.GetInt("Account_count");
        // Initializing array
        for (int i = 1; i <= m_account_count; i++)
        {
            string tmp_id       = PlayerPrefs.GetString($"Id_{m_account_count}");
            string tmp_pw       = PlayerPrefs.GetString($"Pw_{m_account_count}");
            string tmp_nickname = PlayerPrefs.GetString($"Nickname_{m_account_count}");

            if (tmp_id == "")
                break;

            m_account_info_dic.Add(tmp_id, tmp_pw);
            m_nickname_info_dic.Add(tmp_id, tmp_nickname);
        }
    }

    // Check if not empty
    void Check_login_field()
    {
        bool is_id = Chatting_manager.instance.Is_login_id_field_empty();
        bool is_pw = Chatting_manager.instance.Is_login_pw_field_empty();

        // Checking for field
        if      (is_id &&
                 !is_pw)
                 m_alert_msg = "Check id.";

        else if (is_pw &&
                 !is_id)
                 m_alert_msg = "Check pw.";

        else if (is_id &&
                 is_pw)
                 m_alert_msg = "Check id and pw.";
    }

    // Checking sign_up field
    bool Check_sign_up_field()
    {
        bool is_id       = Chatting_manager.instance.Is_sign_up_id_field_empty();
        bool is_pw       = Chatting_manager.instance.Is_sign_up_pw_field_empty();

        // Checking for field
        if      (is_id &&
                 !is_pw)
                 m_alert_msg = "Check id.";

        else if (is_pw &&
                 !is_id)
                 m_alert_msg = "Check pw.";

        else if (is_id &&
                 is_pw)
                 m_alert_msg = "Check id and pw.";

        if (m_alert_msg != "")
            return false;

        else
            return true;
    }

    // Trying to sign_up
    void Sign_up()
    {
        if (!Check_sign_up_field())
            return;

        m_id       = Chatting_manager.instance.sign_up_id_field.text;
        m_pw       = Chatting_manager.instance.sign_up_pw_field.text;
        m_nickname = Chatting_manager.instance.nickname_field.text;

        if (!Check_id())
        {
            if(!m_account_info_dic.ContainsKey(m_id))
                m_account_info_dic.Add(m_id, m_pw);

            if(!m_nickname_info_dic.ContainsKey(m_id))
                m_nickname_info_dic.Add(m_id,m_nickname);

            m_account_count++;
            PlayerPrefs.SetString($"Id_{m_account_count}", m_id);
            PlayerPrefs.SetString($"Pw_{m_account_count}", m_pw);
            PlayerPrefs.SetString($"Nickname_{m_account_count}", m_nickname);
            m_alert_msg   = "Successfully created";
            chat_state_dic[e_chat_state.INTRO_SCREEN].Invoke();
        }
        else
        {
            Check_login_field();

            if (m_alert_msg == "")
                m_alert_msg = "Id already exists";

            chat_state_dic[e_chat_state.SIGN_UP_SCREEN].Invoke();
        }
    }

    // Check if exists Id
    void Check_if_id_exists()
    {
        m_id = Chatting_manager.instance.login_id_field.text;

        if (m_id == "" ||
            !m_account_info_dic.ContainsKey(m_id))
        {
            m_alert_msg = "Invalid ID";
            return;
        }
        else
            chat_state_dic[e_chat_state.EDIT_SCREEN].Invoke();
    }

    // Editing
    public void Edit()
    {
        // Check for pw
        m_pw = Chatting_manager.instance.pw_edit_field_arr[0].text;

        if (m_pw != m_account_info_dic[m_id])
            m_alert_msg = "Wrong password";

        else
        {
            string new_pw     = Chatting_manager.instance.pw_edit_field_arr[1].text;
            string confirm_pw = Chatting_manager.instance.pw_edit_field_arr[2].text;

            if (new_pw != confirm_pw)
                m_alert_msg = "Password mismatch";

            else
            {
                m_account_info_dic[m_id] = new_pw;
                chat_state_dic[e_chat_state.INTRO_SCREEN].Invoke();

                for (int i = 0; i < 3; i++)
                     Chatting_manager.instance.pw_edit_field_arr[i].text = "";
            }
        }
    }

    // Trying to log in
    public void Log_in()
    {
        Check_login_field();
        m_id = Chatting_manager.instance.login_id_field.text;
        m_pw = Chatting_manager.instance.login_pw_field.text;

        // Checking for ID_PW login
        if (Check_id() &&
            Check_pw())
            chat_state_dic[e_chat_state.CHATTING_SCREEN].Invoke();

        else
            chat_state_dic[e_chat_state.INTRO_SCREEN].Invoke();
    }

    // Checking current id
    bool Check_id()
    {
        // If has the key
        if (m_account_info_dic.ContainsKey(m_id))
        {
            m_nickname = m_nickname_info_dic[m_id];
            return true;
        }
        else
        {
            if (Chatting_manager.instance.current_state == e_chat_state.LOGIN_SCREEN &&
                m_alert_msg == "")
                m_alert_msg = "Id not registered.";

            return false;
        }
    }

    // Checking current pw
    bool Check_pw()
    {
        // If has the key
        if (m_account_info_dic[m_id] == m_pw)
            return true;

        else
        {
            Wrong_pw();
            return false;
        }
    }

    // Checking duplicated id
    void Duplicated_id()
    {
        m_alert_msg = "Duplicated ID";
        Chatting_manager.instance.Reset_id_pw_inputfield();
    }

    // Checking wrong pw
    void Wrong_pw()
    {
        m_alert_msg = "Wrong Password";
        Chatting_manager.instance.Reset_id_pw_inputfield();
    }

    // An alert for msg
    public IEnumerator IE_show_msg()
    {
        alert_txt_obj.SetActive(true);
        alert_txt_obj.GetComponent<Text>().text = m_alert_msg;
        m_alert_msg = "";
        yield return new WaitForSeconds(m_time);
        alert_txt_obj.SetActive(false);
    }
};