using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Define.CameraMode _mode = Define.CameraMode.QuaterView;
    [SerializeField]
    private Vector3 _delta;
    [SerializeField]
    private GameObject player;

    public Transform Offset;
    void Start()
    {
        
    }
    private void LateUpdate()
    {
        if(_mode == Define.CameraMode.QuaterView)
        {
            RaycastHit hit;
            if(Physics.Raycast(Offset.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - Offset.position).magnitude * 0.8f;
                transform.position = Offset.position + _delta.normalized * dist;

            }
            else
            {
                UpdateCameraPostion();
            }

        }
    }

    public void UpdateCameraPostion()
    {
        transform.position = player.transform.position + _delta;
    }

    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuaterView;
        _delta = delta;
    }
}
