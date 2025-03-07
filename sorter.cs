using UnityEngine;
using UnityEngine.EventSystems;

public class Sorter : MonoBehaviour
{
    public GameObject[] prefabs; 
    public GameObject aim; 
    public GameObject leftBoundObject; 
    public GameObject rightBoundObject; 

    public float dragSensitivity = 1.5f;
    public float spawnDelay = 0.5f;

    private bool isDragging = false; 
    private bool isSpawning = false; 
    private bool isFirstObject = true;

    private GameObject currentObject; 
    private Camera mainCamera; 

    private float minX, maxX; 
    private float spawnTimer = 0f;

    private void Start()
    {
        aim.SetActive(false); 
        mainCamera = Camera.main; 
        Randomizer();
    }

    private void Update()
    {
        if (currentObject == null && !isSpawning) 
        {
            Randomizer(); 
            return;
        }

        if (!isFirstObject && spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                SpawnNewObject();
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!isDragging)
                    {
                        float newX = Mathf.Clamp(touchPosition.x, minX, maxX);
                        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
                    }
                    isDragging = true;
                    aim.SetActive(true);
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        float newX = Mathf.Clamp(touchPosition.x, minX, maxX);
                        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
                    }
                    break;

                case TouchPhase.Ended:
                    aim.SetActive(false);
                    isDragging = false;
                    Randomizer();
                    break;

                case TouchPhase.Canceled:
                    aim.SetActive(false);
                    isDragging = false;
                    break;
            }
        }
    }

    private void Randomizer()
    {
        if (isSpawning) return;

        isSpawning = true;

        if (currentObject != null)
        {
            currentObject.transform.parent = null;
            currentObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        if (!isFirstObject)
        {
            spawnTimer = spawnDelay;
        } 
        else 
        {
            SpawnNewObject();
            SetScreenBounds();
            isFirstObject = false;
        }
    }

    private void SpawnNewObject()
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        currentObject = Instantiate(prefabs[randomIndex], new Vector3(transform.position.x, transform.position.y - 0.7f, transform.position.z), Quaternion.identity);
        currentObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        currentObject.transform.parent = transform;
        isSpawning = false;
    }

    private void SetScreenBounds()
    {
        if (currentObject == null) return;

        float objectWidth = currentObject.GetComponent<SpriteRenderer>().bounds.extents.x;

        // Define os limites com base nos objetos de limite
        minX = leftBoundObject.transform.position.x + objectWidth;
        maxX = rightBoundObject.transform.position.x - objectWidth;
    }
}
