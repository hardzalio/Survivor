using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "SO/Enemy/WaveData", order = 0)]
public class WaveData : ScriptableObject {
    public float strength = 5f;
    public List<EnemyData> enemies = new();
    public float multiplier = 1f;
}
