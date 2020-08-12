using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    //1)나한테 RigidBody가 있어야한다.(IsKinematic : Off)
    //2)나한테 Collider가 있어야 한다.(IsTrigger : Off)
    //3)상대한테 Collider가 있어야한다.(IsTrigger : Off)
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision!");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTirigger !");
    }

    private void Update()
    {
        //Vector3 look = transform.TransformDirection(Vector3.forward);//현재 게임오브젝트가 보는 방향을 월드에서의 방향으로 변환해준다.

        //Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);

        //RaycastHit[] hits;
        //hits = Physics.RaycastAll(transform.position + new Vector3(0, 1, 0), look * 10);

        //foreach(RaycastHit hit in hits)
        //{
        //    Debug.Log("Raycast " + hit.collider.gameObject.name);
        //}
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(Camera.main.transform.position,ray.direction *100f , Color.red, 1.0f);

            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Player");
            //int mask = (1 << 8) | (1 << 9);//비트플래그 방식으로 8번레이어만 사용하려면 1을8번까지 쉬프트 연산후에 레이케스트할때 마스트변수에 넣어주면된다. 그리고 | 즉 or 연산을 하게되면 8번과 9번이 동시에 켜지게되면서 두개의 레이어를 동시에사용하게 할 수 있다.
            
            if (Physics.Raycast(ray, out hit, 100f, mask))
            {
                Debug.Log("" + hit.collider.gameObject.name);
            }
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //    Vector3 dir = mousePos - Camera.main.transform.position;
        //    dir = dir.normalized;
        //    //투영절두체의 nearplane의 좌표를 얻어와서 카메라에서 그좌표까지의 방향벡터를 구한뒤 레이를 쏘면 마우스가 찍은 위치로 마우스 레이를 쏘는 것이 된다.
        //    Debug.DrawRay(Camera.main.transform.position, dir * 100f, Color.red, 1.0f);

        //    RaycastHit hit;
        //    if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100f))
        //    {
        //        Debug.Log("" + hit.collider.gameObject.name);
        //    }
        //}
    }
}
