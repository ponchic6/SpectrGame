using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interferometr : MonoBehaviour
{
    [SerializeField] private GameObject LaserPrefab;
    private List<GameObject> _selfLasers = new List<GameObject>();
    private List<Ray> _selfRays = new List<Ray>();
    private List<Color> _selfColors = new List<Color>();
    private List<Vector3> _finishPoints = new List<Vector3>();
    private List<Ray> _savedRays = new List<Ray>();

    public List<RaycastHit> recivedHits = new List<RaycastHit>();
    public List<Ray> recivedRays = new List<Ray>();


    void Update()
    {
        if (recivedHits.Count != 0)
        {
            CalculateRayAndHit(recivedRays, recivedHits);
            RenderLaser(_selfLasers, _finishPoints);
            _savedRays = new List<Ray>(_selfRays);
            recivedHits.Clear();
            recivedRays.Clear();
            _finishPoints.Clear();
            _selfRays.Clear();
        }
        else
        {
            recivedHits.Clear();
            recivedRays.Clear();
            _finishPoints.Clear();
            _selfRays.Clear();
            foreach (GameObject go in _selfLasers)
            {
                Destroy(go);
            }
            _selfLasers.Clear();
            _savedRays.Clear();
        }
    }

    private void OnIntersect(bool intersect, RaycastHit hit, Ray ray, Vector3 finishPoint, Color color)
    {
        if (intersect)
        {
            if (hit.collider.tag == "Reflector")
            {
                hit.collider.gameObject.GetComponent<Reflect>().recivedHits.Add(hit);
                hit.collider.gameObject.GetComponent<Reflect>().recivedRays.Add(ray);
                hit.collider.gameObject.GetComponent<Reflect>().recivedColors.Add(color);
            }
            finishPoint = hit.point - ray.origin;
            _finishPoints.Add(finishPoint);
        }
        else
        {
            finishPoint = ray.direction * SourceLaser._maxLenghtLaser;
            _finishPoints.Add(finishPoint);
        }
    }

    private void CalculateRayAndHit(List<Ray> recivedRay, List<RaycastHit> recivedHit)
    {
        foreach (RaycastHit hit in recivedHit)
        {
            Ray selfRay1 = new Ray();
            Ray selfRay2 = new Ray();
            Ray selfRay3 = new Ray();
            RaycastHit selfHit1;
            RaycastHit selfHit2;
            RaycastHit selfHit3;
            Vector3 finishPoint1 = new Vector3();
            Vector3 finishPoint2 = new Vector3();
            Vector3 finishPoint3 = new Vector3();
            selfRay1.origin = hit.point - hit.normal;
            selfRay2.origin = hit.point - hit.normal;
            selfRay3.origin = hit.point - hit.normal;
            selfRay1.direction = Quaternion.Euler(0, 30, 0) * recivedRay[recivedHit.IndexOf(hit)].direction;
            selfRay2.direction = Quaternion.Euler(0, 0, 0) * recivedRay[recivedHit.IndexOf(hit)].direction;
            selfRay3.direction = Quaternion.Euler(0, -30, 0) * recivedRay[recivedHit.IndexOf(hit)].direction;
            _selfRays.Add(selfRay1);
            _selfRays.Add(selfRay2);
            _selfRays.Add(selfRay3);
            bool intersect1 = Physics.Raycast(selfRay1, out selfHit1);
            bool intersect2 = Physics.Raycast(selfRay2, out selfHit2);
            bool intersect3 = Physics.Raycast(selfRay3, out selfHit3);
            OnIntersect(intersect1, selfHit1, selfRay1, finishPoint1, Color.green);
            OnIntersect(intersect2, selfHit2, selfRay2, finishPoint2, Color.blue);
            OnIntersect(intersect3, selfHit3, selfRay3, finishPoint3, Color.red);
        }

    }

    void RenderLaser(List<GameObject> selfLasers, List<Vector3> finishPoints)
    {
        if (_savedRays.Count != _selfRays.Count)
        {
            foreach (GameObject go in _selfLasers)
            {
                Destroy(go);
            }
            _selfLasers.Clear();

            foreach (Ray ray in _selfRays)
            {
                GameObject go = Instantiate(LaserPrefab);
                selfLasers.Add(go);
                LineRenderer lineRend = go.GetComponent<LineRenderer>();
                if ((_selfRays.IndexOf(ray) + 1) % 3 == 0)
                {   
                    lineRend.startColor = Color.red;
                    lineRend.endColor = Color.red;
                }
                if ((_selfRays.IndexOf(ray) + 1) % 3 == 1) 
                {
                    lineRend.startColor = Color.green;
                    lineRend.endColor = Color.green;
                }
                if ((_selfRays.IndexOf(ray) + 1) % 3 == 2)
                {
                    lineRend.startColor = Color.blue;
                    lineRend.endColor = Color.blue;
                }
                go.transform.position = ray.origin;
                lineRend.SetPosition(0, Vector3.zero);
                lineRend.SetPosition(1, finishPoints[_selfRays.IndexOf(ray)]);
            }
        }
        else
        {
            foreach (GameObject go in selfLasers)
            {
                LineRenderer lineRend = go.GetComponent<LineRenderer>();
                go.transform.position = _selfRays[selfLasers.IndexOf(go)].origin;
                lineRend.SetPosition(0, Vector3.zero);
                lineRend.SetPosition(1, finishPoints[selfLasers.IndexOf(go)]);
            }
        }


    }
}
