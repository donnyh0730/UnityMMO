using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager s_Instance;
    public static Manager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                ManagerInit();
            }
            return s_Instance;
        }
    }

    InputManager _input = new InputManager();//프로그램이 실행될때 생성되서 올라간다.
    ResourceManager _resource = new ResourceManager();

    public static InputManager Input{get{return Instance._input;}}
    public static ResourceManager Resourcemanager { get { return Instance._resource; } }


    static void ManagerInit()
    {
        GameObject go = GameObject.Find("Manager");
        if (go == null)
        {
            go = new GameObject { name = "Manager" };
            go.AddComponent<Manager>();
        }

        DontDestroyOnLoad(go);//씬변경이 일어나도 파괴되지 않고 남아있게해주는 함수이다.
        s_Instance = go.GetComponent<Manager>();
    }
    void Start()
    {
        //Init();
    }

    // Update is called once per frame
    void Update()
    {
        _input.OnUpdate();
    }


}
