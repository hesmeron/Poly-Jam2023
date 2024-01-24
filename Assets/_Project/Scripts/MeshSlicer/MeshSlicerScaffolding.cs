using UnityEngine;

public class MeshSlicerScaffolding : MonoBehaviour
{
    [SerializeField]
    private MeshFilter _meshFilter;
    [SerializeField] 
    private Vector3 _origin;    
    [SerializeField] 
    private Vector3 _normal;

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = new Color(0, 1, 0, 0.4f);
        Gizmos.DrawCube(_origin, new Vector3(1,1,0.01f));
        Gizmos.color = new Color(0, 1, 0, 1f);
        Gizmos.DrawWireCube(_origin, new Vector3(1,1,0.01f));
    }

    public void SliceMesh()
    {
        Mesh[] meshes = MeshSlicer.SliceMesh(_meshFilter.sharedMesh, _origin, transform.forward);
        for (int index = 0; index < meshes.Length; index++)
        {
            Mesh mesh = meshes[index];
            GameObject submesh = Instantiate(this.gameObject);
            submesh.gameObject.transform.position += (2* transform.right);
            submesh.GetComponent<MeshFilter>().sharedMesh = mesh;
        }
    }
}
