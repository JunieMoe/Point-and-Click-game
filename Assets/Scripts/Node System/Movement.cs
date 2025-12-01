using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public GameObject[] nodes;
    public GameObject currentNode;
    public float maxClickDistance = 1f;
    public float maxItemClickDistance = 0.5f;
    public float defaultSpeed = 15f;

    private Transform targetNode;
    private bool isMoving = false;
    private Queue<Transform> movementQueue = new Queue<Transform>();

    private Item pendingPickupItem = null;
    private TapInteraction pendingInteraction = null;

    public Inventory inventory;
    public InventoryUI inventoryUI;

    public List<ItemData> allItems;

    public GameManager gameManager;

    private Camera mainCamera;
    private int nodeLayer = 6;

    private Animator animator;
    private string currentTrigger = "";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentNode = GameObject.Find(gameManager.entranceNodeName);
        if (currentNode != null)
        {
            transform.position = currentNode.transform.position;
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        mainCamera = Camera.main;
        allItems = new List<ItemData>(Resources.LoadAll<ItemData>("Items"));
        GameManager.Instance.InitializeInventoryFromSave(allItems);
        inventory = GameManager.Instance.playerInventory;

        if (inventoryUI == null) inventoryUI = FindObjectOfType<InventoryUI>();

        inventoryUI.SetInventory(inventory);
        RefreshNodes();

        animator = GetComponent<Animator>();
    }

    private void RefreshNodes()
    {
        List<GameObject> nodeList = new List<GameObject>();
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            if (obj.layer == nodeLayer)
            {
                nodeList.Add(obj);
            }
        }

        nodes = nodeList.ToArray();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Item closestItem = FindClosestItem(mouseWorldPos);
            TapInteraction closestInteraction = FindClosestInteraction(mouseWorldPos);
            MoveNode closestNode = FindClosestMoveNode(mouseWorldPos);

            if (closestItem != null)
            {
                HandleItemClick(closestItem);
                return;
            }

            if (closestInteraction != null)
            {
                HandleInteractionClick(closestInteraction);
                return;
            }

            if (closestNode != null && closestNode != currentNode.GetComponent<MoveNode>())
            {
                HandleNodeClick(closestNode.gameObject);
            }
        }
    }

    private void HandleItemClick(Item item)
    {
        GameObject itemNode = item.itemNode?.gameObject;
        if (itemNode == null) return;

        if (!isMoving)
        {
            if (itemNode != currentNode)
            {
                SetNewPathToNode(itemNode);
                pendingPickupItem = item;
                pendingInteraction = null;
            }
            else
            {
                PickupItem(item);
            }
        }
        else
        {
            pendingPickupItem = item;
            pendingInteraction = null;
            SetNewPathToNode(itemNode);
        }
    }

    private void HandleInteractionClick(TapInteraction interaction)
    {
        GameObject interactionNode = interaction.interactionNode?.gameObject;
        if (interactionNode == null) return;

        if (!isMoving)
        {
            if (interactionNode != currentNode)
            {
                SetNewPathToNode(interactionNode);
                pendingInteraction = interaction;
                pendingPickupItem = null;
            }
            else
            {
                interaction.Interact();
            }
        }
        else
        {
            pendingInteraction = interaction;
            pendingPickupItem = null;
            SetNewPathToNode(interactionNode);
        }
    }

    private void HandleNodeClick(GameObject node)
    {
        if (!isMoving)
        {
            SetNewPathToNode(node);
        }
        else
        {
            if (node.transform == targetNode)
            {
                TruncatePathAtNode(node.transform);
            }
            else
            {
                GameObject pathStartNode = targetNode != null ? targetNode.gameObject : currentNode;
                List<Transform> path = FindPath(pathStartNode, node);

                if (path != null && path.Count > 0)
                {
                    movementQueue.Clear();
                    foreach (Transform t in path)
                        movementQueue.Enqueue(t);
                }
            }
        }
    }

    private void SetNewPathToNode(GameObject destinationNode)
    {
        List<Transform> path = FindPath(currentNode, destinationNode);
        if (path != null && path.Count > 0)
        {
            movementQueue.Clear();
            foreach (Transform t in path)
                movementQueue.Enqueue(t);

            if (!isMoving) MoveToNode(movementQueue.Dequeue());
        }
    }

    private void TruncatePathAtNode(Transform node)
    {
        if (targetNode == node) movementQueue.Clear();
        else
        {
            Queue<Transform> newQueue = new Queue<Transform>();
            newQueue.Enqueue(targetNode);

            foreach (Transform t in movementQueue)
            {
                newQueue.Enqueue(t);
                if (t == node)
                    break;
            }

            movementQueue = newQueue;
        }
    }

    private void FixedUpdate()
    {
        if (!isMoving && movementQueue.Count > 0) MoveToNode(movementQueue.Dequeue());

        if (isMoving && targetNode != null)
        {

            Vector3 direction = (targetNode.position - transform.position).normalized;

            Vector3 localScale = transform.localScale;

            if (direction.x < 0)
            {
                localScale.x = -Mathf.Abs(localScale.x);
            }
            else if (direction.x > 0)
            {
                localScale.x = Mathf.Abs(localScale.x);
            }

            transform.localScale = localScale;

            float currentSpeed = defaultSpeed;
            
            transform.position = Vector3.MoveTowards(transform.position, targetNode.position, currentSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetNode.position) < 0.01f)
            {
                transform.position = targetNode.position;
                isMoving = false;
                currentNode = targetNode.gameObject;
                gameManager.entranceNodeName = currentNode.name;

                if (currentTrigger.Length > 0)
                {
                    animator.ResetTrigger(currentTrigger);
                    currentTrigger = "";
                }

                if (movementQueue.Count == 0) animator.SetTrigger("Idle");

                if (pendingPickupItem != null && currentNode == pendingPickupItem.itemNode.gameObject)
                {
                    PickupItem(pendingPickupItem);
                    pendingPickupItem = null;
                }

                if (pendingInteraction != null && currentNode == pendingInteraction.interactionNode.gameObject)
                {
                    pendingInteraction.Interact();
                    pendingInteraction = null;
                }
            }
        }
    }

    public void MoveToNode(Transform nodePos)
    {
        MoveNode moveNode = currentNode.GetComponent<MoveNode>();

        string newTrigger = "Idle";

        if (moveNode != null)
        {
            foreach (var conn in moveNode.connections)
            {
                if (conn.targetNode == nodePos.gameObject)
                {
                    newTrigger = conn.animationTrigger;
                    break;
                }
            }
        }

        if (currentTrigger.Length > 0)
        {
            animator.ResetTrigger(currentTrigger);
        }

        animator.SetTrigger(newTrigger);
        currentTrigger = newTrigger;

        targetNode = nodePos;
        isMoving = true;
    }

    public List<Transform> FindPath(GameObject startNode, GameObject targetNode)
    {
        Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
        Queue<GameObject> frontier = new Queue<GameObject>();
        HashSet<GameObject> visited = new HashSet<GameObject>();

        frontier.Enqueue(startNode);
        visited.Add(startNode);

        while (frontier.Count > 0)
        {
            GameObject current = frontier.Dequeue();

            if (current == targetNode) break;

            MoveNode moveNode = current.GetComponent<MoveNode>();
            if (moveNode == null || moveNode.connections == null) continue;

            foreach (var conn in moveNode.connections)
            {
                GameObject neighbor = conn.targetNode;
                if (neighbor == null || visited.Contains(neighbor)) continue;

                visited.Add(neighbor);
                frontier.Enqueue(neighbor);
                cameFrom[neighbor] = current;
            }
        }

        if (!cameFrom.ContainsKey(targetNode) && startNode != targetNode)
            return null;

        List<Transform> path = new List<Transform>();
        GameObject currentStep = targetNode;

        while (currentStep != startNode)
        {
            if (!cameFrom.ContainsKey(currentStep)) return null;
            path.Insert(0, currentStep.transform);
            currentStep = cameFrom[currentStep];
        }

        return path;
    }

    public void PickupItem(Item item)
    {
        if (item == null) return;

        inventory.AddItem(item.itemData);
        inventoryUI.RefreshInventory();
        GameManager.Instance.SetInteractionState("Item_" + item.gameObject.name, "pickedUp");

        nodes = System.Array.FindAll(nodes, node => node != item.gameObject && node != null);

        Destroy(item.gameObject);
    }

    private MoveNode FindClosestMoveNode(Vector3 position)
    {
        return FindClosest<MoveNode>(position, maxClickDistance, currentNode);
    }

    private Item FindClosestItem(Vector3 position)
    {
        return FindClosest<Item>(position, maxItemClickDistance);
    }

    private TapInteraction FindClosestInteraction(Vector3 position)
    {
        return FindClosest<TapInteraction>(position, maxItemClickDistance);
    }

    private T FindClosest<T>(Vector3 position, float maxDistance, GameObject exclude = null) where T : MonoBehaviour
    {
        T[] objects = FindObjectsOfType<T>();
        T closest = null;
        float minDist = Mathf.Infinity;

        foreach (T obj in objects)
        {
            if (obj == null || obj.gameObject == exclude) continue;

            float dist = Vector2.Distance(position, obj.transform.position);
            if (dist <= maxDistance && dist < minDist)
            {
                minDist = dist;
                closest = obj;
            }
        }

        return closest;
    }
} 