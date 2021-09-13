using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----- 로컬 싱글톤 클래스 -----
//        씬 전환 시 파괴
//          재사용 불가
public class Singleton_local<T> : MonoBehaviour where T:MonoBehaviour
{
    static T _instance;

    public static T instance
    {
        get
        {
            GameObject singleton_obj = GameObject.FindObjectOfType<T>().gameObject;
            
            if (singleton_obj == null) 
                _instance = singleton_obj.AddComponent<T>();

            else 
                _instance = singleton_obj.GetComponent<T>();

            return _instance;
        }
    }
}