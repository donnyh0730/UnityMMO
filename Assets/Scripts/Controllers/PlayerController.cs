using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]//객체지향적인 관점에서 프라이빗 변수로 선언을 하더라도 에디터상에서 변수를 수정 하고 싶으면 SerializeField어트리뷰트를 달아주면 된다. 
    private float Speed = 10.0f;

    bool b_moving = false;
    bool b_mouseMove = false;
    Vector3 desPos;

    public CameraController cameraController;
    float wait_run_ratio = 0;
    void Start()
    {
        Manager.Input.KeyAction -= OnKeyBoard;//혹시라도 다른곳에서 같은함수로 구독신청이 되어있다면 일단 한번 없애고 새로 할당한다.
        Manager.Input.KeyAction += OnKeyBoard;//인풋매니저에있는 KeyAction델리게이트에 이벤트 구독신청을 하게된다. 만약 인풋이 있다면 "내가 인풋을 구독하고 있으니 인풋이있다면 OnkeyBoard를 호출해주세요." 라고 하는 것이다.
        Manager.Input.MouseAction -= OnMouseClicked;
        Manager.Input.MouseAction += OnMouseClicked;
    }
    
    void Update()
    {
        if(b_mouseMove)
        {
            Vector3 dir = desPos - transform.position;
            if(dir.magnitude < 0.001f)
            {
                b_mouseMove = false;
            }
            else
            {
                float movedist = Mathf.Clamp(Speed * Time.deltaTime,0,dir.magnitude);
                transform.position += dir.normalized * movedist;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
                //transform.LookAt(desPos);
                //cameraController.UpdateCameraPostion();
            }
        }
        if(b_mouseMove)
        {
            wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10 * Time.deltaTime);
            Animator anim = GetComponent<Animator>();
            anim.SetFloat("wait_run_ratio", wait_run_ratio);
            anim.Play("Wait_Run");
        }
        else
        {
            wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10 * Time.deltaTime);
            Animator anim = GetComponent<Animator>();
            anim.SetFloat("wait_run_ratio", wait_run_ratio);
            anim.Play("Wait_Run");
        }
        //Animator anim = GetComponent<Animator>();
        //anim.Play("RUN");

    }

    void OnKeyBoard()//이함수는 이벤트 구독을 통해 update주기로 동작합니다.
    {
        //TransformDirection : 로컬 -> 월드
        //InverseTransformDirection : 월드 -> 로컬 

        //transform.eulerAngles = new Vector3(0.0f, yAngle, 0.0f);
        //transform.Rotate(new Vector3(0.0f, Time.deltaTime * 360.0f, 0.0f));

        //transform.rotation = Quaternion.Euler(new Vector3(0.0f, yAngle, 0.0f));//오일러 앵글을 넣으면 그 값을 쿼터니언 타입으로 바꿔주는 함수이다
        //예를들면 오일러앵글인 (0,yangle,0)의 의미는 y축을 기준으로 yangle 만큼 회전한 값을 말하며, 이것을 쿼터니언으로 표현하면 (0,1,0,yangle)로 표현된다.
        //transform.rotation 이란 값은 쿼터니언 타입으로 정의되어있다. 오일러앵글이아닌 쿼터니언을 사용하는 이유는 짐벌락 문제를 해결하기위함 이다.
        //따라서 transform.rotation값을 할당하고 싶다면 위와같이 오일러앵글을 쿼터니언으로 바꾸어주는 함수를 이용하면된다.

        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            //transform.Translate(Vector3.forward * Time.deltaTime * Speed);
            transform.position += Vector3.forward * Time.deltaTime * Speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            //transform.Translate(Vector3.forward * Time.deltaTime * Speed);
            transform.position += Vector3.back * Time.deltaTime * Speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            //transform.Translate(Vector3.forward * Time.deltaTime * Speed);
            transform.position += Vector3.left * Time.deltaTime * Speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            //transform.Translate(Vector3.forward * Time.deltaTime * Speed);
            transform.position += Vector3.right * Time.deltaTime * Speed;
        }
        //cameraController.UpdateCameraPostion();
        b_mouseMove = false;
    }

    void OnKeyRealesed()
    {
        if(Input.GetKeyUp(KeyCode.W))
        {

        }
    }

    void OnMouseClicked(Define.MouseEvent _event)
    {
        //if (_event != Define.MouseEvent.Click)
        //    return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1.0f);

        LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
        //int mask = (1 << 8) | (1 << 9);//비트플래그 방식으로 8번레이어만 사용하려면 1을8번까지 쉬프트 연산후에 레이케스트할때 마스트변수에 넣어주면된다. 그리고 | 즉 or 연산을 하게되면 8번과 9번이 동시에 켜지게되면서 두개의 레이어를 동시에사용하게 할 수 있다.

        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            desPos = hit.point;
            b_mouseMove = true;
            Debug.Log("" + hit.collider.gameObject.name);
        }
    }
    private void OnDestroy()
    {
        Manager.Input.KeyAction -= OnKeyBoard;
    }
}
