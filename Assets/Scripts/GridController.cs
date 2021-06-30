using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    protected SelectionController _selection;

    void Awake()
    {
        _selection = GameObject.Find("SelectionController").GetComponent<SelectionController>();
    }

    public void TurnSelectionToClockwise()
    {
        Color _temp = _selection.SelectedHexagons[2].GetComponent<SpriteRenderer>().color;
        for (int i = 1; i >= 0; i--)
        {
            _selection.SelectedHexagons[i + 1].GetComponent<SpriteRenderer>().color = _selection.SelectedHexagons[i].GetComponent<SpriteRenderer>().color;
        }
        _selection.SelectedHexagons[0].GetComponent<SpriteRenderer>().color = _temp;
    }

    public void TurnSelectionToCounterClockwise()
    {
        Color _temp = _selection.SelectedHexagons[0].GetComponent<SpriteRenderer>().color;
        for (int i = 0; i <= 1; i++)
        {
            _selection.SelectedHexagons[i].GetComponent<SpriteRenderer>().color = _selection.SelectedHexagons[i + 1].GetComponent<SpriteRenderer>().color;
        }
        _selection.SelectedHexagons[2].GetComponent<SpriteRenderer>().color = _temp;

    }
}
