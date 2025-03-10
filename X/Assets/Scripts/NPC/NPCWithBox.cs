using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWithBox : MonoBehaviour
{
    [SerializeField] private float distance, speed;
    [SerializeField] private Transform target, target1, target2;

    private int layerMask;

    void Start()
    {
        target = target1;

        layerMask = 1 << gameObject.layer | 1 << 2;
        layerMask = ~layerMask;
    }
    void Update()
    {
        MoveToTarget();

        if(GetWall())
        {
            GetComponent<Animator>().Play("NPC_2(drop)");
            StartCoroutine(WaitForWalk());
        }

        Debug.DrawRay(transform.position, Vector3.right * distance, Color.red);
    }
    private IEnumerator WaitForWalk()
    {
        yield return new WaitForSeconds(1);
        target = target2;
    }
    private void MoveToTarget()
    {
        if(Vector2.Distance(transform.position, target.position) >= 0.1)
        {
            Vector2 targetPosition = new Vector2(target.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
    private bool GetWall() 
    {
        bool result = false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, distance, layerMask);
        if (hit.collider)
        {
            result = true;
        }

        return result;
    }
}
