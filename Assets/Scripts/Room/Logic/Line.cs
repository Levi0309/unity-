using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float offectSpeed;
    private void Update()
    {
        if (lineRenderer!=null)
        {
            Vector2 offect = lineRenderer.material.mainTextureOffset;
            offect.x += offectSpeed * Time.deltaTime;
            lineRenderer.material.mainTextureOffset = offect;
        }
    }
}
