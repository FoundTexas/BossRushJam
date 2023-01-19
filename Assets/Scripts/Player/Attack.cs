using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] int team;
    [SerializeField] LayerMask grabable;
    [SerializeField] Transform grabTransform;
    float draggingTime;
    bool dragging;
    Vector2 dir = Vector2.right;
    GrabObj drag;

    void Start()
    {
        grabTransform = transform.GetChild(0);
    }
    void Update()
    {
        Vector2 newVect = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(newVect != Vector2.zero)
            dir = newVect;

        if (Input.GetKey("k"))
        {
            if (draggingTime <= 0.5f)
                draggingTime += Time.deltaTime;

            if ((!dragging || !drag) && draggingTime > 0.17f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 0.6f, grabable);
                if (hit.collider != null)
                    drag = hit.transform.gameObject.GetComponent<GrabObj>();

                if (drag)
                {
                    dragging = true;
                    drag.SetParent(grabTransform);
                }
            }
        }
        else if (Input.GetKeyUp("k"))
        {
            draggingTime = 0;
            dragging = false;
            if (drag)
                drag = drag.ThrowObj(dir + Vector2.up/3, team);
        }
        else if (Input.GetKeyDown("l"))
        {
            Debug.Log("attack: " + dir);
            Collider2D[] obs = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y) + dir, 0.5f);
            foreach (Collider2D col in obs)
            {
                IDamage idamageable = col.gameObject.GetComponent<IDamage>();
                if (idamageable != null)
                {
                    if (idamageable.team != team)
                    {
                        idamageable.Hit((col.transform.position - transform.position).normalized, 1);
                    }
                }
            }
        }
    }
}
