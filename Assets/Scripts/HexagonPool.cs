using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonPool : MonoBehaviour
{
    [SerializeField]
    private List<Hexagons> _hexInPool = new List<Hexagons>();
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

    #region Singleton
    private static HexagonPool _instance;
    public static HexagonPool Instance { get => _instance; set => _instance = value; }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public Hexagons GetHexagonFromPool()
    {
        Hexagons hex = _hexInPool[0];
        hex.gameObject.SetActive(true);
        hex.HexColor = Grid.Instance.Colors[Random.Range(0, Grid.Instance.Colors.Length)];
        _hexInPool.RemoveAt(0);
        return hex;
    }

    private void ResetAllInPool()
    {
        foreach(Hexagons hex in _hexInPool)
        {
            hex.HexColor = Color.white;
            //hex.gameObject.transform.parent = this.transform;
            hex.gameObject.transform.position = new Vector3(hex.gameObject.transform.position.x, 12, 0);
        }
    }
}
