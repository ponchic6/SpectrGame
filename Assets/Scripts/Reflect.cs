using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Reflect : MonoBehaviour
{
    [SerializeField] private GameObject LaserPrefab;
    private List<GameObject> _selfLasers = new List<GameObject>();
    private List<Ray> _selfRays = new List<Ray>();
    private List<Vector3> _finishPoints = new List<Vector3>();
    private List<Ray> _savedRays = new List<Ray>();

    public List<RaycastHit> recivedHits = new List<RaycastHit>();
    public List<Ray> recivedRays = new List<Ray>();
    public List<Color> recivedColors = new List<Color>();

    void Update()
    {
        if (recivedHits != new List<RaycastHit>())
        {          
            CalculateRayAndHit(recivedRays, recivedHits);
            RenderLaser(_selfLasers, _finishPoints);
            recivedHits.Clear();
            recivedRays.Clear();
            recivedColors.Clear();
            _finishPoints.Clear();
            _selfRays.Clear();
            
            

        }
        else
        {
            recivedHits.Clear();
            recivedRays.Clear();
            recivedColors.Clear();
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

    void CalculateRayAndHit(List<Ray> recivedRays, List<RaycastHit> recivedHits)
    {   
        foreach(Ray ray in recivedRays)
        {
            Ray selfRay = new Ray();
            RaycastHit selfHit;
            Vector3 finishPoint;
            selfRay.origin = recivedHits[recivedRays.IndexOf(ray)].point;
            selfRay.direction = Vector3.Reflect(ray.direction, recivedHits[recivedRays.IndexOf(ray)].normal);
            _selfRays.Add(selfRay);
            bool intersect = Physics.Raycast(selfRay, out selfHit);
            if (intersect)
            {
                finishPoint = selfHit.point - selfRay.origin;
                _finishPoints.Add(finishPoint);
                if (selfHit.collider.tag == "Reflector") 
                { 
                    selfHit.collider.gameObject.GetComponent<Reflect>().recivedHits.Add(selfHit);
                    selfHit.collider.gameObject.GetComponent<Reflect>().recivedRays.Add(selfRay);
                    selfHit.collider.gameObject.GetComponent<Reflect>().recivedColors.Add(recivedColors[recivedRays.IndexOf(ray)]);
                }
                if (selfHit.collider.tag == "Interferometr")
                {
                    selfHit.collider.gameObject.GetComponent<Interferometr>().recivedHits.Add(selfHit);
                    selfHit.collider.gameObject.GetComponent<Interferometr>().recivedRays.Add(selfRay);
                }
            }
            else
            {
                finishPoint = selfRay.direction * SourceLaser._maxLenghtLaser;
                _finishPoints.Add(finishPoint);
            }
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
                lineRend.startColor = recivedColors[_selfRays.IndexOf(ray)];
                lineRend.endColor = recivedColors[_selfRays.IndexOf(ray)];
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

        _savedRays = new List<Ray>(_selfRays);

    }

}
