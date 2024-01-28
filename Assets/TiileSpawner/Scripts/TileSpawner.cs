using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> m_ToSpawn;

    [SerializeField]
    protected GameObject m_SpawnTile;

    [SerializeField]
    protected GameObject m_SpawnerTile;

    [SerializeField]
    protected Vector2Int m_Size;

    [SerializeField]
    protected float m_TileSize;

    [SerializeField]
    protected float m_MinTime;

    [SerializeField]
    protected float m_MaxTime;

    [Range(0.0f, 1.0f)]
    public float boidsSpawnerChance = 0.1f;

    [Range(0.0f, 1.0f)]
    private float noTileChance = 0.2f;

    public void Spawn()
    {
        float maxDistance = new Vector3((m_Size.x-1) / 2 * m_TileSize * (1f / 1.15f), 0, (m_Size.y-1) / 2 * m_TileSize).magnitude;

        //Random.seed = 

        for (int x = -m_Size.x / 2; x <= m_Size.x/2; x++)
        {
            int howMany = m_Size.y - Mathf.Abs(x);

            for (int z = 0; z < howMany; z++)
            {
                bool isCenterTile = x == 0 && z == howMany / 2;

                if (!isCenterTile && Random.Range(0.0f, 1.0f) < noTileChance)
                {
                    continue;
                }

                float zOffset = Mathf.Abs((x % 2f) / 2f);

                Vector3 position = new Vector3(x * m_TileSize * (1f/1.15f), 0.0f, (z - (howMany - 1) / 2 - zOffset) * m_TileSize);
                Quaternion rot = Quaternion.Euler(0, 60 * Random.Range(0, 6), 0);

                GameObject newTileGameObject = null;

                if (isCenterTile)
                {
                    newTileGameObject = Instantiate(m_SpawnTile, position, rot, transform);
                }
                else
                {
                    if (Random.Range(0.0f, 1.0f) < boidsSpawnerChance)
                    {
                        newTileGameObject = Instantiate(m_SpawnerTile, position, rot, transform);
                    }
                    else
                    {
                        int toSpawn = Random.Range(0, m_ToSpawn.Count);
                        newTileGameObject = Instantiate(m_ToSpawn[toSpawn], position, rot, transform);
                    }
                }

                newTileGameObject.transform.localScale = Vector3.one * 100f * m_TileSize;

                float timeToDrop = Mathf.Lerp(m_MaxTime, m_MinTime, position.magnitude / maxDistance);

                Tile tile = newTileGameObject.GetComponent<Tile>();

                if (!tile)
                {
                    tile = newTileGameObject.AddComponent<Tile>();
                }

                tile.Drop(timeToDrop);

                
            }
        }
    }
}
