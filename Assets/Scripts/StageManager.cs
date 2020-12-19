using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    [SerializeField]
    int RandomBlockNum=0;
    [SerializeField]
    GameObject BlockPrefab;
    [SerializeField]
    int edgethickness = 1;
    [SerializeField]
    float EdgeSpawnRate=0.8f;

    [SerializeField]
    float TimeLimit=30f;
    [SerializeField]
    Text timetext;
    [SerializeField]
    Text ResultText;

    public Dictionary<Vector2Int, GameObject> GridData = new Dictionary<Vector2Int, GameObject>();

    public event Action OnGameEnded;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeGrid();
    }

    private void Start()
    {
        //StartGame();
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

    void GenerateEdgeBlock()
    {
        for (int x = 0; x < GridNum.x; x++)
        {
            for (int y = 0; y < GridNum.y; y++)
            {
                if(Random.value<=EdgeSpawnRate)
                if(x<edgethickness||Mathf.Abs(GridNum.x-x-1)<edgethickness|| y < edgethickness || Mathf.Abs(GridNum.y - y-1) < edgethickness)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    GameObject obj = Instantiate(BlockPrefab, GetCenterPos3D(pos), Quaternion.identity);
                    Regist(pos, obj);
                }
            }
        }
    }

    void GenerateRandomBlock()
    {
        for(int i = 0; i < RandomBlockNum; i++)
        {
            Vector2Int pos = new Vector2Int(Random.Range(0, GridNum.x), Random.Range(0, GridNum.y));
            if (GridData[pos] != null)
                return;
            GameObject obj = Instantiate(BlockPrefab, GetCenterPos3D(pos), Quaternion.identity);
            Regist(pos, obj);
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
        GridData.Values.ToList().ForEach((obj) =>
        {
            if (obj != null&&obj.tag!="Player")
                Destroy(obj);
        });
        CharacterController cc = FindObjectOfType<CharacterController>();
        cc.BePlayable();
        cc.Initialize();
        CatcherController catcher = FindObjectOfType<CatcherController>();
        catcher.BePlayable();
        catcher.Initialize();
        SoundManager.Instance.PlayBGM();
        SoundManager.Instance.PlaySE(SEID.PlayStart);
        GenerateEdgeBlock();
        GenerateRandomBlock();
        StartCoroutine(CountTime());
    }

    IEnumerator CountTime()
    {
        float time = TimeLimit;
        while (time > 0)
        {
            timetext.text="残り"+(int)Mathf.Ceil(time)+"秒";
            time -= Time.deltaTime;
            yield return null;
        }
        timetext.text = "";
        EndGame(2);
    }

    public void Escaped()
    {
        StopAllCoroutines();
        timetext.text = "";
        EndGame(1);
    }

    public void EndGame(int winner)
    {
        FindObjectOfType<CharacterController>().BeUnPlayable();
        FindObjectOfType<CatcherController>().BeUnPlayable();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySE(SEID.PlayFinish);
        ResultText.gameObject.SetActive(true);
        ResultText.text = winner + "P Win!";
        OnGameEnded?.Invoke();
    }
}
