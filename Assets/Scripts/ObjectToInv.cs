using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ObjectToInv : MonoBehaviour
{
    public Vector3 InvPos;
    public float movespeed = 10;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToInv()
    {
        transform.position = InvPos;
    }

    private void OnMouseDown()
    {
        MoveToInv();
        Debug.Log("Клик прошел");
    }
}
