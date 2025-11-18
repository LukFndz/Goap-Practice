using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Queries : MonoBehaviour
{
    public float radius = 20f;

    public IEnumerable<GridEntity> selected = new List<GridEntity>();
    
    public IEnumerable<GridEntity> Query()
    {
        var spatialGrid = GameManager.instance.GetSpatialGrid();
        
        var entityList = spatialGrid.Query(
            transform.position + new Vector3(-radius, 0, -radius),
            transform.position + new Vector3(radius, 0, radius),
            x =>
            {
                var position2d = x - transform.position;
                position2d.y = 0;
                return position2d.sqrMagnitude < radius * radius;
            });
        return entityList;
    }

    void OnDrawGizmosSelected()
    {
        //Flatten the sphere we're going to draw
        Gizmos.color = Color.cyan;
        Gizmos.matrix *= Matrix4x4.Scale(Vector3.forward + Vector3.right);
        Gizmos.DrawWireSphere(transform.position, radius);

        if (Application.isPlaying)
        {
            selected = Query();
            var temp = FindObjectsOfType<GridEntity>().Where(x=>!selected.Contains(x));
            foreach (var item in temp)
            {
                item.onGrid = false;
            }
            foreach (var item in selected)
            {
                item.onGrid = true;
            }

        }
    }


    private void OnGUI()
    {
        GUI.Label( new Rect(0,0,20,20), "HOLA");
    }
}
