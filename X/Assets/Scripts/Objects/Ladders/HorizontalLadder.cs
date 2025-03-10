using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalLadder : MonoBehaviour
{
    public EdgeCollider2D UpCollider;
    public EdgeCollider2D DownCollider;

    private bool isPlayer;

    void Update()
    {
        if(isPlayer)
        {
            if(Input.GetKey(KeyCode.S)) // ����� ����
            {
                UpCollider.enabled = false;
                DownCollider.enabled = true;
            }
            else if (Input.GetKey(KeyCode.W)) // ������ �����
            {
                UpCollider.enabled = true;
                DownCollider.enabled = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) // ���� ����� �����, �� ����� ������� ������
    {
        if(collision.gameObject.tag == "Player")
        {
            isPlayer = true;
            Debug.Log("����� �����");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayer = false;
        }
    }
}
