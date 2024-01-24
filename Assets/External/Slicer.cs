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
        public static GameObject[] Slice( GameObject objectToCut, Vector3 origin, Vector3 normal)
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
            Mesh[]  slicesMeta = SliceMesh(mesh, origin, normal);            

            GameObject positiveObject = CreateMeshGameObject(objectToCut);
            positiveObject.name = objectToCut.name;

            GameObject negativeObject = CreateMeshGameObject(objectToCut);
            negativeObject.name =objectToCut.name;
            
            return new GameObject[] { positiveObject, negativeObject};
        }

        private static GameObject CreateMeshGameObject(GameObject objectToCut)
        {
            throw new NotImplementedException();
        }

        private static Mesh[] SliceMesh(Mesh mesh, Vector3 cutOrigin, Vector3 cutNormal)
        {
            throw new NotImplementedException();
        }
    }
}
