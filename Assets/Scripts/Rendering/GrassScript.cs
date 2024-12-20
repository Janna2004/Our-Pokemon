using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMapGenerator : MonoBehaviour
{
    public Tilemap groundTilemap; // 草地和水的 Tilemap
    public Tilemap treeTilemap;   // 树的 Tilemap
    public Tile[] groundTile;     // 草地 Tile
    public RuleTile waterRuleTile;// 水 Tile
    public Tile[] treeTiles;      // 树 Tile 数组（四种树）

    private int mapWidth = 16 + 2; // 地图宽度
    private int mapHeight = 16 + 2; // 地图高度
    private int groupSize = 2; // 每组的大小

    private byte[,] mapData; // 存储地图数据

    void Start()
    {
        GenerateMapData(); // 生成地图数据
        RenderMap();       // 绘制地图
    }

    // 生成地图数据
    void GenerateMapData()
    {
        mapData = new byte[mapWidth, mapHeight];

        // 初始化地图为全草地
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                mapData[x, y] = 0; // 草地：0
            }
        }

        // 强制在边缘放置随机树
        PlaceEdgeTrees();

        // 减少水分组和树分组的总数量，同时空出最外圈
        int totalGroupsX = (mapWidth - 2) / groupSize; // 不包括边缘
        int totalGroupsY = (mapHeight - 2) / groupSize;

        int waterGroups = UnityEngine.Random.Range(2, 3); // 总水分组数量
        int treeGroups = UnityEngine.Random.Range(2, 3);  // 总树分组数量

        // 用来标记哪些分组已经被占用
        bool[,] occupiedGroups = new bool[totalGroupsX, totalGroupsY];

        // 随机生成水分组
        for (int i = 0; i < waterGroups; i++)
        {
            // 找到一个未被占用的分组
            int groupX, groupY;
            do
            {
                groupX = UnityEngine.Random.Range(1, totalGroupsX); // 从第1组开始
                groupY = UnityEngine.Random.Range(1, totalGroupsY); // 从第1组开始
            } while (occupiedGroups[groupX, groupY]); // 如果这个分组已经被占用，则继续选择

            // 标记该分组为已占用
            occupiedGroups[groupX, groupY] = true;

            GenerateWaterGroup(groupX * groupSize, groupY * groupSize);
        }

        // 随机生成树分组
        for (int i = 0; i < treeGroups; i++)
        {
            // 找到一个未被占用的分组
            int groupX, groupY;
            do
            {
                groupX = UnityEngine.Random.Range(1, totalGroupsX); // 从第1组开始
                groupY = UnityEngine.Random.Range(1, totalGroupsY); // 从第1组开始
            } while (occupiedGroups[groupX, groupY]); // 如果这个分组已经被占用，则继续选择

            // 标记该分组为已占用
            occupiedGroups[groupX, groupY] = true;

            GenerateTreeGroup(groupX * groupSize-1, groupY * groupSize-1);
        }
    }

    // 在边缘放置随机树
    void PlaceEdgeTrees()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            // 上边缘
            mapData[x, mapHeight - 1] = 2; // 树：2
            // 下边缘
            mapData[x, 0] = 2;
        }

        for (int y = 0; y < mapHeight; y++)
        {
            // 左边缘
            mapData[0, y] = 2;
            // 右边缘
            mapData[mapWidth - 1, y] = 2;
        }
    }

    // 在2*2组内随机生成水
    void GenerateWaterGroup(int startX, int startY)
    {
        int waterCount = UnityEngine.Random.Range(2, 4);

        for (int i = 0; i < waterCount; i++)
        {
            // 确保随机生成的 x 和 y 在地图范围内
            int x = UnityEngine.Random.Range(startX, Mathf.Min(startX + groupSize, mapWidth - 1));
            int y = UnityEngine.Random.Range(startY, Mathf.Min(startY + groupSize, mapHeight - 1));

            // 只有在合法范围内时才设置
            if (x >= 1 && x < mapWidth - 1 && y >= 1 && y < mapHeight - 1)
            {
                mapData[x, y] = 1; // 水：1
            }
        }
    }

    // 在2*2组内随机生成树
    void GenerateTreeGroup(int startX, int startY)
    {
        int treeCount = UnityEngine.Random.Range(2, 4);

        for (int i = 0; i < treeCount; i++)
        {
            // 确保随机生成的 x 和 y 在地图范围内
            int x = UnityEngine.Random.Range(startX, Mathf.Min(startX + groupSize, mapWidth - 1));
            int y = UnityEngine.Random.Range(startY, Mathf.Min(startY + groupSize, mapHeight - 1));

            // 只有在合法范围内时才设置
            if (x >= 1 && x < mapWidth - 1 && y >= 1 && y < mapHeight - 1)
            {
                mapData[x, y] = 2; // 树：2
            }
        }
    }

    // 渲染地图
    void RenderMap()
    {
        groundTilemap.ClearAllTiles(); // 清除地形 Tilemap
        treeTilemap.ClearAllTiles();   // 清除树 Tilemap

        // 偏移量，使地图居中
        int offsetX = -mapWidth / 2;
        int offsetY = -mapHeight / 2;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x + offsetX, y + offsetY, 0);

                // 绘制草地或水
                if (mapData[x, y] == 0) // 草地
                {
                    Tile groundType = groundTile[UnityEngine.Random.Range(0, groundTile.Length)];
                    groundTilemap.SetTile(tilePosition, groundType);
                }
                else if (mapData[x, y] == 1) // 水
                {
                    groundTilemap.SetTile(tilePosition, waterRuleTile); // 使用 RuleTile 绘制水
                }

                // 绘制树
                if (mapData[x, y] == 2) // 树
                {
                    Tile groundType = groundTile[UnityEngine.Random.Range(0, groundTile.Length)];
                    groundTilemap.SetTile(tilePosition, groundType);
                    Tile treeType = treeTiles[UnityEngine.Random.Range(0, treeTiles.Length)];
                    treeTilemap.SetTile(tilePosition, treeType);

                    // 动态调整树的 Sorting Order，使 y 轴靠下的树覆盖靠上的树
                    var tileRenderer = treeTilemap.GetComponent<TilemapRenderer>();
                    tileRenderer.mode = TilemapRenderer.Mode.Individual; // 确保使用 Individual 模式
                    tileRenderer.sortingOrder = -(tilePosition.y);       // 按 y 值排序
                }
            }
        }
    }
}