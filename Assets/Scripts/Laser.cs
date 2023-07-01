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
    static public float _maxLenghtLaser = 20f;

    void Start()
    {
        _mainLaser = Instantiate(LaserPrefab);
        _ray.origin = transform.position;
        _ray.direction = transform.up;
        SetLaserPos(_mainLaser);

    }

    void Update()
    {
        _ray.origin = transform.position;
        _ray.direction = transform.up;
        SetLaserPos(_mainLaser);
    }

    void SetLaserPos(GameObject laser)
    {
        bool intersect = Physics.Raycast(_ray, out _hit);
        LineRenderer lineRend = laser.GetComponent<LineRenderer>();
        if (intersect)
        {
            laser.transform.position = transform.position;
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, _hit.point - transform.position);
            _hit.transform.gameObject.GetComponent<ReflectController>().recivedHit = _hit;
            _hit.transform.gameObject.GetComponent<ReflectController>().recivedRay = _ray;
        }
        else
        {
            laser.transform.position = transform.position;
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, _ray.direction * _maxLenghtLaser);
        }
    }
}
