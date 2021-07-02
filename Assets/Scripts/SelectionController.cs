using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    [SerializeField]
    private Hexagons _temp;
    private GameObject _singleCheck;
    private int _singleIndex;
    private bool _isSelectedTrio = false;
    public bool IsSelectedTrio { get => _isSelectedTrio; }
    [SerializeField]
    private GameObject[] _selectedHexagons = new GameObject[3];

    public int FindNearestTrio(Vector3 fromThis, GameObject[,] hexagons, int recursive)
    {
        if (recursive >= 3)
        {
            _isSelectedTrio = true;
            GoToPivotPoint();
            ShortSelectedTriangleClockwise(IsSelectedTriangleRight());
            return -1;
        }
        recursive++;
        if(_isSelectedTrio) ClearSelectedHighLight();
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis;
        foreach (GameObject potentialTarget in hexagons)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr && !potentialTarget.GetComponent<Hexagons>().IsSelected)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        _selectedHexagons[recursive - 1] = bestTarget;
        bestTarget.GetComponent<Hexagons>().IsSelected = true;
        return FindNearestTrio(fromThis, hexagons, recursive);
    }

    public void TurnSelectionToClockwise()
    {
        if(Grid.gameState==GameState.CanPlay)
        {
            _temp.X = _selectedHexagons[0].GetComponent<Hexagons>().X;
            _temp.Y = _selectedHexagons[0].GetComponent<Hexagons>().Y;
            _temp.gameObject.transform.position = _selectedHexagons[0].GetComponent<Hexagons>().gameObject.transform.position;
            for (int i = 0; i <= 1; i++)
            {
                _selectedHexagons[i].GetComponent<Hexagons>().SetPosition(
                    _selectedHexagons[i + 1].GetComponent<Hexagons>().X,
                    _selectedHexagons[i + 1].GetComponent<Hexagons>().Y);
                _selectedHexagons[i].transform.position = _selectedHexagons[i + 1].transform.position;
            }
            _selectedHexagons[2].GetComponent<Hexagons>().SetPosition(
                _temp.GetComponent<Hexagons>().X,
                _temp.GetComponent<Hexagons>().Y);
            _selectedHexagons[2].transform.position = _temp.transform.position;
            ShortSelectedTriangleClockwise(IsSelectedTriangleRight());
            ClearSelectedHighLight();
            Grid.gameState = GameState.Waiting;
            Grid.Instance.SetHexagonsNeighbours();
        }
    }

    public void TurnSelectionToCounterClockwise()
    {
        if (Grid.gameState == GameState.CanPlay)
        {
            _temp.X = _selectedHexagons[2].GetComponent<Hexagons>().X;
            _temp.Y = _selectedHexagons[2].GetComponent<Hexagons>().Y;
            _temp.gameObject.transform.position = _selectedHexagons[2].GetComponent<Hexagons>().gameObject.transform.position;
            for (int i = 1; i >= 0; i--)
            {
                _selectedHexagons[i + 1].GetComponent<Hexagons>().SetPosition(
                    _selectedHexagons[i].GetComponent<Hexagons>().X,
                    _selectedHexagons[i].GetComponent<Hexagons>().Y);
                _selectedHexagons[i + 1].transform.position = _selectedHexagons[i].transform.position;
            }
            _selectedHexagons[0].GetComponent<Hexagons>().SetPosition(
                _temp.GetComponent<Hexagons>().X,
                _temp.GetComponent<Hexagons>().Y);
            _selectedHexagons[0].transform.position = _temp.transform.position;
            ShortSelectedTriangleClockwise(IsSelectedTriangleRight());
            ClearSelectedHighLight();
            Grid.gameState = GameState.Waiting;
            Grid.Instance.SetHexagonsNeighbours();
        }
    }

    public void ClearSelectedHighLight()
    {
        for (int i = 0; i < _selectedHexagons.Length; i++)
        {
            _selectedHexagons[i].GetComponent<Hexagons>().IsSelected = false;
        }
        _isSelectedTrio = false;
    }

    private void GoToPivotPoint()
    {
        float x = 0, y = 0, z = 0;
        for (int i = 0; i < _selectedHexagons.Length; i++)
        {
            x += _selectedHexagons[i].transform.position.x;
            y += _selectedHexagons[i].transform.position.y;
            z += _selectedHexagons[i].transform.position.z;
        }
        this.transform.position = new Vector3(x / 3, y / 3, x / 3);
    }

    private void ShortSelectedTriangleClockwise(bool isRightAligned)
    {
        List<GameObject> CompareList = new List<GameObject>();
        if (isRightAligned)
        {
            for (int i = 0; i < _selectedHexagons.Length; i++)
            {
                if (_selectedHexagons[i] != _selectedHexagons[_singleIndex])
                {
                    CompareList.Add(_selectedHexagons[i]);
                }
            }
            _selectedHexagons[0] = GetLowerYValue(CompareList[0], CompareList[1]);
            _selectedHexagons[1] = GetUpperYValue(CompareList[0], CompareList[1]);
            _selectedHexagons[2] = _singleCheck;
        }
        else
        {
            for (int i = 0; i < _selectedHexagons.Length; i++)
            {
                if (_selectedHexagons[i] != _selectedHexagons[_singleIndex])
                {
                    CompareList.Add(_selectedHexagons[i]);
                }
            }
            _selectedHexagons[0] = GetUpperYValue(CompareList[0], CompareList[1]);
            _selectedHexagons[1] = GetLowerYValue(CompareList[0], CompareList[1]);
            _selectedHexagons[2] = _singleCheck;
        }
    }

    private bool IsSelectedTriangleRight()
    {
        if (_selectedHexagons[0].GetComponent<Hexagons>().X == _selectedHexagons[1].GetComponent<Hexagons>().X)
        {
            _singleIndex = 2;
            _singleCheck = _selectedHexagons[2];
            if (_singleCheck.GetComponent<Hexagons>().X > _selectedHexagons[0].GetComponent<Hexagons>().X)
                return true;
            else return false;
        }
        else if (_selectedHexagons[0].GetComponent<Hexagons>().X == _selectedHexagons[2].GetComponent<Hexagons>().X)
        {
            _singleIndex = 1;
            _singleCheck = _selectedHexagons[1];
            if (_singleCheck.GetComponent<Hexagons>().X > _selectedHexagons[0].GetComponent<Hexagons>().X)
                return true;
            else return false;
        }
        else
        {
            _singleIndex = 0;
            _singleCheck = _selectedHexagons[0];
            if (_singleCheck.GetComponent<Hexagons>().X > _selectedHexagons[1].GetComponent<Hexagons>().X)
                return true;
            else return false;
        }
    }

    private GameObject GetLowerYValue(GameObject h1, GameObject h2)
    {
        if (h1.GetComponent<Hexagons>().Y == Mathf.Min(h1.GetComponent<Hexagons>().Y, h2.GetComponent<Hexagons>().Y)) return h1;
        else return h2;
    }

    private GameObject GetUpperYValue(GameObject h1, GameObject h2)
    {
        if (h1.GetComponent<Hexagons>().Y == Mathf.Max(h1.GetComponent<Hexagons>().Y, h2.GetComponent<Hexagons>().Y)) return h1;
        else return h2;
    }
}