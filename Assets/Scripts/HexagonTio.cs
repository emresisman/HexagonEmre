using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexagonTio : MonoBehaviour
{
    public bool isTrioExist = false;
    private List<Hexagons> willRemoveHex = new List<Hexagons>();
    [SerializeField]
    private HexagonPool _hexPool;

    public void DetectHexagonTrio(Hexagons mainHexagon, List<Hexagons> neighbourHexagons)
    {
        if (neighbourHexagons.Count > 1)
        {
            for (int i = 0; i < neighbourHexagons.Count - 1; i++)
            {
                for (int j = i; j < neighbourHexagons.Count - 1; j++)
                {
                    if (AreTheyNeighbor(neighbourHexagons[i], neighbourHexagons[j + 1]))
                    {
                        isTrioExist = true;
                        AddHexToRemoveList(neighbourHexagons[i]);
                        AddHexToRemoveList(neighbourHexagons[j + 1]);
                        AddHexToRemoveList(mainHexagon);
                    }
                }
            }
        }
    }

    private bool AreTheyNeighbor(Hexagons gp1, Hexagons gp2)
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

    public void ResetColor()
    {
        foreach(Hexagons hex in willRemoveHex)
        {
            hex.HexColor = Grid.Instance.Colors[Random.Range(0, Grid.Instance.Colors.Length)];
        }
        Grid.Instance.SetHexagonsNeighbours();
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
                //hex.gameObject.transform.parent = _hexPool.gameObject.transform;
                hex.gameObject.SetActive(false);
            }
            willRemoveHex.Clear();
        }
    }
}
