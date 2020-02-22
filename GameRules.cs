using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    public int maxExposure;
    public int maxWater;
    public float updateRate;


    private FlowerController flower;
    private WindowController window;

    private IEnumerator flowerRoutine;

    // Start is called before the first frame update
    void Start()
    {
        flower = new FlowerController(maxExposure, maxWater);
        window = new WindowController(0);

        flowerRoutine = UpdateStats(updateRate);
        StartCoroutine(flowerRoutine);
    }

    // Update is called once per frame
    void Update()
    {
        CheckFlowerStatus();
        CheckInput();

    }

    private IEnumerator UpdateStats(float rate)
    {
        while (true)
        {
            if(!flower.IsDead())
            {
                if (flower.GetWater() > 0)
                    flower.SetWater(flower.GetWater() - 1);
                if (flower.GetExposure() < maxExposure)
                    flower.SetExposure(flower.GetExposure() + 1);
                flower.IncrementLifetime();
            }
            
            yield return new WaitForSeconds(rate);
        }
    }

    private void CheckFlowerStatus()
    {
        if (flower.GetWater() == 0)
            flower.Kill();
        if (flower.GetExposure() == maxExposure | flower.GetExposure() == 0)
            flower.Kill();
    }

    private void CheckInput()
    {
        if (!flower.IsPlanted() & Input.GetButtonDown("plant"))
        {
            flower.PlantNewFlower();
        }
        if (flower.IsPlanted() & Input.GetButtonDown("remove"))
        {
            flower.RemovePlanted();
        }

    }

    
}
