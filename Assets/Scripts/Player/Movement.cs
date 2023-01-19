using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float jumpforce, walljumpforce, speed, holdMultiplier, stamina;
    [SerializeField] LayerMask Ground;
    bool canJump, canWallJump, canFlash;
    float jumpcooldown = 0, holdtime = 0, curStamina;
    Vector2 mov;
    Rigidbody2D rb;
    SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.gray;
        canFlash = true;
    }

    void Update()
    {
        canJump = Physics2D.Raycast(transform.position, transform.up * -1, 0.7f, Ground);
        canWallJump = Physics2D.Raycast(transform.position, transform.right, 0.6f, Ground);
    }

    void LateUpdate()
    {
        mov = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (mov.x != 0 && jumpcooldown <= 0)
        {
            transform.rotation = Quaternion.Euler(0, mov.x < 0 ? 180 : 0, 0);
            rb.velocity = new Vector2(canWallJump ? 0 : mov.x * (canJump ? speed : speed / 2), rb.velocity.y);
        }

        rb.isKinematic = (mov.x != 0 && canWallJump && !canJump && stamina > 0);

        if(!rb.isKinematic && stamina <= 0f && canFlash)
        {
            canFlash = false;
            StartCoroutine(flashColor(Color.red, 0.4f));
        }

        if (canWallJump && stamina > 0 && mov.x != 0)
        {
            if (!canJump)
                stamina -= Time.deltaTime;

            rb.velocity = new Vector2(rb.velocity.x, mov.y * speed * 0.75f);
        }
        else if (stamina < 0.75f && (canJump || !canWallJump))
        {
            StartCoroutine(flashColor(Color.green, 0.6f));
            stamina = 0.75f;
        }

        if (holdtime > 0.25f && canFlash)
        {
            canFlash = false;
            StartCoroutine(flashColor(Color.yellow, 0.5f));
        }

        if (Input.GetKeyUp("space") && holdtime > 0.5f)
            Jump(holdMultiplier);
        else if (Input.GetKeyUp("space") && holdtime < 0.75f)
            Jump(1);

        if (Input.GetKey("space"))
            holdtime += Time.deltaTime;

        if (jumpcooldown > 0)
            jumpcooldown -= Time.deltaTime;
    }

    IEnumerator flashColor(Color color, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time / 2)
        {
            sr.color = Vector4.Lerp(Color.gray, color, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        sr.color = color;
        while (elapsedTime < time)
        {
            sr.color = Vector4.Lerp(color, Color.gray, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        sr.color = Color.gray;
        canFlash = true;
        yield return null;
    }

    void Jump(float multi)
    {
        canFlash = true;
        StopAllCoroutines();
        sr.color = Color.gray;
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
