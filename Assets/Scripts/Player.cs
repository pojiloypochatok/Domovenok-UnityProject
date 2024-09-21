using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;
using Spine.Unity;
using Spine.Unity.Examples;
using System;



public class PlayerController : MonoBehaviour
{
    public float HorizontalSpeed;
    Rigidbody2D rb;
    Vector3 diference;
    public GameObject player;
    
    [SpineAnimation]
    public string idle;
    [SpineAnimation]
    public string run_right;
    [SpineAnimation]
    public string run_left;


    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    string currentname;


    // Start is called before the first frame update
    void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.Find("Domovoy");
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            
        }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetAnimation(int scene, string name, bool loop)
    {
        if (name == currentname)
        {
            return;
        }
        spineAnimationState.SetAnimation(scene, name, loop);
        currentname = name;

    }

    public void Move()
    {
        Vector3 targetposition;
        if (Input.GetMouseButtonDown(0))
        {
            diference = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }
        if (transform.position.x + 1 < diference.x)
        {
            transform.Translate(HorizontalSpeed, 0, 0);
            SetAnimation(0, run_right, true);
        }
        else if (transform.position.x - 1 >= diference.x)
        {
            transform.Translate(-HorizontalSpeed, 0, 0);
            SetAnimation(0, run_left, true);
        }
        else
        {
            SetAnimation(0, idle, true);
        }

    }

}
