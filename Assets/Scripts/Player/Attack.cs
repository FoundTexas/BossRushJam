using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] int team;
    [SerializeField] LayerMask grabable, attack;
    [SerializeField] Transform grabTransform;
    float draggingTime;
    bool dragging;
    Vector2 dir = Vector2.right;
    GrabObj drag;

    void Start()
    {
        grabTransform = transform.GetChild(0).GetChild(0);
    }
    void Update()
    {
        Vector2 newVect = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        dir = newVect;

        if (Input.GetKey("k"))
        {
            if (draggingTime <= 0.5f)
                draggingTime += Time.deltaTime;

            if ((!dragging || !drag) && draggingTime > 0.17f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position, dir, 0.6f, grabable);
                if (hit.collider != null)
                    drag = hit.transform.gameObject.GetComponent<GrabObj>();

                if (drag)
                {
                    dragging = true;
                    drag.SetParent(grabTransform, team);
                }
            }
        }
        else if (Input.GetKeyUp("k"))
        {
            draggingTime = 0;
            dragging = false;
            if (drag)
                drag = drag.ThrowObj(Movement.canWallJump? new Vector2(dir.x*-1, dir.y):dir, team);
        }
        else if (Input.GetKeyDown("l"))
        {
            Collider2D[] obs = Physics2D.OverlapCircleAll(new Vector2(transform.GetChild(0).position.x, transform.GetChild(0).position.y) + dir, 1f,attack);
            Debug.Log("attack: " + dir + " " + obs.Length);
            foreach (Collider2D col in obs)
            {
                IDamage idamageable = col.gameObject.GetComponent<IDamage>();
                if (idamageable != null)
                {
                    Debug.Log(col.name);
                    Vector2 vec = (col.transform.position - transform.position).normalized;
                    idamageable.Hit(new Vector2(vec.x, 0.66f), 4, team);
                }
            }
        }
    }
}
