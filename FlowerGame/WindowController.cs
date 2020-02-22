using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController
{
    private bool open;
    private int season;

    public WindowController(int startSeason)
    {
        open = false;
        season = startSeason;
    }

    public bool IsOpen() { return this.open; }
    public void SetOpen(bool open) { this.open = open; }
}
