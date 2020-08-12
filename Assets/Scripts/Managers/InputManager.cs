using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public Action KeyAction = null;//Action은 일종의 델리게이트(함수들을 담는 가방)이다.
    public Action<Define.MouseEvent> MouseAction = null; //Define.MouseEvent타입의 파라미터를 필요로하는 함수를 호출하는 델리게이트

    bool _mousePressed = false;

    public void OnUpdate()
    {
        
        if(Input.anyKey && KeyAction != null)//어떠한 인풋이라도 있으면 델리게이트 인보크를 해준다.
        {
            KeyAction.Invoke();//델리게이트 브로드캐스팅 
            //여기서부터 호출스택을 따라가면 어떤 컴포넌트가 이 인풋을 수행하는지 알 수 있다.
            
        }
        

        if(MouseAction != null)
        {
            if(Input.GetMouseButton(0))
            {
                MouseAction.Invoke(Define.MouseEvent.Press);//이런식으로 델리게이터바인딩된 함수들에 인자를 전달하게되면,
                //각 함수에서 이 이벤트의 타입별로 실행해줄 메소드의 분기를 만들어서 관리 할 수 있다.
                _mousePressed = true;
            }
            else
            {
                if(_mousePressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.Click);
                    _mousePressed = false;
                }
            }
        }
    }
}
