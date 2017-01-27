using UnityEngine;
using System.Collections;

public static class MathUtils
{
    //Return a point inside the specified area
    public static Vector3 PointInsideArea(Collider area)
    {
        Vector3 min = area.bounds.min;
        Vector3 max = area.bounds.max;
        Vector3 point = new Vector3();
        bool gotHit = false;
        RaycastHit hit;

        for (int i = 0; i < 100 && !gotHit; i++)
        {
            point.x = Random.Range(min.x, max.x);
            point.y = Random.Range(min.y, max.y);
            point.z = Random.Range(min.z, max.z);

            Ray ray = new Ray(point + new Vector3(0, 100, 0), Vector3.down);
            gotHit = area.Raycast(ray, out hit, 100f);
        }

        return point;
    }

    //Snap the point to the ground
    public static Vector3 SnapToTheGround(Vector3 point, Collider ground)
    {
        Vector3 origin = new Vector3(point.x, ground.bounds.max.y + 10, point.z);
        Ray ray = new Ray(origin, Vector3.down);
        RaycastHit hit;
        if (ground.Raycast(ray, out hit, ground.bounds.extents.y + 20.0f))
        {
            point = hit.point;
        }
        return point;
    }

}
