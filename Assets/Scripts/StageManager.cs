using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = FindObjectOfType<StageManager>();
            return m_Instance;
        }
    }
    static StageManager m_Instance;

    public float GridSize = 1f;
    public Dictionary<Vector2Int, GameObject> GridData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 GetCenterPos2D(Vector2Int pos)
    {
        return new Vector2((float)pos.x + GridSize / 2, (float)pos.y + GridSize / 2);
    }

    public Vector3 GetCenterPos3D(Vector2Int pos)
    {
        return new Vector3((float)pos.x + GridSize / 2, GridSize / 2, (float)pos.y + GridSize / 2);
    }
}
