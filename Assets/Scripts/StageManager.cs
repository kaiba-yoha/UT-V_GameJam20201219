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

    [SerializeField]
    float GridSize = 1f;
    [SerializeField]
    Vector2Int GridNum;

    public Dictionary<Vector2Int, GameObject> GridData = new Dictionary<Vector2Int, GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        InitializeGrid();
    }

    private void Start()
    {
        StartGame();
    }

    void InitializeGrid()
    {
        for (int x = 0; x < GridNum.x; x++)
        {
            for (int y = 0; y < GridNum.y; y++)
            {
                GridData.Add(new Vector2Int(x, y), null);
            }
        }
    }

    public Vector2 GetCenterPos2D(Vector2Int pos)
    {
        return new Vector2((float)pos.x + GridSize / 2, (float)pos.y + GridSize / 2);
    }

    public Vector3 GetCenterPos3D(Vector2Int pos)
    {
        return new Vector3((float)pos.x + GridSize / 2, GridSize / 2, (float)pos.y + GridSize / 2);
    }

    public void Regist(Vector2Int pos, GameObject obj)
    {
        GridData[pos]=obj;
    }

    public void StartGame()
    {
        FindObjectOfType<CharacterController>().BePlayable();
        FindObjectOfType<CatcherController>().BePlayable();
    }

    public void EndGame()
    {
        FindObjectOfType<CharacterController>().BeUnPlayable();
        FindObjectOfType<CatcherController>().BeUnPlayable();
    }
}
