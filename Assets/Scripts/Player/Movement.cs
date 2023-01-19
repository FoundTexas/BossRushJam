using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float jumpforce, walljumpforce, speed, holdMultiplier, stamina;
    [SerializeField] LayerMask Ground;
    static public bool canJump, canWallJump;
    float jumpcooldown = 0, holdtime = 0, curStamina;
    Vector2 mov;
    Rigidbody2D rb;

    SpriteRenderer sr;

    void Start()
    {
        curStamina = stamina;
        rb = GetComponent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        canJump = Physics2D.Raycast(transform.position, transform.up * -1, 1, Ground);
        canWallJump = Physics2D.Raycast(transform.position, transform.right, 0.6f, Ground);

        mov = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (mov.x != 0 && jumpcooldown <= 0)
        {
            transform.rotation = Quaternion.Euler(0, mov.x < 0 ? 180 : 0, 0);
            rb.velocity = new Vector2(canWallJump ? 0 : mov.x * (canJump ? speed : speed / 2), rb.velocity.y);
        }

        rb.isKinematic = (mov.x != 0 && canWallJump && !canJump && curStamina > 0);

        if (canWallJump && curStamina > 0)
        {
            curStamina -= Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, mov.y * speed * 0.75f);
        }
        else if (curStamina <= stamina && canJump)
            curStamina += Time.deltaTime;

        if (Input.GetKeyUp("space") && holdtime > 0.5f)
            Jump(holdMultiplier);
        else if (Input.GetKeyUp("space") && holdtime < 0.75f)
            Jump(1);

        if (Input.GetKey("space"))
            holdtime += Time.deltaTime;

        if (jumpcooldown > 0)
            jumpcooldown -= Time.deltaTime;
    }

    void Jump(float multi)
    {
        holdtime = 0;

        if (canJump)
        {
            rb.velocity = transform.up * jumpforce * multi;
        }
        else if (canWallJump && !canJump)
        {
            jumpcooldown = 1f;
            rb.velocity = new Vector2(transform.right.x * -1 * walljumpforce * multi, transform.up.y * jumpforce * multi);
            transform.rotation = Quaternion.Euler(0, transform.rotation.x == 0 ? 180 : 0, 0);
        }
    }
}
