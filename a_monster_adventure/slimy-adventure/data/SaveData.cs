using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public string CurrentSceneName { get; set; }
    // public float ZoomX { get; set; }
    // public float ZoomY { get; set; }
    // Store the player's last known position so they don't always reset to the instantiator's default
    public float PlayerPosX { get; set; }
    public float PlayerPosY { get; set; }
    
    public List<AllySaveEntry> Allies { get; set; } = new List<AllySaveEntry>();
}

[Serializable]
public class AllySaveEntry
{
    public int Id { get; set; }
    public bool IsFollowing { get; set; }
    public bool IsImprisoned { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public string SceneName { get; set; }
}