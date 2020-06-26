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
    public int adjacencyWeightCutoff;
    public bool debugGreyscale;

    //if a grid is occupied in x, y position
    private int[,] adjacencyWeight;
    private int[,] occupationStatus;
    private Random random;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        random = new Random();
        occupationStatus = new int[generationAreaX,generationAreaY];
        adjacencyWeight = new int[generationAreaX, generationAreaY];

        //generate floor
        for (int x = 0; x < generationAreaX; x++)
        {
            AttemptInstantiate2(x, 0);
        }
        for (int y = 1; y < generationAreaY; y++)
        {
            for(int x = 0; x<generationAreaX; x++)
            {
                AttemptInstantiate2(x, y);
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

    //heavy/higher number is white
    private void CalculateWeight(int x, int y)
    {
        if (y == 0)
        {
            if(x == 0)
                adjacencyWeight[x, y] = Random.Range(1, adjacencyWeightCutoff+1);
            else
            {
                if (Random.Range(0, 2) == 0)
                    adjacencyWeight[x, y] = adjacencyWeight[x-1, y] + 1;
                else
                    adjacencyWeight[x, y] = adjacencyWeight[x-1, y] - 1;
            }
        }
        else
        {
            if (Random.Range(0, 2) == 0)
                adjacencyWeight[x, y] = AverageAdjWeight(x,y) + 1;
            else
                adjacencyWeight[x, y] = AverageAdjWeight(x,y) - 1;
        }

        if (adjacencyWeight[x, y] > 10)
            adjacencyWeight[x, y] = 9;
        else if (adjacencyWeight[x, y] <= 0)
            adjacencyWeight[x, y] = 2;
    }

    private int AverageAdjWeight(int x, int y)
    {
        if (x == 0)
            return adjacencyWeight[x, y - 1];
        else
            return ((adjacencyWeight[x, y - 1] + adjacencyWeight[x-1, y])/2);
    }

    private void CheckAdjacency(int x, int y)
    {
        if(x == 0)
        {
            if(y == 0)
            {
                if (occupationStatus[x + 1, y] != 0 || occupationStatus[x, y + 1] != 0)
                {
                    AttemptInstantiate(x, y, 7);

                }

            }
            else if(y == generationAreaY-1)
            {
                if (occupationStatus[x + 1, y] != 0 || occupationStatus[x, y - 1] != 0)
                {
                        AttemptInstantiate(x, y, 7);
                }

            }
            else
            {
                if (occupationStatus[x + 1, y] != 0 || occupationStatus[x, y + 1] != 0 || occupationStatus[x, y - 1] != 0)
                {
                        AttemptInstantiate(x, y, 7);
                }
            }
        }
        else if(x == generationAreaX-1)
        {
            if (y == 0)
            {
                if (occupationStatus[x - 1, y] != 0 || occupationStatus[x, y + 1] != 0)
                {
                        AttemptInstantiate(x, y, 7);
                }

            }
            else if (y == generationAreaY - 1)
            {
                if (occupationStatus[x - 1, y] != 0 || occupationStatus[x, y - 1] != 0)
                {
                        AttemptInstantiate(x, y, 7);
                }
            }
            else
            {
                if (occupationStatus[x - 1, y] != 0 || occupationStatus[x, y + 1] != 0 || occupationStatus[x, y - 1] != 0)
                {
                        AttemptInstantiate(x, y, 7);
                }
            }
        }
        else if(y == 0)
        {
            if (occupationStatus[x + 1, y] != 0 || occupationStatus[x - 1, y] != 0 || occupationStatus[x, y + 1] != 0)
            {
                    AttemptInstantiate(x, y, 7);
            }
        }
        else if(y == generationAreaY-1)
        {
            if (occupationStatus[x + 1, y] != 0 || occupationStatus[x - 1, y] != 0 || occupationStatus[x, y - 1] != 0)
            {
                    AttemptInstantiate(x, y, 7);
            }
        }
        else
        {
            if (occupationStatus[x + 1, y] != 0 || occupationStatus[x - 1, y] != 0 || occupationStatus[x, y + 1] != 0 || occupationStatus[x, y - 1] != 0)
            {
                    AttemptInstantiate(x, y, 7);
            }
        }
    }

    private void AttemptInstantiate(int x, int y, int chanceOutOfTen)
    {
        if (flip == false)
        {
            if (Random.Range(0, 11) <= chanceOutOfTen)
            {
                Instantiate(tile, new Vector2(x * blockSize + rb.position.x, y * blockSize + rb.position.y), Quaternion.identity);
                occupationStatus[x, y] = 1;
            }
        }
        else
        {
            if (Random.Range(0, 11) <= chanceOutOfTen)
            {
                Instantiate(tile, new Vector2((generationAreaX-x) * blockSize + rb.position.x, (y) * blockSize + rb.position.y), Quaternion.identity);
                occupationStatus[x, y] = 1;
            }
        }
       
    }
    private void AttemptInstantiate2(int x, int y)
    {
        CalculateWeight(x, y);
        if (flip == false)
        {
            if (adjacencyWeight[x, y] <= adjacencyWeightCutoff)
            {
                GameObject created = Instantiate(tile, new Vector2(x * blockSize + rb.position.x, y * blockSize + rb.position.y), Quaternion.identity);
                if(debugGreyscale)
                    created.GetComponent<SpriteRenderer>().color = new Color(((float)adjacencyWeight[x, y]) / 10f, ((float)adjacencyWeight[x, y]) / 10f, ((float)adjacencyWeight[x, y]) / 10f);
                occupationStatus[x, y] = 1;
            }

            /*
            if(debugGreyscale)
            {
                GameObject created = Instantiate(tile, new Vector2(x * blockSize + rb.position.x, y * blockSize + rb.position.y), Quaternion.identity);
                created.GetComponent<SpriteRenderer>().color = new Color(((float)adjacencyWeight[x, y]) / 10f, ((float)adjacencyWeight[x, y]) / 10f, ((float)adjacencyWeight[x, y]) / 10f);
                occupationStatus[x, y] = 1;
            }
            else if (adjacencyWeight[x,y] <= adjacencyWeightCutoff)
            {
                Instantiate(tile, new Vector2(x * blockSize + rb.position.x, y * blockSize + rb.position.y), Quaternion.identity);
                occupationStatus[x, y] = 1;
            }*/
        }
        else
        {
            if (adjacencyWeight[x, y] <= adjacencyWeightCutoff)
            {
                Instantiate(tile, new Vector2((generationAreaX - x) * blockSize + rb.position.x, (y) * blockSize + rb.position.y), Quaternion.identity);
                occupationStatus[x, y] = 1;
            }
        }

    }
}

