using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] private GameInput gameInput;
    [SerializeField] public float interactionRange = 1f; // How close the player needs to be to interact with an item or NPC
    [SerializeField] public float skinWidth = 0.01f;   // ����ǽ��İ�ȫ���룬����Ƕ��
    [SerializeField] public LayerMask obstacleLayer;   // ǽ�����ڵ�ͼ��

    private Vector2 movement;
    private Rigidbody2D rb;
    private float mygravity = 0;
    private bool isWalking;
    private bool isFacingRight = false; // Default to false if the sprite initially faces left

    private Animator animator; // Reference to the Animator component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mygravity = rb.gravityScale;

        // Get the Animator component attached to the player
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Get the normalized movement input from the GameInput script
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        if (inputVector.x == 0)
        {
            rb.drag = 10f; // ���ýϸߵ�Ħ������ʹ��ɫֹͣ
            rb.gravityScale = 0;
        }
        else
        {
            rb.drag = 0f;  // ��������룬�ָ�������Ħ����
            rb.gravityScale = mygravity;
        }

        float distance = inputVector.magnitude * moveSpeed * Time.fixedDeltaTime;
        // Create a movement direction vector for 2D, ignoring the z-axis
        Vector2 targetPosition = rb.position + inputVector * distance;

        // ���ǰ���Ƿ����ϰ���
        RaycastHit2D hit = Physics2D.BoxCast(rb.position, rb.GetComponent<Collider2D>().bounds.size, 0, inputVector, distance, obstacleLayer);

        if (hit.collider != null)
        {
            // �����⵽�ϰ����ֻ�ƶ����ϰ���ǰ�İ�ȫ����
            float adjustedDistance = hit.distance - skinWidth;
            if (adjustedDistance > 0)
            {
                Vector2 newPosition = rb.position + inputVector * adjustedDistance;
                rb.MovePosition(newPosition);
            }
        }
        else
        {
            // ���û���ϰ�������ƶ�
            rb.MovePosition(targetPosition);
        }

        // Update the walking state (only true if moving along the x-axis)
        isWalking = inputVector.x != 0;

        // Update the Animator with the walking state
        if (animator != null)
        {
            animator.SetBool("isWalk", isWalking);
        }

        // Flip the player sprite to face the movement direction
        if ((inputVector.x > 0 && !isFacingRight) ||
            (inputVector.x < 0 && isFacingRight))
        {
            Flip();
        }
    }

    // Flips the player's sprite by adjusting the localScale
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Inverts the x scale
        transform.localScale = localScale;
    }

    // Returns whether the player is currently walking
    public bool IsWalking()
    {
        return isWalking;
    }
}
