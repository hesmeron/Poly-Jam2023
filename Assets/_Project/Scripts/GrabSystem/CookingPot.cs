using System;
using System.Collections;
using UnityEngine;

public class CookingPot : GrabDestination
{
    [SerializeField] 
    private Vector3 _start, _end;
    private GrabTarget _target;
    [SerializeField]
    private int[] _ingredientsWithCount;

    private void Awake()
    {
        int length = Enum.GetNames(typeof(IngredientType)).Length;
        _ingredientsWithCount = new int[length];
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawLine(_start + transform.position, _end + transform.position);
    }

    protected override void OnReach(GrabTarget grabTarget)
    {
        base.OnReach(grabTarget);
        if (grabTarget.TryGetComponent(out Ingredient ingredient))
        {
            _target = grabTarget;
            StartCoroutine(FallDownCoroutine());
        }
        else if(grabTarget.TryGetComponent(out Recipe recipe))
        {
            if (recipe.FitsRequirements(_ingredientsWithCount))
            {
                Debug.Log("Hurray");
            }
     
            for(int i=0; i < _ingredientsWithCount.Length; i++)
            {
                _ingredientsWithCount[i] = 0;
            }
            
        }

    }

    IEnumerator FallDownCoroutine()
    {
        float timePassed = 0f;
        float duration = 1.5f;
        Vector3 start = _start + transform.position;
        Vector3 end = _end + transform.position;
        Vector3 dir = (end-start);
        while (timePassed <= duration)
        {
            _target.transform.position = start + (dir *(timePassed / duration));
            yield return null;
            timePassed += Time.deltaTime;
        }

        int index = (int) _target.GetComponent<Ingredient>().IngredientType;
        _ingredientsWithCount[index]++;
        Destroy(_target.gameObject);
    }
}
