using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagons : MonoBehaviour
{
    private Transform _moveTarget;
    private bool _moving=false;
    [SerializeField]
    private GameObject _highLightedObject;
    private int _x, _y;
    private bool _isSelected;
    private Color _hexColor = Color.white;
    [SerializeField]
    private SpriteRenderer _hexSprite;
    [SerializeField]
    private List<Hexagons> _neighbors = new List<Hexagons>();
    [SerializeField]
    private List<Hexagons> _sameColorNeighbors = new List<Hexagons>();
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
    public Color HexColor
    {
        get
        {
            return _hexColor;
        }
        set
        {
            _hexColor = value;
            if (_hexSprite != null) _hexSprite.color = value;
        }
    }
    public List<Hexagons> SameColorNeighbors { get => _sameColorNeighbors; set => _sameColorNeighbors = value; }
    public bool Moving 
    { 
        get{return _moving;}
        set
        {
            _moveTarget=Grid.Instance.GetIndexPosition(_x,_y).transform;
            this.transform.position=new Vector3(_moveTarget.position.x,this.transform.position.y,this.transform.position.z);
            _moving=value;
        }
    }

    private void Start()
    {
        if (_hexSprite != null) _hexSprite.color = _hexColor;
    }

    public void SetPosition(int xnew, int ynew)
    {
        _x=xnew;
        _y=ynew;
        Grid.Instance.ChangeHexIndex(this.gameObject,_x,_y);
    }

    private void SetHighLightActivate(bool value)
    {
        _highLightedObject.SetActive(value);
    }

    public void AddNeighbour(Hexagons _hex)
    {
        _neighbors.Add(_hex);
        if(_hex.HexColor==_hexColor) _sameColorNeighbors.Add(_hex);
    }

    public void ClearNeighbours()
    {
        _neighbors.Clear();
    }

    public void ClearSameColorNeighbors()
    {
        _sameColorNeighbors.Clear();
    }

    void OnDisable()
    {
        Grid.Instance.IAmExploded(_x, _y);
    }

    void Update()
    {
        if(Moving)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _moveTarget.position, 0.05f);
            if(this.transform.position==_moveTarget.transform.position)
            {
                _moving=false;
                
            }
        }
    }
}
