using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    [SerializeField] private float blockSize;
    [SerializeField] private int generationAreaX;
    [SerializeField] private int generationAreaY;
    public GameObject tile;
    public bool flip;
    public int adjacencyWeightCutoff;
    public bool debugGreyscale;

    public int heightChangeChance;
    public float directionChangePercentage;
    public int hillynessLevel;
    private int prevY;
    private int increasingHeight;

    //if a grid is occupied in x, y position
    private int[,] adjacencyWeight;
    private int[,] occupationStatus;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        occupationStatus = new int[generationAreaX, generationAreaY];
        adjacencyWeight = new int[generationAreaX, generationAreaY];

        //generate floor
        GenerateGround();

    }

    private void GenerateGround()
    {
        prevY = 0;
        increasingHeight = 1;

        for(int x =0; x<generationAreaX; x++)
        {
            DrawFromPrevious(x);
        }
    }

    private void DrawFromPrevious(int x)
    {
        if(Random.Range(0, 1+heightChangeChance) == 0)
        {
            GenerateBlock(x, prevY);
        }
        else
        {
            if(Random.Range(0f, 1f) <= directionChangePercentage)
            {
                increasingHeight *= -1;
            }

            for(int i = 0; i<hillynessLevel; i++)
            {
                GenerateBlock(x, prevY + 1 * increasingHeight);
                prevY = prevY + 1 * increasingHeight;
            }
        }
    }

    private void GenerateBlock(int x, int y)
    {
        Instantiate(tile, new Vector2(x * blockSize + rb.position.x, y * blockSize + rb.position.y), Quaternion.identity);
    }

    //heavy/higher number is white
    private void CalculateWeight(int x, int y)
    {
        if (y == 0)
        {
            if (x == 0)
                adjacencyWeight[x, y] = Random.Range(1, adjacencyWeightCutoff + 1);
            else
            {
                if (Random.Range(0, 2) == 0)
                    adjacencyWeight[x, y] = adjacencyWeight[x - 1, y] + 1;
                else
                    adjacencyWeight[x, y] = adjacencyWeight[x - 1, y] - 1;
            }
        }
        else
        {
            if (Random.Range(0, 2) == 0)
                adjacencyWeight[x, y] = AverageAdjWeight(x, y) + 1;
            else
                adjacencyWeight[x, y] = AverageAdjWeight(x, y) - 1;
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
            return ((adjacencyWeight[x, y - 1] + adjacencyWeight[x - 1, y]) / 2);
    }
}
