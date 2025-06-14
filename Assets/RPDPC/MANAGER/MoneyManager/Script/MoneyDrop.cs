using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    [SerializeField] public List<MinMaxMoney> dropRate = new List<MinMaxMoney>();

    private const float SPAWN_FACTOR = 0.5f;
    
    public void DropMonney()
    {
        foreach(MinMaxMoney money in dropRate)
        {
            HandleDropMonney(this.transform.position, money);
        }
    }
    private void HandleDropMonney(Vector3 spawn, MinMaxMoney coin)
    {
        int numberSpawn = UnityEngine.Random.Range(coin.min, coin.max + 1);

        for(int i = 0; i < numberSpawn; i++)
        {
            Vector3 trueSpawn = new Vector3(spawn.x, spawn.y + 0.1f, spawn.z);
            var c = Instantiate(coin.coinSO.coin.gameObject, trueSpawn, Quaternion.Euler(-90f, 0, 0));
        }
    }
}
