using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Dead,
        Moving,
        Idle,
    }
    [SerializeField]//객체지향적인 관점에서 프라이빗 변수로 선언을 하더라도 에디터상에서 변수를 수정 하고 싶으면 SerializeField어트리뷰트를 달아주면 된다. 
    private float Speed = 10.0f;

    public CameraController cameraController;
    public PlayerState playerState;

    private Vector3 desPos;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {  
        Manager.Input.MouseAction -= OnMouseClicked;
        Manager.Input.MouseAction += OnMouseClicked;
        
        playerState = PlayerState.Idle;
    }
    
    void Update()
    {
        StateSwitch();

    }

    private void StateSwitch()
    {
        switch(playerState)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Dead:
                Die();
                break;
        }
    }

    private void UpdateIdle()
    {
        anim.SetFloat("speed", 0);
    }

    private void UpdateMoving()
    {
        Vector3 dir = desPos - transform.position;
        if (dir.magnitude < 0.001f)
        {
            playerState = PlayerState.Idle;
        }
        else
        {
            float movedist = Mathf.Clamp(Speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * movedist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
        anim.SetFloat("speed", Speed);
        
    }
    private void Die()
    {

    }
    void OnMouseClicked(Define.MouseEvent _event)
    {
        if (playerState == PlayerState.Dead)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1.0f);

        LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
        //int mask = (1 << 8) | (1 << 9);//비트플래그 방식으로 8번레이어만 사용하려면 1을8번까지 쉬프트 연산후에 레이케스트할때 마스트변수에 넣어주면된다. 그리고 | 즉 or 연산을 하게되면 8번과 9번이 동시에 켜지게되면서 두개의 레이어를 동시에사용하게 할 수 있다.

        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            desPos = hit.point;
            playerState = PlayerState.Moving;
            Debug.Log("" + hit.collider.gameObject.name);
        }
    }
    
    private void OnRunEvent(string str)
    {
        Debug.Log(str);
    }
}
