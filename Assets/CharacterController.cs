using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour
{

    [SerializeField] float speed = 1.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("1PHorizontal");
        float z = Input.GetAxis("1PVertical");
        float step = speed * Time.deltaTime;
        Vector3 direction = new Vector3(x,0,z);
        transform.position += direction * step;

    }
}
