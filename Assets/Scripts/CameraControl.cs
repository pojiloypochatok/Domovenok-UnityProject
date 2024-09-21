using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraControl : MonoBehaviour
{
    public float wheel_speed = 100f;
    private Camera cam;


    [SerializeField]
    Vector3 leftlimit;
    [SerializeField]
    Vector3 rightlimit;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0 & transform.position.x <= rightlimit.x)                  // �������� ������ ������ 
        {
            transform.position += transform.right * scroll * wheel_speed;
            
        }

        else if (scroll < 0 & transform.position.x >= leftlimit.x)            // �������� ������ �����
        {
            transform.position += transform.right * scroll * wheel_speed;
        }
    }
}
