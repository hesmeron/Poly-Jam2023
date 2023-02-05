using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    class Slicer
    {
        /// <summary>
        /// Slice the object by the plane 
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="objectToCut"></param>
        /// <returns></returns>
        public static GameObject[] Slice(Plane plane, GameObject objectToCut, Vector3 crossPoint, Vector3 origin, Vector3 normal)
        {            
            //Get the current mesh and its verts and tris
            Mesh mesh = objectToCut.GetComponent<MeshFilter>().mesh;
            var a = mesh.GetSubMesh(0);
            Sliceable originalSliceable = objectToCut.GetComponent<Sliceable>();
            if(originalSliceable == null)
            {
                throw new NotSupportedException("Cannot slice non sliceable object, add the sliceable script to the object or inherit from sliceable to support slicing");
            }
            
            //Create left and right slice of hollow object
            SlicesMetadata slicesMeta = new SlicesMetadata(plane, mesh, origin, normal);            

            GameObject positiveObject = CreateMeshGameObject(objectToCut);
            positiveObject.name = objectToCut.name;

            GameObject negativeObject = CreateMeshGameObject(objectToCut);
            negativeObject.name =objectToCut.name;

            var positiveSideMeshData = slicesMeta.PositiveSideMesh;
            var negativeSideMeshData = slicesMeta.NegativeSideMesh;

            positiveObject.GetComponent<MeshFilter>().mesh = positiveSideMeshData;
            negativeObject.GetComponent<MeshFilter>().mesh = negativeSideMeshData;

            //FillSlicableData(positiveObject, originalSliceable, crossPoint, plane.normal);
            //FillSlicableData(negativeObject, originalSliceable, crossPoint, -plane.normal);
            return new GameObject[] { positiveObject, negativeObject};
        }

        private static void FillSlicableData(GameObject positiveObject, Sliceable originalSliceable, Vector3 crossPoint, Vector3 normal)
        {
            Sliceable sliceable = positiveObject.AddComponent<Sliceable>();
            Vector3 pivot = crossPoint + normal;
            Vector3 end = originalSliceable.SliceAbleAreaEnd;
            Vector3 start = originalSliceable.SliceAbleAreaStart;
            if (Vector3.Distance(pivot, start) <= Vector3.Distance(crossPoint, start))
            {
                sliceable.Initialize(crossPoint, start);
            }
            else
            {
                sliceable.Initialize(crossPoint, end);
            }
            sliceable.ReverseWireTriangles = originalSliceable.ReverseWireTriangles;

        }

        /// <summary>
        /// Creates the default mesh game object.
        /// </summary>
        /// <param name="originalObject">The original object.</param>
        /// <returns></returns>
        private static GameObject CreateMeshGameObject(GameObject originalObject)
        {
            var originalMaterial = originalObject.GetComponent<MeshRenderer>().materials;

            GameObject meshGameObject = new GameObject();

            meshGameObject.AddComponent<MeshFilter>();
            meshGameObject.AddComponent<MeshRenderer>();

            meshGameObject.GetComponent<MeshRenderer>().materials = originalMaterial;

            meshGameObject.transform.localScale = originalObject.transform.localScale;
            meshGameObject.transform.rotation = originalObject.transform.rotation;
            meshGameObject.transform.position = originalObject.transform.position;

            meshGameObject.tag = originalObject.tag;

            return meshGameObject;
        }

        /// <summary>
        /// Add mesh collider and rigid body to game object
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="mesh"></param>
        private static void SetupCollidersAndRigidBodys(ref GameObject gameObject, Mesh mesh)
        {                     
            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;

            var rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }
}
