using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterController : MonoBehaviour
{

    [SerializeField]
    float MoveDelay = 1f;
    [SerializeField]
    float BreakDelay = 1f;
    Vector2Int currentPosition;
    [SerializeField]
    Animator animator;

    Coroutine MoveCoroutine;

    [SerializeField] Vector2Int InitialPos;
    [SerializeField] float distance = 3f; // 飛ばす&表示するRayの長さ
    [SerializeField] AudioClip naguruVoice;

    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        currentPosition = InitialPos;
        Vector3 init = StageManager.Instance.GetCenterPos3D(InitialPos);
        init.y = 0;
        transform.position = init;
        StageManager.Instance.Regist(InitialPos, gameObject);
    }

    public void BePlayable()
    {
        MoveCoroutine = StartCoroutine(ProcessMoveInput());
    }

    public void BeUnPlayable()
    {
        StopCoroutine(MoveCoroutine);
    }

    IEnumerator ProcessMoveInput()
    {
        while (true)
        {
            Vector2Int input = new Vector2Int((int)Mathf.Ceil(Input.GetAxis("1PHorizontal")), (int)Mathf.Ceil(Input.GetAxis("1PVertical")));
            if (Input.GetButton("Break"))
            {
                if (input.magnitude == 0)
                    input = new Vector2Int((int)transform.forward.x, (int)transform.forward.z);
                if (StageManager.Instance.GridData.TryGetValue(currentPosition + input, out GameObject obj))
                {
                    if (BreakBlock(obj))
                        yield return new WaitForSeconds(BreakDelay);
                    else
                        yield return null;
                }
                else
                    yield return null;
            }

            else
            {
if (input.magnitude > 0)
            {
                if (Move(input))
                    yield return new WaitForSeconds(MoveDelay);
                else
                    yield return null;
            }
            else
                yield return null;
            }
        
        }

    }

    bool Move(Vector2Int direction)
    {
        int ax = Mathf.Abs(direction.x), ay = Mathf.Abs(direction.y);
        if (ax + ay != Mathf.Max(ax, ay))
        {
            Vector2 fwd = new Vector2(transform.forward.x, transform.forward.z);
            Vector2 xd = new Vector2(direction.x, 0);
            Vector2 yd = new Vector2(0, direction.y);
            direction = Vector2.Dot(xd, fwd) > Vector2.Dot(yd, fwd) ? new Vector2Int(direction.x, 0) : new Vector2Int(0, direction.y);
        }
        Vector3 diff = new Vector3(direction.x, 0, direction.y);
        transform.rotation = Quaternion.LookRotation(diff);

        if (!StageManager.Instance.GridData.ContainsKey(currentPosition + direction))
        {
            currentPosition += direction;
            StartCoroutine(MovedeltaPosition(StageManager.Instance.GetCenterPos3D(currentPosition), MoveDelay));
            StageManager.Instance.Escaped();
            return true;
        }

        if (StageManager.Instance.GridData[currentPosition + direction] != null)
            return false;

        StageManager.Instance.Regist(currentPosition, null);
        currentPosition += direction;
        StageManager.Instance.Regist(currentPosition, gameObject);

        StartCoroutine(MovedeltaPosition(StageManager.Instance.GetCenterPos3D(currentPosition), MoveDelay));
        return true;
    }

    IEnumerator MovedeltaPosition(Vector3 destination, float time)
    {
        animator.SetBool("IsRun", true);
        float timestamp = 0;
        while (timestamp < time)
        {
            timestamp += Time.deltaTime;
            float percent = timestamp / time;
            transform.position = Vector3.Lerp(transform.position, destination, percent);
            yield return null;
        }
        transform.position = destination;
        animator.SetBool("IsRun", false);
    }

    bool BreakBlock(GameObject obj)
    {
        if (obj == null)
            return false;
        obj.GetComponent<BreakableObjectController>().GetAttacked(StageManager.Instance.GetCenterPos3D(currentPosition));
        SoundManager.Instance.PlayBrakeBlockSE();
        return true;
    }
}
