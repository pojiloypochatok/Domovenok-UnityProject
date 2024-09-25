using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraControl : MonoBehaviour
{
    public float wheel_speed;
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

        if (scroll > 0 && transform.position.x < rightlimit.x) // Движение камеры вправо
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * scroll * (wheel_speed * 100), Time.deltaTime);
        }
        else if (scroll < 0 && transform.position.x > leftlimit.x) // Движение камеры влево
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * scroll * (wheel_speed * 100), Time.deltaTime);
        }
    }
}
