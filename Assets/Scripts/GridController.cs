using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    protected SelectionController _selection;
    private Grid _grid;

    void Awake()
    {
        _grid = GameObject.Find("Grid").GetComponent<Grid>();
        _selection = GameObject.Find("SelectionController").GetComponent<SelectionController>();
    }

    public void TurnSelectionToClockwise()
    {
        Color _temp = _selection.SelectedHexagons[2].GetComponent<Hexagons>().HexColor;
        for (int i = 1; i >= 0; i--)
        {
            _selection.SelectedHexagons[i + 1].GetComponent<Hexagons>().HexColor = _selection.SelectedHexagons[i].GetComponent<Hexagons>().HexColor;
        }
        _selection.SelectedHexagons[0].GetComponent<Hexagons>().HexColor = _temp;
        _grid.CheckSameColorNeighbours();
    }

    public void TurnSelectionToCounterClockwise()
    {
        Color _temp = _selection.SelectedHexagons[0].GetComponent<Hexagons>().HexColor;
        for (int i = 0; i <= 1; i++)
        {
            _selection.SelectedHexagons[i].GetComponent<Hexagons>().HexColor = _selection.SelectedHexagons[i + 1].GetComponent<Hexagons>().HexColor;
        }
        _selection.SelectedHexagons[2].GetComponent<Hexagons>().HexColor = _temp;
        _grid.CheckSameColorNeighbours();
    }
}
