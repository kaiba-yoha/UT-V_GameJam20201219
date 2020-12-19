using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour
{

    [SerializeField] float speed = 1.0f;
    Vector3 currentPosition;
    Animator animator;
    Vector3 direction;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        currentPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("1PHorizontal");
        float z = Input.GetAxis("1PVertical");

        if(x != 0) {
            direction = new Vector3(x,0,0);
            //animator.CrossFadeInFixedTime("Run",0);
            animator.SetBool("isRun",true);
        }
        else if(z != 0) {
            direction = new Vector3(0,0,z);
            //animator.CrossFadeInFixedTime("Run",0);
            animator.SetBool("isRun",true);
        } else if(x == 0 && z == 0) {
            //animator.CrossFadeInFixedTime("Idle",0);
            animator.SetBool("isRun",false);
        }


        float step = speed * Time.deltaTime;
        //Vector3 direction = new Vector3(x,0,z);
        transform.position += direction * step;

        Vector3 diff = transform.position - currentPosition;
        transform.rotation = Quaternion.LookRotation(diff);
        currentPosition = transform.position;
    }
}
