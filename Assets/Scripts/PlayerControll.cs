using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;
using Spine.Unity;
using Spine.Unity.Examples;
using System.Security.Cryptography;
using UnityEditor.Rendering.LookDev;



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
    public LayerMask layerMask;


    // Start is called before the first frame update
    void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
        }


// Update is called once per frame
void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetMouseButtonDown(0))
        {
            diference = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (CheckForCollision(diference))
            {
                if (diference.x < 0)
                {
                    diference.x += 40;
                }
                else
                {
                    diference.x -= 40;
                }
                Debug.Log("Colider");
            }
        }
        Invoke("PlayerToCam", 5f);
        Move(diference);
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

    public void Move(Vector3 diference)
    {
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

    public void PlayerToCam()
    {
        float screenWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        float leftBorder = Camera.main.transform.position.x - Camera.main.orthographicSize * Screen.width / Screen.height;
        float rightBorder = Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height;
        float centerX = Camera.main.transform.position.x;
        if (transform.position.x < leftBorder)
        {
            diference.x = centerX;
        }

        else if (transform.position.x > rightBorder)
        {
            diference.x = centerX;
        }        
    }

    bool CheckForCollision(Vector3 spawnlocation)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnlocation, new Vector2(15, 15), 0f, layerMask);
        if (colliders.Length > 0)
        {
            return true;
        }
        // Åñëè êîëëèçèÿ íå îáíàðóæåíà, âîçâðàùàåì false
        return false;
    }

}