using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawnManager : MonoBehaviour
{
    public float spawnDensity = 1.0f;
    public float monsterSpawnDistance = 10;
    public List<Monster> monstersToSpawn;

    private List<EnemySpawn> unitSpawnCache = new List<EnemySpawn>();

    private class EnemySpawn
    {
        public EnemySpawn(Vector3 position, Unit monster, bool isEmpowered = false)
        {
            this.position = position;
            this.monster = monster;
            this.isEmpowered = isEmpowered;
        }
        public Vector3 position;
        public Unit monster;
        public bool isEmpowered;
    }

    private void Start()
    {
        GenerateSpawns();
    }

    private void Update()
    {
        TrySpawnEnemies();
    }
    
    /// <summary>
    /// Calculates which monster to spawn after considering validity and rarity for each monster.
    /// </summary>
    private Monster GetMonsterToSpawn(int totalSpawnLiklihood, int maximumRoomLevel)
    {
        bool validMonster = false;
        for (int i = 0; i < monstersToSpawn.Count; i++)
        {
            if (monstersToSpawn[i].spawnLevel <= maximumRoomLevel)
            {
                validMonster = true;
                break;
            }
        }
        if (!validMonster)
        {
            Debug.Log("There are no valid monsters to spawn with the minimum spawn level needed for a room.");
            return null;
        }

        int remainingSpawns = monstersToSpawn.Count;
        Monster monsterToSpawn = null;
        while (monsterToSpawn == null)
        {
            int value = Random.Range(0, totalSpawnLiklihood);
            for (int i = 0; i < monstersToSpawn.Count; i++)
            {
                if (value <= monstersToSpawn[i].spawnLikelihood)
                {
                    if (monstersToSpawn[i].spawnLevel <= maximumRoomLevel)
                        monsterToSpawn = monstersToSpawn[i];
                    break;
                }
                else
                    value -= monstersToSpawn[i].spawnLikelihood;
            }
        }
        return monsterToSpawn;
    }

    public void GenerateSpawns() 
    {
        int totalMonsterLikelihood = 0;
        for (int i = 0; i < monstersToSpawn.Count; i++)
        {
            totalMonsterLikelihood += monstersToSpawn[i].spawnLikelihood;
        }

        MonsterSpawnArea[] spawnLocations = GameObject.FindObjectsByType<MonsterSpawnArea>(FindObjectsSortMode.None); 

        float modifier = 0.04f; // Normalises this so that a spawn density of 1 would spawn 1 monster in a 5x5 area (i.e., 1 / (5*5) = 0.04).
        float total = 0;
        foreach (MonsterSpawnArea trigger in spawnLocations)
        {
            total += trigger.transform.localScale.x * trigger.transform.localScale.z * trigger.spawnDensity;
        }

        foreach (MonsterSpawnArea trigger in spawnLocations)
        {
            int toSpawn = (int)(trigger.GetSpawnAreaSize() * spawnDensity * modifier * trigger.spawnDensity);
            for (int i = 0; i < toSpawn; i++)
            {
                bool isEmpowered = trigger.empoweredMonsters > 0;
                trigger.empoweredMonsters--;
                unitSpawnCache.Add(new EnemySpawn(Utilities.GetValidNavMeshPosition(trigger.GetRandomVectorInCollider()),
                    GetMonsterToSpawn(totalMonsterLikelihood, trigger.maximumSpawnLevel), isEmpowered));
            }
        }
    }

    /// <summary>
    /// Handles the spawning of monsters as the player gets close enough to them.
    /// </summary>
    private void TrySpawnEnemies()
    {
        for (int i = 0; i < unitSpawnCache.Count; i++)
        {
            if (Vector3.Distance(GameManager.player.transform.position, unitSpawnCache[i].position) < monsterSpawnDistance)
            {
                SpawnMonster((Monster)unitSpawnCache[i].monster, unitSpawnCache[i].position, Unit.Faction.Enemy, unitSpawnCache[i].isEmpowered, true);
                unitSpawnCache.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// This method is used to immediately spawn a monster. This can be used to bypass the monster's spawn effect, or used
    /// after the effect has played to spawn in the monster.
    /// </summary>
    private void SpawnMonsterImmediate (Monster monsterPrefab, Vector3 position, Unit.Faction faction, bool isEmpowered)
    {
        Monster monster = Instantiate(monsterPrefab, position, Quaternion.identity);
        if (isEmpowered) monster.Empower();
        monster.SetFaction(faction);
    }

    /// <summary>
    /// This method is used to spawn the monster using its spawn effect. 
    /// </summary>
    private IEnumerator SpawnMonsterWithEffect (Monster monsterPrefab, Vector3 position, Unit.Faction faction, bool isEmpowered)
    {
        if (monsterPrefab.spawnEffect != null)
        {
            float duration = monsterPrefab.spawnEffect.effectDuration;
            ObjectPooler.InstantiatePooled(monsterPrefab.spawnEffect.gameObject, position, Quaternion.identity); 
            yield return new WaitForSeconds(duration);
        }
        SpawnMonsterImmediate(monsterPrefab, position, faction,isEmpowered);
    }

    /// <summary>
    /// This is the main method which should be used to spawn monsters in the game.
    /// </summary>
    public void SpawnMonster (Monster monster, Vector3 position, Unit.Faction faction, bool isEmpowered = false, bool useSpawnEffect = true)
    {
        position = Utilities.GetValidNavMeshPosition(position);
        if (monster == null)
        {
            Debug.LogError("The unit you tried to spawn was null.");
            return;
        }
        if (!useSpawnEffect || monster.spawnEffect == null)
            SpawnMonsterImmediate(monster, position, faction, isEmpowered);
        else
            StartCoroutine(SpawnMonsterWithEffect(monster, position, faction, isEmpowered));
    }
}
