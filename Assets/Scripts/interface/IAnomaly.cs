public interface IAnomaly 
{
    AnomalyList AnomalyName { get;}
    AreaNames AreaName { get; }
    bool IsDiscoverd { get; }
    int AppearanceCount { get; }
    void SetSavedData(AnomalyData data);
    public void UpdateAppearanceCount();
    public void UpdateDiscover();
    public void SetAnomalyName();
    public void SetAnomaly();

    public void ResetAnomaly();
}


