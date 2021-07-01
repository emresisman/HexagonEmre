using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonPool : MonoBehaviour
{
    [SerializeField]
    private List<Hexagons> _hexInPool = new List<Hexagons>();
    [SerializeField]
    private Grid _grid;
    public List<Hexagons> HexInPool
    {
        get
        {
            return _hexInPool;
        }
        set
        {
            _hexInPool = value;
            ResetAllInPool();
        }
    }

    public Hexagons GetHexagonFromPool(GridPoints _gridPoint)
    {
        Hexagons hex = _hexInPool[0];
        hex.MyGridPoint=_gridPoint;
        hex.HexColor = _grid.Colors[Random.Range(0, _grid.Colors.Length)];
        _hexInPool.RemoveAt(0);
        return hex;
    }

    private void ResetAllInPool()
    {
        foreach(Hexagons hex in _hexInPool)
        {
            hex.HexColor = Color.white;
            hex.gameObject.transform.parent = this.transform;
            hex.gameObject.transform.localPosition = new Vector3(hex.gameObject.transform.localPosition.x, 0, 0);
        }
    }
}
