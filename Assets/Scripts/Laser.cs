using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private GameObject LaserPrefab;
    private GameObject _mainLaser;
    private Ray _ray;
    private RaycastHit _hit;
    private Vector3 _finishPointOfRay;

    static public float _maxLenghtLaser = 20f;

    void Start()
    {
        _mainLaser = Instantiate(LaserPrefab);
        CalculateRayAndHit(_ray, _hit);
        RenderLaser(_mainLaser);

    }

    void Update()
    {
        CalculateRayAndHit(_ray, _hit);
        RenderLaser(_mainLaser);
    }

    void RenderLaser(GameObject laser)
    {
        LineRenderer lineRend = laser.GetComponent<LineRenderer>();     
        laser.transform.position = transform.position + transform.up;
        lineRend.SetPosition(0, Vector3.zero);
        lineRend.SetPosition(1, _finishPointOfRay);
        
    }

    void CalculateRayAndHit(Ray ray, RaycastHit hit)
    {
        ray.origin = transform.position + transform.up;
        ray.direction = transform.up;

        bool intersect = Physics.Raycast(ray, out hit);
        if (intersect)
        {   
            _finishPointOfRay = hit.point - ray.origin;
            hit.collider.gameObject.GetComponent<ReflectController>().recivedHits.Add(hit);
            hit.collider.gameObject.GetComponent<ReflectController>().recivedRays.Add(ray);
        }
        else _finishPointOfRay = ray.direction * _maxLenghtLaser;

    }
}
