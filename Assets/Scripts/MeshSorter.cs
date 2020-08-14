using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshSorter : MonoBehaviour
{
    public Vector3 sortDirection;

    private void Start()
    {
        var meshfilter = GetComponent<MeshFilter>();
        if (meshfilter != null && meshfilter.mesh != null)
        {
            meshfilter.mesh = SortMesh(meshfilter.mesh, sortDirection);
        }
    }

    /// <summary>
    /// Sort the vertices in a mesh by some direction vector to make sure they are in order, to create effect of wave with ordered mesh emission
    /// </summary>
    /// <param name="mesh">Mesh to sort</param>
    /// <param name="sortDirection">Direction to sort in.</param>
    /// <returns>The sorted mesh</returns>
    public static Mesh SortMesh(Mesh mesh, Vector3 sortDirection)
    {
        //Get existing properties
        Vector3[] verts = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector4[] tangents = mesh.tangents;
        Vector2[] uv = mesh.uv;
        int[] tris = mesh.triangles;

        //Prepare 2 lists, each will store the same set of objects, but one will be sorted, and the other not.
        List<SortVertex> sortVerts = new List<SortVertex>(verts.Length);
        List<SortVertex> unsortVerts = new List<SortVertex>(verts.Length);

        for (int i = 0; i < verts.Length; i++)
        {
            //Add vertex objects to our lists, and get the dot product 
            //between each vertex position and the sort direction.
            //This will be the value that we'll sort by, later.
            sortVerts.Add(new SortVertex()
            {
                position = verts[i],
                normal = normals[i],
                tangent = tangents[i],
                uv = uv[i],
                sortValue = Vector3.Dot(verts[i], sortDirection)
            });
            unsortVerts.Add(sortVerts[i]);
        }

        //Sort the vertices using linq.
        sortVerts.Sort((lhs, rhs) => lhs.sortValue > rhs.sortValue ? -1 : 1);

        for (int i = 0; i < sortVerts.Count; i++)
        {
            //Set the new index of each vertex to its sorted value.
            sortVerts[i].sortIndex = i;
        }

        for (int i = 0; i < tris.Length; i++)
        {
            //Use the unsorted list to map unsorted indices to sorted indices.
            tris[i] = unsortVerts[tris[i]].sortIndex;
        }

        //Output resulting mesh.
        Mesh result = new Mesh();
        result.vertices = sortVerts.Select(v => v.position).ToArray();
        result.uv = sortVerts.Select(v => v.uv).ToArray();
        result.normals = sortVerts.Select(v => v.normal).ToArray();
        result.tangents = sortVerts.Select(v => v.tangent).ToArray();
        result.triangles = tris;
        return result;
    }

    private class SortVertex
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector4 tangent;
        public Vector2 uv;
        public int sortIndex;
        public float sortValue;
    }
}
