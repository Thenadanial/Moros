using UnityEngine;
using UnityEngine.UI; // For UI text display

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float interactionRange = 1f; // How close the player needs to be to interact with an item or NPC
    public float skinWidth = 0.1f;   // ����ǽ��İ�ȫ���룬����Ƕ��
    public LayerMask obstacleLayer;   // ǽ�����ڵ�ͼ��

    private Rigidbody2D rb;
    private Vector2 movement;

    // Reference to the NPC the player is interacting with
    private GameObject npcInRange;
    private Text dialogueText; // Reference to the UI Text for dialogue

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Assuming there's a UI Text element in your scene for dialogue display
        dialogueText = GameObject.Find("DialogueText").GetComponent<Text>();
        dialogueText.text = ""; // Ensure no text is showing at the start
    }

    void Update()
    {
        // ��ȡ�ƶ�����
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        // Handle search action when pressing the "F" key for items
        if (Input.GetKeyDown(KeyCode.F) && npcInRange != null)
        {
            SearchItem();
        }

        // Handle talk action when pressing the "T" key for NPCs
        if (Input.GetKeyDown(KeyCode.T) && npcInRange != null)
        {
            TalkToNPC();
        }
    }

    void FixedUpdate()
    {
        // �����ƶ�������Ŀ��λ��
        Vector2 direction = movement.normalized;
        float distance = movement.magnitude * speed * Time.fixedDeltaTime;
        Vector2 targetPosition = rb.position + direction * distance;

        // ʹ�� BoxCast �����ǰ���Ƿ����ϰ���
        RaycastHit2D hit = Physics2D.BoxCast(rb.position, rb.GetComponent<Collider2D>().bounds.size, 0, direction, distance, obstacleLayer);

        if (hit.collider != null)
        {
            // �����⵽�ϰ����ֻ�ƶ����ϰ���ǰ�İ�ȫ����
            float adjustedDistance = hit.distance - skinWidth;
            if (adjustedDistance > 0)
            {
                Vector2 newPosition = rb.position + direction * adjustedDistance;
                rb.MovePosition(newPosition);
            }
        }
        else
        {
            // ���û���ϰ�������ƶ�
            rb.MovePosition(targetPosition);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Detect if the player is near an NPC
        if (other.CompareTag("NPC"))
        {
            npcInRange = other.gameObject;
            Debug.Log("Press T to talk to the NPC!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Exit detection when the player moves away
        if (other.CompareTag("NPC"))
        {
            npcInRange = null;
            dialogueText.text = ""; // Clear dialogue when leaving
        }
    }

    void TalkToNPC()
    {
        // Here you can display the NPC's dialogue
        Debug.Log("Talking to NPC: " + npcInRange.name);

        // Example: Display a basic dialogue
        dialogueText.text = "Hello! Welcome to the Dute Town.";
    }

    void SearchItem()
    {
        // Handle item interaction (same as before)
        Debug.Log("Searching the item: " + npcInRange.name);
        Destroy(npcInRange); // For example, destroy item after searching
    }
}
