using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectController : MonoBehaviour
{
    [SerializeField] private GameObject LaserPrefab;
    private GameObject _selfLaser;
    private RaycastHit _emptyHit;
    private Ray _emptyRay;
    private RaycastHit _selfHit;
    private Ray _selfRay;

    public RaycastHit recivedHit;
    public Ray recivedRay;

    void Update()
    {
        if (recivedHit.collider != null)
        {
            if (_selfLaser == null) _selfLaser = Instantiate(LaserPrefab);
            _selfRay.origin = recivedHit.point;
            _selfRay.direction = Vector3.Reflect(recivedRay.direction, recivedHit.normal);
            SetLaserPos(_selfLaser);
        }
        else Destroy(_selfLaser);

        recivedHit = _emptyHit;
        recivedRay = _emptyRay;
    }

    void SetLaserPos(GameObject laser)
    {
        bool intersect = Physics.Raycast(_selfRay, out _selfHit);
        LineRenderer lineRend = laser.GetComponent<LineRenderer>();
        if (intersect)
        {
            laser.transform.position = recivedHit.point;
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, _selfHit.point - recivedHit.point);
            _selfHit.transform.gameObject.GetComponent<ReflectController>().recivedHit = _selfHit;
            _selfHit.transform.gameObject.GetComponent<ReflectController>().recivedRay = _selfRay;
        }
        else
        {
            laser.transform.position = recivedHit.point;
            lineRend.SetPosition(0, Vector3.zero);
            lineRend.SetPosition(1, _selfRay.direction * Laser._maxLenghtLaser);
        }
    }
}
