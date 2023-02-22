using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [Header ("Tile Atlas")]
    public TileAtlas tileAtlas;

    [Header ("Trees")]
    public int treeChance = 10;
    public int minTreeHeight = 4;
    public int maxTreeHeight = 7;

    [Header ("Generation Settings")]
    public int dirtLayerHeight = 5;
    public int chunkSize = 16;
    public int worldSize = 100;
    public bool generateCaves = true;
    public float surfaceValue = 0.25f;
    public float heightMultiplier = 4f;
    public int heightAddition = 25;

    [Header ("Noise Settings")]
    public float caveFreq = 0.05f;
    public float terrainFreq = 0.05f;
    public float seed;
    public Texture2D caveNoiseTexture;

    [Header ("Ore Settings")]
    public float coalRarity;
    public float ironRarity;
    public float carbonRarity;
    public float tungstenRarity;
    public float plutoniumRarity;
    public texture2D coalSpread;
    public texture2D ironSpread;
    public texture2D carbonSpread;
    public texture2D tungstenSpread;
    public texture2D plutoniumSpread;

    private GameObject[] worldChunks;
    private List<Vector2> worldTiles = new List<Vector2>();

    private void OnValidate()
    {

    }

    private void Start()
    {
        seed = Random.Range(-10000, 10000);
        GenerateNoiseTexture(caveFreq, caveNoiseTexture);
        //Ores
        GenerateNoiseTexture(coalRarity, coalSpread);
        GenerateNoiseTexture(ironRarity, ironSpread);
        GenerateNoiseTexture(carbonRarity, carbonSpread);
        GenerateNoiseTexture(tungstenlRarity, tungstenSpread);
        GenerateNoiseTexture(plutoniumRarity, plutoniumSpread);

        CreateChunks();
        GenerateTerrain();
    }

    public void CreateChunks()
    {
        int numChunks = worldSize / chunkSize;
        worldChunks = new GameObject[numChunks];

        for (int i = 0; i < numChunks; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform;
            worldChunks[i] = newChunk;
        }
    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < worldSize; x++)
        {
            float height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * heightMultiplier + heightAddition;

            for (int y = 0; y < height; y++)
            {
                Sprite tileSprite;
                if (y < height - dirtLayerHeight)
                {
                    tileSprite = tileAtlas.stone.tileSprite;
                }
                else if (y > height - 1)
                {
                    // top layer of terrain
                    tileSprite = tileAtlas.grass.tileSprite;
                }
                else
                {
                    tileSprite = tileAtlas.dirt.tileSprite;
                }

                if (generateCaves)
                {
                    if (caveNoiseTexture.GetPixel(x, y).r > surfaceValue)
                    {
                        PlaceTile(tileSprite, x, y);
                    }
                }
                else
                {
                    PlaceTile(tileSprite, x, y);
                }

                if (y >= height - 1)
                {
                    int t = Random.Range(0, treeChance);
                    if (t == 1)
                    {
                        //generate tree
                        if (worldTiles.Contains(new Vector2(x, y)))
                        {
                            GenerateTree(x, y + 1);
                        }
                    }
                }
            }
        }
    }

    public void GenerateNoiseTexture(float frequency, Texture 2D noiseTexture)
    {
        Texture2D noise = new Texture2D(worldSize, worldSize);

        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                float  v = Mathf.PerlinNoise((x + seed) * frequency, (y + seed) * frequency);
                noiseTexture.SetPixel(x, y, new Color(v, v, v));
            }
        }

        noiseTexture.Apply();
    }

    public void GenerateTree(int x, int y)
    {
        // Define Our Tree

        // Generate Logs
        int treeHeight = Random.Range(minTreeHeight, maxTreeHeight);
        for (int i = 0; i < treeHeight; i++)
        {
            PlaceTile(tileAtlas.pineLog.tileSprite, x, y + i);
        }

        // Generate Leaves
        PlaceTile(tileAtlas.pineLeaf.tileSprite, x, y + treeHeight);
        PlaceTile(tileAtlas.pineLeaf.tileSprite, x, y + treeHeight + 1);
        PlaceTile(tileAtlas.pineLeaf.tileSprite, x, y + treeHeight + 2);

        PlaceTile(tileAtlas.pineLeaf.tileSprite, x - 1, y + treeHeight);
        PlaceTile(tileAtlas.pineLeaf.tileSprite, x - 1, y + treeHeight + 1);

        PlaceTile(tileAtlas.pineLeaf.tileSprite, x + 1, y + treeHeight);
        PlaceTile(tileAtlas.pineLeaf.tileSprite, x + 1, y + treeHeight + 1);
    }

    public void PlaceTile(Sprite tileSprite, int x, int y)
    {
        GameObject newTile = new GameObject();

        float chunkCoord = (Mathf.Round(x / chunkSize) * chunkSize);
        chunkCoord /= chunkSize;
        newTile.transform.parent = worldChunks[(int)chunkCoord].transform;

        newTile.AddComponent<SpriteRenderer>();
        newTile.GetComponent<SpriteRenderer>().sprite = tileSprite;
        newTile.name = tileSprite.name;
        newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

        worldTiles.Add(newTile.transform.position - (Vector3.one * 0.5f));
    }
}
