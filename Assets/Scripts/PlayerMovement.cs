using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed = 5f;
    public Rigidbody2D rigid;
    public Animator animator;
    public SpriteRenderer spriteRenderer;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y*-100) + 10;
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized*speed*Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);

        if (inputVec.x < 0)
        
            spriteRenderer.flipX = true;
        else if (inputVec.x > 0)
            spriteRenderer.flipX = false;
        
        animator.SetFloat("Xinput", inputVec.x);
        animator.SetFloat("Yinput", inputVec.y);

    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }



    // void FixedUpdate()
    // {
    //     float horizontal = Input.GetAxis("Horizontal");
    //     float vertical  = Input.GetAxis("Vertical");

    //     rigid.linearVelocity = new Vector2(horizontal, vertical) * speed;
    // }
    
}
