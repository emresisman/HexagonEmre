using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexagonTio : MonoBehaviour
{
    private List<Hexagons> willRemoveHex = new List<Hexagons>();
    [SerializeField]
    private HexagonPool _hexPool;

    public void DetectHexagonTrio(GridPoints mainGrid, List<GridPoints> neighbourGrid)
    {
        if (neighbourGrid.Count > 1)
        {
            for (int i = 0; i < neighbourGrid.Count - 1; i++)
            {
                for (int j = i; j < neighbourGrid.Count - 1; j++)
                {
                    if (AreTheyNeighbor(neighbourGrid[i], neighbourGrid[j + 1]))
                    {
                        AddHexToRemoveList(neighbourGrid[i].MyHex);
                        AddHexToRemoveList(neighbourGrid[j + 1].MyHex);
                        AddHexToRemoveList(mainGrid.MyHex);
                    }
                }
            }
        }
    }

    private bool AreTheyNeighbor(GridPoints gp1, GridPoints gp2)
    {
        if (gp1.SameColorNeighbors.Contains(gp2)) return true;
        return false;
    }


    private void AddHexToRemoveList(Hexagons hex)
    {
        if (!IsHexExist(hex))
        {
            willRemoveHex.Add(hex);
        }
    }

    private bool IsHexExist(Hexagons hex)
    {
        if (willRemoveHex.Contains(hex)) return true;
        return false;
    }

    public void ExplodeHexagons()
    {
        _hexPool.HexInPool = willRemoveHex.ToList();
        if (willRemoveHex.Count > 0)
        {
            foreach (Hexagons hex in willRemoveHex)
            {
                hex.MyGridPoint.MyHex = null;
                hex.MyGridPoint = null;
                //hex.gameObject.transform.parent = _hexPool.gameObject.transform;
                hex.gameObject.SetActive(false);
            }
            willRemoveHex.Clear();
        }
    }
}
