using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject LaserPrefab;
    private GameObject laser;
    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        laser = Instantiate(LaserPrefab, Vector3.zero, Quaternion.identity);
        LasersArray.lasers.Add(laser);
        ray.origin = transform.position;
        ray.direction = transform.up;
        SetLaserPos(laser);

    }

    void Update()
    {
        ray.origin = transform.position;
        ray.direction = transform.up;
        SetLaserPos(laser);
    }

    void SetLaserPos(GameObject laser)
    {
        bool intersect = Physics.Raycast(ray, out hit);
        LineRenderer lineRend = laser.GetComponent<LineRenderer>();
        if (intersect)
        {
            laser.transform.position = ray.origin;
            lineRend.SetPosition(0, ray.origin);
            lineRend.SetPosition(1, hit.point);
        }
        else
        {   
            laser.transform.position = ray.origin;
            lineRend.SetPosition(0, ray.origin);
            lineRend.SetPosition(1, ray.origin + transform.up * 2 );
        }
    }
}
