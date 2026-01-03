using UnityEngine;

using UnityEngine;

public class ConveyorWidget : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 3.5f;

    public Sprite unprocessedSprite;
    public Sprite processedSprite;

    public GameObject widget;

    public Movement player;

    public ConveyorSpawner conveyorSpawner;

    Vector3 startPosition;
    bool isMoving = true;
    bool isProcessed = false;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        conveyorSpawner = FindAnyObjectByType<ConveyorSpawner>();
        spriteRenderer.sprite = unprocessedSprite;
        player = FindAnyObjectByType<Movement>();

        startPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            MoveForward();
        }
    }

    void MoveForward()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (!isProcessed && Vector3.Distance(startPosition, transform.position) >= moveDistance)
        {
            isMoving = false;
        }

        if (Vector3.Distance(startPosition, transform.position) >= moveDistance * 2)
        {
            conveyorSpawner.SpawnWidget();
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        if (!isProcessed && !isMoving && player.currentNode.name == "ConveyorMoveNode")
        {
            ProcessItem();
        }
    }

    void ProcessItem()
    {
        isProcessed = true;
        spriteRenderer.sprite = processedSprite;
        isMoving = true;
    }
}

