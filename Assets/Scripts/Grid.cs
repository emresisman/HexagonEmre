using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    CanPlay,
    Waiting,
    FlowingHex
};


public class Grid : MonoBehaviour
{
    private bool ifFirst = true;
    public static GameState gameState = GameState.Waiting;
    public Color[] Colors = new Color[] { Color.blue, Color.red, Color.yellow, Color.green, Color.magenta };

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private GameObject _hexPrefab;

    private float xOffset = 0.77f;
    private float yOffset = 0.89f;
    private int[] _browseX = new int[6] { 0, 0, -1, -1, 1, 1 };
    private int[] _browseYUp = new int[6] { 1, -1, 0, 1, 0, 1 };
    private int[] _browseYDown = new int[6] { 1, -1, 0, -1, 0, -1 };
    [SerializeField]
    public Hexagons[,] _hexagons;
    private Vector3 _mousePosition;
    [SerializeField]
    private GameObject[,] _hexagonList, _gridPoints;
    private HexagonTio _hexTrio;
    private SelectionController _sc;

#region Singleton
    private static Grid _instance;

    public static Grid Instance { get => _instance; }

    private void Awake()
    {
        _instance=this;
    }
#endregion    

    void Start()
    {
        _hexTrio = this.gameObject.GetComponent<HexagonTio>();
        _hexagonList = new GameObject[width, height];
        _hexagons = new Hexagons[width, height];
        _gridPoints = new GameObject[width, height];
        _sc = GameObject.Find("SelectionController").GetComponent<SelectionController>();
        CreateGridWithHexagons();     
    }

    private void CreateGridWithHexagons()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float yPos = y * yOffset;
                if (x % 2 == 1) yPos += yOffset / 2f;
                GameObject hexGameObject = (GameObject)Instantiate(_hexPrefab, new Vector3(x * xOffset, yPos, 0), Quaternion.identity);
                GameObject gridPoints = new GameObject(x + "_" + y);
                gridPoints.transform.position = hexGameObject.transform.position;
                _gridPoints[x, y] = gridPoints;
                Hexagons hx = hexGameObject.GetComponent<Hexagons>();
                _hexagonList[x,y] = hexGameObject;
                _hexagons[x,y]=hx;
                hx.HexColor = Colors[Random.Range(0, Colors.Length)];
                hexGameObject.name = x + "_" + y;
                hx.SetPosition(x,y);
            }
        }
        SetHexagonsNeighbours();
    }

    public void SetHexagonsNeighbours()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _hexagons[x,y].ClearNeighbours();
                _hexagons[x,y].ClearSameColorNeighbors();
                for (int i = 0; i < 6; i++)
                {
                    if (x % 2 == 1)
                    {
                        if (IsRowColExist(x + _browseX[i], y + _browseYUp[i]))
                            _hexagons[x, y].AddNeighbour(_hexagons[x + _browseX[i], y + _browseYUp[i]]);
                    }
                    else
                    {
                        if (IsRowColExist(x + _browseX[i], y + _browseYDown[i]))
                            _hexagons[x, y].AddNeighbour(_hexagons[x + _browseX[i], y + _browseYDown[i]]);
                    }

                }
            }
        }
        gameState=GameState.Waiting;
    }

    public Transform GetIndexPosition(int x, int y)
    {
        return _gridPoints[x, y].transform;
    }

    public void ChangeHexIndex(GameObject hexOject, int xNew, int yNew)
    {
        _hexagonList[xNew,yNew] = hexOject;
        _hexagons[xNew,yNew] = hexOject.GetComponent<Hexagons>();
    }

    void Update()
    {
        if (gameState == GameState.CanPlay)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 12f));
                if (_mousePosition.y < 8)
                    _sc.FindNearestTrio(_mousePosition, _hexagonList, 0);
            }
        }
        else if(gameState==GameState.Waiting)
        {
            if (ifFirst)
            {
                CheckAllTrio(0);
                if (_hexTrio.isTrioExist)
                {
                    _hexTrio.isTrioExist = false;
                    _hexTrio.ResetColor();
                }
                else
                {
                    ifFirst = false;
                    SetHexagonsNeighbours();
                }
            } 
            else
            {
                CheckAllTrio();
                if (_hexTrio.isTrioExist)
                {
                    _hexTrio.isTrioExist = false;
                    _hexTrio.ExplodeHexagons();
                    gameState = GameState.FlowingHex;
                }
                else
                {
                    gameState = GameState.CanPlay;
                }
            } 
        }
        else if(gameState==GameState.FlowingHex)
        {
            FillHexagons();
        }
    }

    public void FillHexagons()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_hexagons[x, y] == null)
                {
                    Hexagons _hex = CallNewHexagon(x, y + 1);
                    if (_hex != null)
                    {
                        _hex.SetPosition(x, y);
                        _hex.Moving=true;
                    }
                }
            }
        }
        SetHexagonsNeighbours();
    }

    public Hexagons CallNewHexagon(int x, int y)
    {
        Hexagons _hex;
        if (GetHexOnIndex(x, y))
        {
            if (_hexagons[x, y] != null)
            {
                _hex = _hexagons[x, y];
                _hexagons[x, y] = null;
                return _hex;
            }
            else
            {
                return CallNewHexagon(x, y + 1);
            }
        }
        else
        {
            return HexagonPool.Instance.GetHexagonFromPool();
        }

    }

    public void IAmExploded(int x, int y)
    {
        _hexagons[x, y] = null;
        _hexagonList[x, y] = null;
    }

    public bool GetHexOnIndex(int x, int y)
    {
        if (y >= height) return false;
        return true;
    }

    private void CheckAllTrio()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _hexTrio.DetectHexagonTrio(_hexagons[x, y], _hexagons[x, y].SameColorNeighbors);
            }
        }
    }

    private void CheckAllTrio(int i)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _hexTrio.DetectHexagonTrio(_hexagons[x, y], _hexagons[x, y].SameColorNeighbors);
            }
        }
    }

    private bool IsRowColExist(int x, int y)
    {
        if (x < 0 || x >= width) return false;
        else if (y < 0 || y >= height) return false;
        else return true;        
    }
}