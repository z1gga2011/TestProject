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
            if(Input.GetKey(KeyCode.S)) // спуск вниз
            {
                UpCollider.enabled = false;
                DownCollider.enabled = true;
            }
            else if (Input.GetKey(KeyCode.W)) // подьем вверх
            {
                UpCollider.enabled = true;
                DownCollider.enabled = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) // если игрок зашел, то можно прожать кнопку
    {
        if(collision.gameObject.tag == "Player")
        {
            isPlayer = true;
            Debug.Log("игрок здесь");
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
