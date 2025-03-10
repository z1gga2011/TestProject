using Game.Core.Rendering;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public AudioSource AudioSource;
    public Transform StartPos, EndPos, CableStartPos, CableEndPos;
    public Transform point;

    private MultiLineRenderer2D lineRenderer;
    void Start()
    {
        lineRenderer = transform.GetChild(0).GetComponent<MultiLineRenderer2D>();

        point.position = (CableStartPos.position + CableEndPos.position) / 2;

        lineRenderer.Points[0] = CableStartPos.position;
        lineRenderer.Points[1] = point.position;
        lineRenderer.Points[2] = CableEndPos.position;
    }
    public void PlayerFollow(Vector2 playerPosition)
    {
        lineRenderer.Points[1] = playerPosition + Vector2.up * 0.4f;
    }
    public void ResetMovePointPosition()
    {
        lineRenderer.Points[1] = (CableStartPos.position + CableEndPos.position) / 2;
    }
}
