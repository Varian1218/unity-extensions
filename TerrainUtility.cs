using System.Collections.Generic;
using UnityEngine;

namespace UnityBoosts
{
    public static class TerrainUtility
    {
        public static void ModifyHeight(this Terrain terrain, MeshFilter meshFilter)
        {
            var mesh = meshFilter.sharedMesh;
            var terrainData = terrain.terrainData;
            var vertices = mesh.vertices;
            var heights = terrainData.GetHeights(
                0,
                0,
                terrainData.heightmapResolution,
                terrainData.heightmapResolution
            );
            var resolution = terrainData.heightmapResolution - 1;
            var modifiedPositions = new Dictionary<int, (int X, int Z, float Y)>();

            foreach (var vertex in vertices)
            {
                var terrainPos = terrain.transform.InverseTransformPoint(meshFilter.transform.TransformPoint(vertex));
                var terrainX = Mathf.RoundToInt(terrainPos.x / terrainData.size.x * resolution);
                var terrainZ = Mathf.RoundToInt(terrainPos.z / terrainData.size.z * resolution);
                var terrainY = terrainPos.y / terrainData.size.y;

                var matrixHash = GetMatrixHash(terrainX, terrainZ);

                if (terrainY < heights[terrainZ, terrainX] &&
                    (!modifiedPositions.TryGetValue(matrixHash, out var value) || terrainY < value.Y))
                {
                    modifiedPositions[matrixHash] = (terrainX, terrainZ, terrainY);
                }
            }

            foreach (var (x, z, y) in modifiedPositions.Values)
            {
                heights[z, x] = y;
            }

            terrainData.SetHeights(0, 0, heights);
        }

        private static int GetMatrixHash(int x, int y)
        {
            return x << 16 | y;
        }
    }
}