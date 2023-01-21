using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float jumpforce, walljumpforce, speed, holdMultiplier, stamina;
    [SerializeField] LayerMask Ground;
    public static bool canJump, canWallJump, canFlash;
    float jumpcooldown = 0, holdtime = 0, curStamina, curSpeed;
    Vector2 mov;
    Rigidbody2D rb;
    SpriteRenderer sr, hsr;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hsr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        canFlash = true;
        curStamina = stamina;
    }

    void Update()
    {
        canJump = Physics2D.Raycast(transform.position, transform.up * -1, 0.75f, Ground);
        canWallJump = Physics2D.Raycast(transform.position, transform.right, 0.75f, Ground);

        anim.SetBool("OnGround", canJump);
        anim.SetBool("OnWall", canWallJump);
        anim.SetFloat("mag", Mathf.Abs(mov.x));
    }

    void LateUpdate()
    {
        curSpeed = speed * (holdtime >= 0.5f ? 0 : 1);
        mov = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, mov.y < 0 ? -20 : mov.y > 0 ? 20 : 0);

        if (mov.x != 0 && jumpcooldown <= 0)
        {
            transform.rotation = Quaternion.Euler(0, mov.x < 0 ? 180 : 0, 0);
            rb.velocity = new Vector2(canWallJump ? 0 : mov.x * curSpeed, rb.velocity.y);
        }

        rb.isKinematic = (mov.x != 0 && canWallJump && !canJump && curStamina > 0);

        if (!rb.isKinematic && curStamina <= 0f && canFlash)
        {
            canFlash = false;
            StartCoroutine(flashColor(Color.red, 0.4f));
        }
        if (holdtime > 0.25f && canFlash)
        {
            canFlash = false;
            StartCoroutine(flashColor(Color.yellow, 0.5f));
        }

        if (canWallJump && curStamina > 0 && mov.x != 0)
        {
            if (!canJump && mov.y != 0)
                curStamina -= Time.deltaTime;

            rb.velocity = new Vector2(rb.velocity.x, mov.y * curSpeed * 0.75f);
        }
        else if (curStamina < 0.75f && (canJump || !canWallJump))
        {
            StartCoroutine(flashColor(Color.green, 0.6f));
            curStamina = stamina;
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
            sr.color = Vector4.Lerp(Color.white, color, (elapsedTime / time));
            hsr.color = Vector4.Lerp(Color.white, color, (elapsedTime / time));
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        sr.color = color;
        hsr.color = color;
        while (elapsedTime < time)
        {
            sr.color = Vector4.Lerp(color, Color.white, (elapsedTime / time));
            hsr.color = Vector4.Lerp(color, Color.white, (elapsedTime / time));

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        sr.color = Color.white;
        hsr.color = Color.white;
        canFlash = true;
        yield return null;
    }

    void Jump(float multi)
    {
        canFlash = true;
        StopAllCoroutines();
        sr.color = Color.white;
        hsr.color = Color.white;
        holdtime = 0;

        if (canJump)
        {
            rb.velocity = (mov == Vector2.zero ? transform.up : new Vector3(mov.x, 1)) * jumpforce * multi;
        }
        else if (canWallJump && !canJump)
        {
            jumpcooldown = 1f;
            rb.velocity = new Vector2(transform.right.x * -1 * walljumpforce * multi, transform.up.y * jumpforce * multi);
            transform.rotation = Quaternion.Euler(0, transform.rotation.y == 0 ? 180 : 0, 0);
        }
    }
}
