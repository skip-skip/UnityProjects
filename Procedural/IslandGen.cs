using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGen : MonoBehaviour
{
    public GameObject tile;
    public int tileSize;
    private Rigidbody2D rb;


    public int areaX;
    public int areaY;
    public int numSubpeaks;
    public int minPeakHeight;
    public int minTerracePoints;
    public int maxTerracePoints;
    public int terraceDeltaDistance;

    private int[,] occupationStatus;
    private Vector2 peak;
    private Vector2[] joints;
    private int[,] terraceCount;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        occupationStatus = new int[areaX, areaY];
        joints = new Vector2[numSubpeaks+3];
        terraceCount = new int[2, numSubpeaks + 3]; //peak and subpeaks can terrace

        FindJoints();
        SortJoints();
        FindTerraceCounts();
        InterpolateJoints();
        foreach (Vector2 v in joints)
            Debug.Log(v);
        DrawOccupied();
    }

    private void FindJoints()
    {
        //find peak
        int randX = Random.Range(0, areaX);
        int randY = Random.Range(0 + (minPeakHeight - 1), areaY);
        peak = new Vector2(randX, randY);
        occupationStatus[randX, randY] = 1;
        
        //all other joints must be below peak
        for (int i=0; i < numSubpeaks; i++)
        {
            randX = Random.Range(0, areaX);
            bool duplicate = true;
            while (duplicate)
            {
                duplicate = false;
                randY = Random.Range(1, (int)peak.y);
                for(int d = 0; d<i; d++)
                {
                    if (randY == joints[d].y)
                        duplicate = true;
                }
            }

            joints[i] = new Vector2(randX, randY);
            occupationStatus[randX, randY] = 1;
        }

        joints[numSubpeaks] = peak;
        joints[numSubpeaks + 1] = new Vector2(0, 0);
        joints[numSubpeaks + 2] = new Vector2(areaX-1, 0);
        occupationStatus[0, 0] = 1;
        occupationStatus[areaX-1, 0] = 1;    
    }

    /*Sort Joints by x values (insertion)*/
    private void SortJoints()
    {
        int current;
        for(int i = 0; i < numSubpeaks+3; i++)
        {
            current = i;
            while(current != 0 && joints[current].x < joints[current-1].x)
            {
                Vector2 hold = joints[current - 1];
                joints[current - 1] = joints[current];
                joints[current] = hold;
                current--;
            }
        }
    }

    private void InterpolateJoints()
    {
        Vector2 primary, secondary;
        //step through joints
        for(int j = 0; j < numSubpeaks+2; j++)
        {
            primary = joints[j];
            secondary = joints[j + 1];
            if (secondary.x - primary.x != 0)
                SubpointInterpolate(primary, secondary);
            else
                OneDimInterpolate(primary, secondary);
        }
    }

    private void DrawOccupied()
    {
        for(int x = 0; x<areaX; x++)
        {
            for(int y = 0; y < areaY; y++)
            {
                if(occupationStatus[x,y] != 0)
                    Instantiate(tile, new Vector2(x * tileSize + rb.position.x, y * tileSize + rb.position.y), Quaternion.identity);
            }
        }
    }

    private void SubpointInterpolate(Vector2 primary, Vector2 secondary)
    {
        float slope = (secondary.y - primary.y) / (secondary.x - primary.x);

        //create subpoints
        int xPos, yPos;
        int prevYPos = (int)primary.y;
        for (int dx = 1; dx <= secondary.x - primary.x; dx++)
        {
            xPos = (int)(primary.x + dx);
            yPos = (int)(primary.y + (dx * slope));
            occupationStatus[xPos, yPos] = 1;
            //fill gaps
            if (yPos > primary.y)    //going up
            {
                for (int y = prevYPos + 1; y <= yPos; y++)
                    occupationStatus[xPos, y] = 1;
            }
            else                    //going down
            {
                for (int y = prevYPos - 1; y >= yPos; y--)
                    occupationStatus[xPos, y] = 1;
            }
            prevYPos = yPos;
        }
    }
    private void OneDimInterpolate(Vector2 primary, Vector2 secondary)
    {
        if (primary.y < secondary.y)    //going up
        {
            for (int y = (int)primary.y + 1; y <= secondary.y; y++)
                occupationStatus[(int)primary.x, y] = 1;
        }
        else                    //going down
        {
            for (int y = (int)primary.y - 1; y >= secondary.y; y--)
                occupationStatus[(int)primary.x, y] = 1;
        }
    }

    private void FindTerraceCounts()
    {
        //skip corners
        for(int i = 1; i < numSubpeaks + 2; i++)
        {
            int usedTerracePoints = Random.Random(0,);
            terraceCount[i, 0] = 
        }
    }

    /* Generation Parameters */
    // *Integers
    // number of peaks

    // *Percentages (float 0-1)
    // roughness
    // deviation/overhangs




}
