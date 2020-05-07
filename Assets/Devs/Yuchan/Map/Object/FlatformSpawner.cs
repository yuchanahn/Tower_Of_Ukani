using Dongjun.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlatformSpawner : MonoBehaviour
{
    [SerializeField] Vector2 leftbottom;
    [SerializeField] Vector2 overlap_size;
    [SerializeField] GameObject prefab;

    [SerializeField] RangeInt block_size_x;
    [SerializeField] RangeInt block_size_y;

    [SerializeField] RangeInt height_offset;
    [SerializeField] int next_pos;

    [SerializeField] Transform map_tr;

    Vector2 calc_spawn_pos(int[] chance_of, float y)
    {
        var r = Random.Range(0, chance_of.Length);
        var t = r + chance_of[r] * (ARandom.Get(50) ? -1 : 1);
        t = t < 0 ? chance_of.Length - Mathf.Abs(t) : t;
        t %= chance_of.Length;
        return new Vector2(r, y);
    }


    public void run()
    {
        int[] chance = new int[Mathf.FloorToInt(overlap_size.x)];
        for (float i = 0; i < overlap_size.y; i += Random.Range(height_offset.min, height_offset.max))
        {
            var p = calc_spawn_pos(chance, i).Add(leftbottom.x, leftbottom.y);
            chance = new int[Mathf.FloorToInt(overlap_size.x)];
            chance[(int)(p.x - leftbottom.x)] += next_pos;
            
            var x = Random.Range(block_size_x.min, block_size_x.max);
            var y = Random.Range(block_size_y.min, block_size_y.max);
            for (int ii = 0; ii < x; ii++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (ii <= j - 1 || ii >= x - j) continue;
                    Instantiate(prefab, p.Add(ii, -j), Quaternion.identity).transform.SetParent(map_tr);
                }
            }
        }
    }
}