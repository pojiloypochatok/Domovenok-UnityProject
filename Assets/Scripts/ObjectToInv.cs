using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ObjectToInv : MonoBehaviour
{
    public Vector3 InvPos;
<<<<<<< HEAD
=======
    public GameObject molot_right;
    public GameObject molot_left;
>>>>>>> 365a608 (Work of playercontroll and itemtoinv)
    public float movespeed = 10;
    public GameObject player;
    bool isclickon = false;
    bool isdestory = true;
    void Start()
    {

    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x - player.transform.position.x) <= 40 & isdestory & isclickon)
        {
            if (transform.position.x > player.transform.position.x)
            {
                Instantiate(molot_right, new Vector3(transform.position.x - 10, transform.position.y + 10, transform.position.z), Quaternion.identity);
                Animator anim = molot_right.GetComponent<Animator>();
                anim.Play("GarbageBroke");
                Invoke("MoveToInv", 0.4f);
                Debug.Log("Клик прошел");
                isdestory = false;
            }
            else
            {
                Instantiate(molot_left, new Vector3(transform.position.x + 10, transform.position.y + 10, transform.position.z), Quaternion.identity);
                Animator anim = molot_left.GetComponent<Animator>();
                anim.Play("GarbageBroke_left");
                Invoke("MoveToInv", 0.4f);
                Debug.Log("Клик прошел");
                isdestory = false;
            }
        }
    }

    public void MoveToInv()
    {
<<<<<<< HEAD
=======
        Destroy(GameObject.Find("Molot_right(Clone)"));
        Destroy(GameObject.Find("Molot_left(Clone)"));
>>>>>>> 365a608 (Work of playercontroll and itemtoinv)
        transform.position = InvPos;
    }

<<<<<<< HEAD
    private void OnMouseDown()
    {
        string objectName = gameObject.name;
        MoveToInv();
        Debug.Log("Clicked object name: " + objectName);
=======
    public void OnMouseDown()
    { 
        isclickon = true;
>>>>>>> 365a608 (Work of playercontroll and itemtoinv)
    }
}
