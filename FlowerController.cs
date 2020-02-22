using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController
{

    private int flowersGrown;
    private int waterLevel;
    private int exposureLevel;
    private int flowerLifeTime;
    private bool dead;
    private bool planted;

    private readonly int maxExposure;
    private readonly int maxWater;

    public FlowerController(int maxExposure, int maxWater)
    {
        flowersGrown = 0;
        waterLevel = 0;
        exposureLevel = 0;
        flowerLifeTime = 0;
        dead = true;
        planted = false;

        this.maxExposure = maxExposure;
        this.maxWater = maxWater;
    }

    public void PlantNewFlower()
    {
        planted = true;
        dead = false;
        flowerLifeTime = 0;
        waterLevel = maxWater / 4;
        exposureLevel = maxExposure / 2;
        flowersGrown++;
    }

    public void Kill()
    {
        dead = true;
    }

    public void RemovePlanted()
    {
        if (!planted)
            throw new System.ArgumentException("flower is not planted");
        else
        {
            this.Kill();
            SetPlanted(false);
        }
    }

    public int GetWater() { return this.waterLevel; }
    public void SetWater(int level) { waterLevel = level; }
    public int GetExposure() { return this.exposureLevel; }
    public void SetExposure(int level) { exposureLevel = level; }
    public int GetLifetime() { return this.flowerLifeTime; }
    public void IncrementLifetime() { this.flowerLifeTime++; }
    public bool IsDead() { return this.dead; }
    public bool IsPlanted() { return this.planted; }
    public void SetPlanted(bool planted) { this.planted = planted; }
}
