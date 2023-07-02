using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReflectController : MonoBehaviour
{
    [SerializeField] private GameObject LaserPrefab;
    private List<GameObject> _selfLasers = new List<GameObject>();
    private List<Ray> _selfRays = new List<Ray>();
    private List<Vector3> _finishPoints = new List<Vector3>();
    private List<Ray> _savedRays = new List<Ray>();

    public List<RaycastHit> recivedHits = new List<RaycastHit>();
    public List<Ray> recivedRays = new List<Ray>();

    void Update()
    {
        Debug.Log(_savedRays.Count);
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

    void CalculateRayAndHit(List<Ray> recivedRay, List<RaycastHit> recivedHit)
    {   
        foreach(RaycastHit hit in recivedHit)
        {
            Ray selfRay = new Ray();
            RaycastHit selfHit;
            Vector3 finishPoint;
            selfRay.origin = hit.point;
            selfRay.direction = Vector3.Reflect(recivedRay[recivedHit.IndexOf(hit)].direction, hit.normal);
            _selfRays.Add(selfRay);
            bool intersect = Physics.Raycast(selfRay, out selfHit);
            if (intersect)
            {
                finishPoint = selfHit.point - selfRay.origin;
                _finishPoints.Add(finishPoint);
                selfHit.collider.gameObject.GetComponent<ReflectController>().recivedHits.Add(selfHit);
                selfHit.collider.gameObject.GetComponent<ReflectController>().recivedRays.Add(selfRay);
            }
            else
            {
                finishPoint = selfRay.direction * Laser._maxLenghtLaser;
                _finishPoints.Add(finishPoint);
            }
        }

    }

    void RenderLaser(List<GameObject> selfLasers, List<Vector3> finishPoints)
    {   
        if (_savedRays.Count != _selfRays.Count)
        {   
            foreach (GameObject gameobj in _selfLasers)
            {
                Destroy(gameobj);
            }
            _selfLasers.Clear();

            foreach (Ray ray in _selfRays)
            {
                GameObject go = Instantiate(LaserPrefab);
                selfLasers.Add(go);
                LineRenderer lineRend = go.GetComponent<LineRenderer>();
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
