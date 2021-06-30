using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagons : MonoBehaviour
{
    [SerializeField]
    private GameObject _highLightedObject;
    private int _x, _y;
    private bool _isSelected;
    private List<Hexagons> _neighbors = new List<Hexagons>();
    private List<Hexagons> _neighborsSameColor = new List<Hexagons>();
    private Color _hexColor;
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
            SetHighLightActivate(value);
        }
    }    
    public int Y { get => _y; set => _y = value; }
    public int X { get => _x; set => _x = value; }
    public List<Hexagons> Neighbors { get => _neighbors; }
    public Color HexColor { get => _hexColor; set => _hexColor = value; }

    private void Awake()
    {
        HexColor = this.GetComponent<SpriteRenderer>().color;
    }

    void SetHighLightActivate(bool value)
    {
        _highLightedObject.SetActive(value);
    }

    public void AddNeighbors(Hexagons _neighbor)
    {
        _neighbors.Add(_neighbor);
        if (_neighbor.HexColor == this._hexColor) _neighborsSameColor.Add(_neighbor);
    }
}
