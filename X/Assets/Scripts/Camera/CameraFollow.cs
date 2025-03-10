using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smooth = 2.5f; // ����������� ��� ���������� �� ����������
    public float offset; // �������� �������� (��������� = 0)
    private Vector2 velocity = Vector3.zero;
    public SpriteRenderer boundsMap; // ������, � ������ �������� ����� ������������ ������
    public bool useBounds = true; // ������������ ��� ���, ������� ��� ������

    private Transform player;
    private Vector3 min, max, direction;
    private static CameraFollow _inst;
    private Camera cam;

    void Awake()
    {
        _inst = this;
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        CalculateBounds();
        FindPlayer();
    }

    // �������������, ��� ������������� �� ������� ������
    public static void UseCameraBounds(bool value)
    {
        _inst.UseCameraBounds_inst(value);
    }

    // ����� ���������, ���� �� ��� ���������, � ����� ���������
    // ��� ������ �� ������� ������, �����: CameraFollow.use.FindPlayer();
    public static void FindPlayer()
    {
        _inst.FindPlayer_inst();
    }

    // ���� � �������� ����, ���� �������� ���������� ������
    // ��� �������� "Orthographic Size", �� ������� ������� ����� ������ ������� ��������
    public static void CalculateBounds()
    {
        _inst.CalculateBounds_inst();
    }

    void CalculateBounds_inst()
    {
        if (boundsMap == null) return;
        Bounds bounds = Camera2DBounds();
        min = bounds.max + boundsMap.bounds.min;
        max = bounds.min + boundsMap.bounds.max;
    }

    void FindPlayer_inst()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player)
        {
            direction = player.right;
            Vector3 position = player.position + direction * offset;
            position.z = transform.position.z;
            transform.position = MoveInside(position, new Vector3(min.x, min.y, position.z), new Vector3(max.x, max.y, position.z));
        }
    }

    void UseCameraBounds_inst(bool value)
    {
        useBounds = value;
    }

    Bounds Camera2DBounds()
    {
        float height = cam.orthographicSize * 2;
        return new Bounds(Vector3.zero, new Vector3(height * cam.aspect, height, 0));
    }

    Vector3 MoveInside(Vector3 current, Vector3 pMin, Vector3 pMax)
    {
        if (!useBounds || boundsMap == null) return current;
        current = Vector3.Max(current, pMin);
        current = Vector3.Min(current, pMax);
        return current;
    }
    void Follow()
    {
        direction = player.right;
        Vector2 position = player.position + direction * offset;
        position = MoveInside(position, new Vector2(min.x, min.y), new Vector2(max.x, max.y));
        transform.position = Vector2.SmoothDamp(transform.position, position, ref velocity, smooth * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
    void Update()
    {
        if (player)
        {
            Follow();
        }
    }
}
