using UnityEngine;
using UnityEngine.AI;

public class MinimapMesh : MonoBehaviour
{
    [SerializeField] MeshFilter filter;

    private void Awake()
    {
        Bake();
    }

    [ContextMenu("Bake Mesh")]
    void Bake()
    {
        NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();
        Mesh mesh = new Mesh();
        mesh.vertices = triangles.vertices;
        mesh.triangles = triangles.indices;
        filter.mesh = mesh;
    }
}
