using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoints : MonoBehaviour
{
    private Grid _grid;
    private int _x, _y;
    [SerializeField]
    private Color _myHexColor;
    private Hexagons _myHex;
    [SerializeField]
    private List<GridPoints> _neighbors = new List<GridPoints>();
    [SerializeField]
    private List<GridPoints> _sameColorNeighbors = new List<GridPoints>();
    private HexagonPool _hexPool;
    public Color MyHexColor { get => _myHexColor; set => _myHexColor = value; }
    public List<GridPoints> SameColorNeighbors { get => _sameColorNeighbors; set => _sameColorNeighbors = value; }
    public Hexagons MyHex
    {
        get
        {
            return _myHex;
        }
        set
        {
            _myHex = value;
            
            if (value != null)
            {
                _myHex.gameObject.SetActive(true);
                _myHexColor = _myHex.HexColor;
                _myHex.transform.parent = this.transform;
                _myHex.transform.localPosition = Vector3.zero;
            }
        }
    }

    public Hexagons MyHexWithCoroutine
    {
        set
        {
            _myHex = value;

            if (value != null)
            {
                _myHex.gameObject.SetActive(true);
                _myHexColor = _myHex.HexColor;
                _myHex.transform.parent = this.transform;
                //_myHex.transform.localPosition = Vector3.zero;
                MoveToGridPoint(_myHex);
                //StartCoroutine(SmoothLerp(_myHex.transform));
            }          
        }
    }

    public int X { get => _x; set => _x = value; }
    public int Y { get => _y; set => _y = value; }

    public void AddNeighbors(GridPoints _neighbor)
    {
        _neighbors.Add(_neighbor);
        CheckSameColorNeighbors();
    }

    public void CheckSameColorNeighbors()
    {
        SameColorNeighbors.Clear();
        foreach(GridPoints _gp in _neighbors)
        {
            if (_gp.MyHexColor == _myHexColor) SameColorNeighbors.Add(_gp);
        }
    }

    private void Awake()
    {
        _grid = GameObject.Find("Grid").GetComponent<Grid>();
        _hexPool = GameObject.Find("HexagonPool").GetComponent<HexagonPool>();
    }

    public Hexagons CallNewHexagon(int y)
    {
        Hexagons _hex;
        GridPoints _myUpperGrid;
        if(_grid.GetGridOnIndex(_x, y) != null)
        {
            _myUpperGrid = _grid.GetGridOnIndex(_x, y);
            if (_myUpperGrid.MyHex != null)
            {
                _hex = _myUpperGrid.MyHex;
                _myUpperGrid.MyHex = null;
                return _hex;
            }
            else
            {
                return CallNewHexagon(y + 1);
            }
        }
        else
        {
            return _hexPool.GetHexagonFromPool(this);
        }

    }

    private IEnumerator SmoothLerp(Transform hex)
    {
        Vector3 startingPos = hex.transform.localPosition;
        float speed = 0.05f;

        while (hex.transform.localPosition.y >= 0)
        {
            //hex.transform.localPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            hex.transform.localPosition = new Vector3(0, hex.transform.localPosition.y - speed, 0f);
            yield return null;
        }
    }

    private void MoveToGridPoint(Hexagons hex)
    {
        Vector3 startingPos = hex.transform.localPosition;
        float speed = 0.02f;

        while (hex.transform.localPosition.y >= 0)
        {
            //hex.transform.localPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            hex.transform.localPosition = new Vector3(0, hex.transform.localPosition.y - speed, 0f);
        }
    }
}