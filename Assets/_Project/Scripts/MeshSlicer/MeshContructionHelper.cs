using System.Collections.Generic;
using UnityEngine;

class MeshContructionHelper
{
        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Vector2> _uvs;
        private List<Vector3> _normals;

        public List<Vector3> Vertices => _vertices;

        public List<int> Triangles => _triangles;

        public List<Vector2> Uvs => _uvs;

        public List<Vector3> Normals => _normals;

        public Mesh ConstructMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();
            mesh.normals = _normals.ToArray();
            mesh.uv = _uvs.ToArray();
            Debug.Log("Vertices "+ _vertices.Count +" UV's " + _uvs.Count);
            return mesh;
        }
        
        public MeshContructionHelper()
        {
            _triangles = new List<int>();
            _vertices = new List<Vector3>();
            _uvs = new List<Vector2>();
            _normals = new List<Vector3>();
        }
        

        public void AddTriangle(int indexA, int indexB, int indexC)
        {
            _triangles.Add(indexA);
            _triangles.Add(indexB);
            _triangles.Add(indexC);
        }

        public void AddVertex(Vector3 vertex)
        {
            _vertices.Add(vertex);
        }
        
        public void AddUV(Vector2 uv)
        {
            _uvs.Add(uv);
        }
        public void AddNormal(Vector3 normal)
        {
            _normals.Add(normal);
        }
    
}
