using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "panel Collection", menuName = "Create Panel Collection")]
public class PanelsCollection : ScriptableObject
{
    [SerializeField] List<PanelsData> panels;

    public List<PanelsData> Panels { get => panels; set => panels = value; }
}
