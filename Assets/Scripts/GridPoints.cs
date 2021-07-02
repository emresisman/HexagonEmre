using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoints : MonoBehaviour
{

    private IEnumerator SmoothLerp(Transform hex)
    {
        Vector3 startingPos = hex.transform.localPosition;
        float speed = 0.05f;

        while (hex.transform.localPosition.y >= 0)
        {
            //hex.transform.localPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            hex.transform.localPosition = new Vector3(0, hex.transform.localPosition.y - speed, 0f);
            yield return null;
        }
    }

    private void MoveToGridPoint(Hexagons hex)
    {
        Vector3 startingPos = hex.transform.localPosition;
        float speed = 0.02f;

        while (hex.transform.localPosition.y >= 0)
        {
            //hex.transform.localPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            hex.transform.localPosition = new Vector3(0, hex.transform.localPosition.y - speed, 0f);
        }
    }
}