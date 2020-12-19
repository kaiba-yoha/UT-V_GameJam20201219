﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherController : MonoBehaviour
{

    public Vector2Int SelectingPos = Vector2Int.zero;

    [SerializeField]
    float InputDelay = 1;

    Coroutine InputCoroutine;

    [SerializeField]
    GameObject SelectionObj;

    // Start is called before the first frame update
    void Start()
    {
        InputCoroutine = StartCoroutine(ProcessInput());
    }

    IEnumerator ProcessInput()
    {
        while (true)
        {
            Vector2Int input = new Vector2Int(Mathf.RoundToInt(Input.GetAxis("2PHorizontal")), Mathf.RoundToInt(Input.GetAxis("2PVertical")));
            if (input.magnitude > 0)
            {
                SelectingPos += input;
                input = Vector2Int.zero;
                UpdateSelection();
                yield return new WaitForSeconds(InputDelay);
            }
            else
                yield return null;
        }

    }

    void UpdateSelection()
    {
        SelectionObj.transform.position = StageManager.Instance.GetCenterPos3D(SelectingPos);
    }


}
