using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawnerRanges : MonoBehaviour
{
    [SerializeField] MobSpawner[] spawners;
    [SerializeField] RangeFloat spawn_rate;
    [SerializeField] RangeInt spawn_xpos_ranges;
    [SerializeField] float y;


    private void Start()
    {
        YCTimer.Add(new YCTimerData(Random.Range(spawn_rate.min, spawn_rate.max), start_spawn)); 
    }

    void start_spawn()
    {
        foreach(var i in spawners)
        {
            i.Spawn(new Vector2(Random.Range(spawn_xpos_ranges.min, spawn_xpos_ranges.max), y));
        }
        YCTimer.Add(new YCTimerData(Random.Range(spawn_rate.min, spawn_rate.max), start_spawn));
    }
}
