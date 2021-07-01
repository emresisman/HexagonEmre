using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagons : MonoBehaviour
{
    [SerializeField]
    private GameObject _highLightedObject;
    private int _x, _y;
    private bool _isSelected;
    private Color _hexColor;
    private GridPoints _myGridPoint;
    [SerializeField]
    private SpriteRenderer _hexSprite;
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
    public GridPoints MyGridPoint { get => _myGridPoint; set => _myGridPoint = value; }
    public Color HexColor
    {
        get
        {
            return _hexColor;
        }
        set
        {
            _hexColor = value;
            _hexSprite.color = value;
            _myGridPoint.MyHexColor = value;
        }
    }
    
    private void Start()
    {
        _hexSprite.color = _hexColor;
    }

    void SetHighLightActivate(bool value)
    {
        _highLightedObject.SetActive(value);
    }
}
