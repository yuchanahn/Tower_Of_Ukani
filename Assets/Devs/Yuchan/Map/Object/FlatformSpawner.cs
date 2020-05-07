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

    [SerializeField] RangeFloat height_offset;


    Vector2 calc_spawn_pos(ref int[] chance_of, float y)
    {
        var r = Random.Range(0f, chance_of.Length);
        var l = (chance_of.Length - 1);
        r += (chance_of[Mathf.Min(Mathf.RoundToInt(r), l)] * 10 / l) * (ARandom.Get(50) ? -1 : 1);
        chance_of[Mathf.Min(Mathf.RoundToInt(r), l)]++;
        r = r < 0 ? chance_of.Length - Mathf.Abs(r) : r;
        r %= chance_of.Length;

        return new Vector2(r, y);
    }





    public void run()
    {
        int[] chance = new int[Mathf.FloorToInt(overlap_size.x)];
        for (float i = 0; i < overlap_size.y; i+= Random.Range(height_offset.min, height_offset.max))
        {
            var p = calc_spawn_pos(ref chance, i).Add(leftbottom.x, leftbottom.y);

            var x = Random.Range(block_size_x.min, block_size_x.max);
            var y = Random.Range(block_size_y.min, block_size_y.max);
            for (int ii = 0; ii < x; ii++)
            {
                for(int j = 0; j < y; j++)
                {
                    if (ii <= j - 1 || ii >= x - j) continue;
                    Instantiate(prefab, p.Add(ii, -j), Quaternion.identity);
                }
            }
        }
    }
}