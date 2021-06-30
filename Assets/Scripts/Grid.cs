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

    void Start()
    {
        _hexagons = new Hexagons[width, height];
        CreateGridWithHexagons();     
    }

    private void CreateGridWithHexagons()
    {
        int i = 0;
        _hexagonList = new GameObject[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float yPos = y * yOffset;
                if (x % 2 == 1) yPos += yOffset / 2f;
                GameObject gridGameObject = (GameObject)Instantiate(_gridPointPrefab, new Vector3(x * xOffset, yPos, 0), Quaternion.identity);
                gridGameObject.transform.parent = this.transform;
                GameObject hexGameObject = (GameObject)Instantiate(_hexPrefab, new Vector3(x * xOffset, yPos, 0), Quaternion.identity);
                hexGameObject.transform.parent = gridGameObject.transform;
                hexGameObject.transform.localPosition = new Vector3(0, 0, 0);
                hexGameObject.GetComponent<SpriteRenderer>().color = Colors[Random.Range(0, Colors.Length)];
                gridGameObject.name = x + "_" + y;
                _hexagons[x, y] = hexGameObject.GetComponent<Hexagons>();
                _hexagons[x, y].X = x;
                _hexagons[x, y].Y = y;
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
                            _hexagons[x, y].AddNeighbors(_hexagons[x + _browseX[i], y + _browseYUp[i]]);
                    }
                    else
                    {
                        if (IsRowColExist(x + _browseX[i], y + _browseYDown[i]))
                            _hexagons[x, y].AddNeighbors(_hexagons[x + _browseX[i], y + _browseYDown[i]]);
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
    }

    private bool IsRowColExist(int x, int y)
    {
        if (x < 0 || x >= width) return false;
        else if (y < 0 || y >= height) return false;
        else return true;        
    }
}