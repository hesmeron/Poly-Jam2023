using System;
using UnityEngine;

public static class BetterSlicer
{
    public static GameObject[] Slice(GameObject objectToCut, Vector3 start, Vector3 end, Vector3 direction)
    {
        GameObject[] slicedObjects = new GameObject[2];

        //slicedObjects[0] = CreateMeshGameObject();
        //slicedObjects[1] = CreateMeshGameObject();
        return slicedObjects;
    }
    
    
    private static GameObject CreateMeshGameObject(GameObject originalObject, Mesh mesh)
    {
        var originalMaterial = originalObject.GetComponent<MeshRenderer>().materials;

        GameObject meshGameObject = new GameObject();

        MeshFilter filter = meshGameObject.AddComponent<MeshFilter>();
        MeshRenderer renderer = meshGameObject.AddComponent<MeshRenderer>();

        renderer.materials = originalMaterial;
        filter.mesh = mesh;

        meshGameObject.transform.localScale = originalObject.transform.localScale;
        meshGameObject.transform.rotation = originalObject.transform.rotation;
        meshGameObject.transform.position = originalObject.transform.position;
        meshGameObject.name = originalObject.name;
        meshGameObject.tag = originalObject.tag;

        return meshGameObject;
    }
}
