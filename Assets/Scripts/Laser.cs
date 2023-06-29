using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject LaserPrefab;
    private GameObject laser;
    private Ray ray;
    private RaycastHit hit;
    private float MaxLenghtLaser = 20f;

    void Start()
    {
        laser = Instantiate(LaserPrefab);
        LasersArray.lasers.Add(laser);
        ray.origin = transform.position;
        ray.direction = transform.up;
        SetLaserPos(laser);
        Debug.Log(ray.direction);

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
            laser.transform.position = transform.position;
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, hit.point - transform.position);
        }
        else
        {
            laser.transform.position = transform.position;
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, ray.direction * MaxLenghtLaser);
            Debug.Log(ray.direction);
        }
    }
}
