using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ObjectToInv : MonoBehaviour
{
    public Vector3 InvPos;
    public GameObject molot;
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
        Destroy(GameObject.Find("Molot(Clone)"));
        transform.position = InvPos;

    }

    private void OnMouseDown()
    {
        Instantiate(molot, new Vector3(transform.position.x - 10, transform.position.y + 10, transform.position.z), Quaternion.identity);
        Animator anim = molot.GetComponent<Animator>();
        anim.Play("Garbage Broke"); 
        Invoke("MoveToInv", 0.4f);

        Debug.Log("Клик прошел");
    }
}
