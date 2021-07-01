using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : GridController
{
    public Color[] Colors = new Color[] { Color.blue, Color.red, Color.yellow, Color.green, Color.magenta };

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    [SerializeField]
    private GameObject _hexPrefab;
    [SerializeField]
    private GameObject _gridPointPrefab;

    private float xOffset = 0.77f;
    private float yOffset = 0.89f;
    private int[] _browseX = new int[6] { 0, 0, -1, -1, 1, 1 };
    private int[] _browseYUp = new int[6] { 1, -1, 0, 1, 0, 1 };
    private int[] _browseYDown = new int[6] { 1, -1, 0, -1, 0, -1 };
    public Hexagons[,] _hexagons;
    public GridPoints[,] _gridPoints;
    private Vector3 _mousePosition;
    private GameObject[] _hexagonList;
    private HexagonTio _hexTrio;
    private SelectionController _sc;

    void Start()
    {
        _hexTrio = this.gameObject.GetComponent<HexagonTio>();
        _hexagonList = new GameObject[width * height];
        _gridPoints = new GridPoints[width, height];
        _sc = GameObject.Find("SelectionController").GetComponent<SelectionController>();
        CreateGridWithHexagons();     
    }

    private void CreateGridWithHexagons()
    {
        int i = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float yPos = y * yOffset;
                if (x % 2 == 1) yPos += yOffset / 2f;
                GameObject gridGameObject = (GameObject)Instantiate(_gridPointPrefab, new Vector3(x * xOffset, yPos, 0), Quaternion.identity);
                gridGameObject.transform.parent = this.transform;
                _gridPoints[x, y] = gridGameObject.GetComponent<GridPoints>();
                GameObject hexGameObject = (GameObject)Instantiate(_hexPrefab, new Vector3(x * xOffset, yPos, 0), Quaternion.identity);
                hexGameObject.transform.parent = gridGameObject.transform;
                hexGameObject.transform.localPosition = new Vector3(0, 0, 0);
                Hexagons hx = hexGameObject.GetComponent<Hexagons>();
                hx.MyGridPoint = _gridPoints[x, y];
                hx.HexColor = Colors[Random.Range(0, Colors.Length)];
                _gridPoints[x, y].MyHex = hx;
                _gridPoints[x, y].X = x;
                _gridPoints[x, y].Y = y;
                gridGameObject.name = x + "_" + y;
                hx.X = x;
                hx.Y = y;
                _hexagonList[i++] = hexGameObject;
            }
        }
        SetHexagonsNeighbours();
    }

    private void SetHexagonsNeighbours()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (x % 2 == 1)
                    {
                        if (IsRowColExist(x + _browseX[i], y + _browseYUp[i]))
                            _gridPoints[x, y].AddNeighbors(_gridPoints[x + _browseX[i], y + _browseYUp[i]]);
                    }
                    else
                    {
                        if (IsRowColExist(x + _browseX[i], y + _browseYDown[i]))
                            _gridPoints[x, y].AddNeighbors(_gridPoints[x + _browseX[i], y + _browseYDown[i]]);
                    }

                }
            }
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 12f));
            if (_mousePosition.y < 8)
                _selection.FindNearestTrio(_mousePosition, _hexagonList, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckAllTrio();
            _hexTrio.ExplodeHexagons();
            FillHexagons();
        }
    }

    public void FillHexagons()
    {
        if(_sc.IsSelectedTrio) _sc.ClearSelectedHighLight();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_gridPoints[x, y].MyHex == null) _gridPoints[x, y].MyHex = _gridPoints[x, y].CallNewHexagon(y + 1);
            }
        }
    }

    public GridPoints GetGridOnIndex(int x, int y)
    {
        if (y >= height) return null;
        return _gridPoints[x, y];
    }

    private void CheckAllTrio()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _hexTrio.DetectHexagonTrio(_gridPoints[x, y], _gridPoints[x, y].SameColorNeighbors);
            }
        }
    }

    public void CheckSameColorNeighbours()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _gridPoints[x, y].CheckSameColorNeighbors();
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