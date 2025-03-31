using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private Animator animator;

    private bool isJumping; // Флаг, чтобы отслеживать, прыгает ли персонаж

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isJumping = false;
    }

    void Update()
    {
        Move();
        Jump();
        UpdateAnimation();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Отзеркаливание персонажа в зависимости от направления движения
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Лицом вправо
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Лицом влево
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true; // Устанавливаем флаг при прыжке
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false; // Сбрасываем флаг при приземлении
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void UpdateAnimation()
    {
        if (isGrounded)
        {
            if (rb.velocity.x != 0)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isJumping", false);
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            if (isJumping) // Проверяем флаг, чтобы воспроизвести анимацию прыжка
            {
                animator.SetBool("isJumping", true);
            }
        }
    }
}