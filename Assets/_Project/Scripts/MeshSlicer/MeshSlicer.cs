using System;
using UnityEngine;

public class MeshSlicer
{
    struct VertexData
    {
        public Vector3 Postion;
        public Vector2 Uv;
        public Vector3 Normal;
        public bool Side;
    }
    
    public static Mesh[] SliceMesh(Mesh mesh, Vector3 cutOrigin, Vector3 cutNormal)
    {
        int[] meshTriangles = mesh.triangles;
        
        Plane plane = new Plane(cutNormal, cutOrigin);
        MeshContructionHelper positiveMesh = new MeshContructionHelper();
        MeshContructionHelper negativeMesh = new MeshContructionHelper();
        
        for (int i = 0; i < meshTriangles.Length; i += 3)
        {
            VertexData vertexA = GetVertexData(mesh, plane, meshTriangles[i]);
            VertexData vertexB = GetVertexData(mesh, plane, meshTriangles[i+1]);
            VertexData vertexC = GetVertexData(mesh, plane, meshTriangles[i+2]);

            bool isABSameSide = vertexA.Side == vertexB.Side;
            bool isBCSameSide = vertexB.Side == vertexC.Side;
            
            //if all vertices are on the same side we save whole triangle to apropriate mesh
            if (isABSameSide && isBCSameSide)
            {
                MeshContructionHelper helper = vertexA.Side ? positiveMesh : negativeMesh;
                AddMeshSection(vertexA, vertexB, vertexC, ref helper);
            }
            else
            {
                //else we have to find intersection between triangle and cutting plane 
                VertexData intersectionD;
                VertexData intersectionE;
                MeshContructionHelper helperA = vertexA.Side ? positiveMesh : negativeMesh;
                MeshContructionHelper helperB = vertexB.Side ? positiveMesh : negativeMesh;
                MeshContructionHelper helperC = vertexC.Side ? positiveMesh : negativeMesh;
                
                if (isABSameSide)
                {
                    intersectionD = GetIntersectionVertex(vertexA, vertexC, cutOrigin, cutNormal);
                    intersectionE = GetIntersectionVertex(vertexB, vertexC, cutOrigin, cutNormal);

                    AddMeshSection(vertexA, vertexB, intersectionE, ref helperA);
                    AddMeshSection(vertexA,intersectionE, intersectionD, ref helperA);
                    AddMeshSection(intersectionE,vertexC, intersectionD, ref helperC);
                }
                else if (isBCSameSide)
                {
                    intersectionD = GetIntersectionVertex(vertexB, vertexA, cutOrigin, cutNormal);
                    intersectionE = GetIntersectionVertex(vertexC, vertexA, cutOrigin, cutNormal);

                    AddMeshSection(vertexB, vertexC, intersectionE, ref helperB);
                    AddMeshSection(vertexB,intersectionE, intersectionD, ref helperB);
                    AddMeshSection(intersectionE, vertexA,intersectionD, ref helperA);
                }
                else
                {
                    intersectionD = GetIntersectionVertex(vertexA, vertexB, cutOrigin, cutNormal);
                    intersectionE = GetIntersectionVertex(vertexC, vertexB, cutOrigin, cutNormal);

                    AddMeshSection(vertexA,  intersectionE, vertexC,ref helperA);
                    AddMeshSection(intersectionD, intersectionE, vertexA,ref helperA);
                    AddMeshSection(vertexB,intersectionE, intersectionD, ref helperB);
                }
            }

        }

        return new[] { positiveMesh.ConstructMesh(), negativeMesh.ConstructMesh()};
    }

    private static VertexData GetVertexData(Mesh mesh, Plane plane, int index)
    {
        Vector3 position = mesh.vertices[index];
        VertexData vertexData = new VertexData()
        {
            Postion = position,
            Side = plane.GetSide(position),
            Uv = mesh.uv[index],
            Normal = mesh.normals[index]
        };
        return vertexData;
    }

    private static void AddMeshSection(VertexData vertexA, VertexData vertexB, VertexData vertexC, ref MeshContructionHelper helper)
    {
        helper.AddVertex(vertexA.Postion);
        helper.AddNormal(vertexA.Normal);
        helper.AddUV(vertexA.Uv);        
        
        helper.AddVertex(vertexB.Postion);
        helper.AddNormal(vertexB.Normal);
        helper.AddUV(vertexB.Uv);       
        
        helper.AddVertex(vertexC.Postion);
        helper.AddNormal(vertexC.Normal);
        helper.AddUV(vertexC.Uv);

        int baseIndex = helper.Vertices.Count;
        helper.AddTriangle(baseIndex-3, baseIndex-2, baseIndex-1);
    }
    
    private static VertexData GetIntersectionVertex(VertexData vertexA, VertexData vertexB, Vector3 planeOrigin, Vector3 normal)
    {
        Trigonometry.PointIntersectsAPlane(vertexA.Postion, vertexB.Postion, planeOrigin, normal, out Vector3 result);
        return new VertexData()
        {
            Postion = result,
            Normal = (vertexA.Normal + vertexB.Normal)/2f,
            Uv = Vector2.zero
        };
    }
}
