public interface IAnomaly 
{
    AnomalyList AnomalyName { get;}
    AreaNames AreaName { get; }
    bool IsDiscoverd { get; }
    int AppearanceCount { get;  }
    void SetData(AnomalyData data);
    public void SetAnomalyName();
    public void SetAnomaly();

    public void ResetAnomaly();
}


