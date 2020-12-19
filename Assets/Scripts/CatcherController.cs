using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherController : MonoBehaviour
{

    public Vector2Int SelectingPos = Vector2Int.zero;

    [SerializeField]
    float PlaceDelay=1;
    [SerializeField]
    float MoveDelay = 1;

    Coroutine MoveInputCoroutine;
    Coroutine PlaceCoroutine;

    [SerializeField]
    GameObject SelectionObj;

    [SerializeField]
    GameObject BlockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void BePlayable()
    {
        MoveInputCoroutine = StartCoroutine(ProcessMoveInput());
        PlaceCoroutine = StartCoroutine(ProcessPlacement());
    }

    public void BeUnPlayable()
    {
        StopCoroutine(MoveInputCoroutine);
        StopCoroutine(PlaceCoroutine);
    }

    IEnumerator ProcessPlacement()
    {
        while (true)
        {
            if (Input.GetButton("Placement"))
            {
                if (PlaceCube())
                    yield return new WaitForSeconds(PlaceDelay);
                else
                    yield return null;
            }
            else
                yield return null;
        }
    }

    IEnumerator ProcessMoveInput()
    {
        while (true)
        {
            Vector2Int input = new Vector2Int((int)Mathf.Ceil(Input.GetAxis("2PHorizontal")), (int)Mathf.Ceil(Input.GetAxis("2PVertical")));
            if (input.magnitude > 0&&StageManager.Instance.GridData.ContainsKey(SelectingPos+input))
            {
                SelectingPos += input;
                input = Vector2Int.zero;
                UpdateSelection();
                yield return new WaitForSeconds(MoveDelay);
            }
            else
                yield return null;
        }

    }

    bool PlaceCube()
    {
        if (StageManager.Instance.GridData[SelectingPos] != null)
            return false;

        GameObject obj = Instantiate(BlockPrefab, StageManager.Instance.GetCenterPos3D(SelectingPos), Quaternion.identity);
        StageManager.Instance.Regist(SelectingPos, obj);
        return true;
    }

    void UpdateSelection()
    {
        SelectionObj.transform.position = StageManager.Instance.GetCenterPos3D(SelectingPos);
    }


}
