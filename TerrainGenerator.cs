using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private float blockSize;
    [SerializeField] private int generationAreaX;
    [SerializeField] private int generationAreaY;
    public GameObject tile;
    public bool flip;

    //if a grid is occupied in x, y position
    private int[,] occupationStatus;
    private Random random;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        random = new Random();
        occupationStatus = new int[generationAreaX,generationAreaY];

        for (int x = 0; x < generationAreaX; x++)
        {
            Instantiate(tile, new Vector2(x * blockSize, 0), Quaternion.identity);
            occupationStatus[x, 0] = 1;
        }
        for (int x = 1; x < generationAreaX; x++)
        {
            for(int y = 0; y<generationAreaY; y++)
            {
                CheckAdjacency(x, y);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckAdjacency(int x, int y)
    {
        if(x == 0)
        {
            if(y == 0)
            {
                if (occupationStatus[x + 1, y] != 0 || occupationStatus[x, y + 1] != 0)
                {
                    AttemptInstantiate(x, y);

                }

            }
            else if(y == generationAreaY-1)
            {
                if (occupationStatus[x + 1, y] != 0 || occupationStatus[x, y - 1] != 0)
                {
                        AttemptInstantiate(x, y);
                }

            }
            else
            {
                if (occupationStatus[x + 1, y] != 0 || occupationStatus[x, y + 1] != 0 || occupationStatus[x, y - 1] != 0)
                {
                        AttemptInstantiate(x, y);
                }
            }
        }
        else if(x == generationAreaX-1)
        {
            if (y == 0)
            {
                if (occupationStatus[x - 1, y] != 0 || occupationStatus[x, y + 1] != 0)
                {
                        AttemptInstantiate(x, y);
                }

            }
            else if (y == generationAreaY - 1)
            {
                if (occupationStatus[x - 1, y] != 0 || occupationStatus[x, y - 1] != 0)
                {
                        AttemptInstantiate(x, y);
                }
            }
            else
            {
                if (occupationStatus[x - 1, y] != 0 || occupationStatus[x, y + 1] != 0 || occupationStatus[x, y - 1] != 0)
                {
                        AttemptInstantiate(x, y);
                }
            }
        }
        else if(y == 0)
        {
            if (occupationStatus[x + 1, y] != 0 || occupationStatus[x - 1, y] != 0 || occupationStatus[x, y + 1] != 0)
            {
                    AttemptInstantiate(x, y);
            }
        }
        else if(y == generationAreaY-1)
        {
            if (occupationStatus[x + 1, y] != 0 || occupationStatus[x - 1, y] != 0 || occupationStatus[x, y - 1] != 0)
            {
                    AttemptInstantiate(x, y);
            }
        }
        else
        {
            if (occupationStatus[x + 1, y] != 0 || occupationStatus[x - 1, y] != 0 || occupationStatus[x, y + 1] != 0 || occupationStatus[x, y - 1] != 0)
            {
                    AttemptInstantiate(x, y);
            }
        }
    }

    private void AttemptInstantiate(int x, int y)
    {
        if (flip == false)
        {
            if (Random.Range(0, 11) <= 7)
            {
                Instantiate(tile, new Vector2(x * blockSize + rb.position.x, y * blockSize + rb.position.y), Quaternion.identity);
                occupationStatus[x, y] = 1;
            }
        }
        else
        {
            if (Random.Range(0, 11) <= 7)
            {
                Instantiate(tile, new Vector2((generationAreaX-x) * blockSize + rb.position.x, (y) * blockSize + rb.position.y), Quaternion.identity);
                occupationStatus[x, y] = 1;
            }
        }
       
    }
}
