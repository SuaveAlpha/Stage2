using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceCube : MonoBehaviour
{

    ARRaycastManager raycastManager;
    GameObject cubeInstance = null;

    float startDistance;
    Vector3 startScale;
    bool touching = false;

    public GameObject cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            bool success = raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon);

            if (success && hits.Count > 0)
            {
                Vector3 hitPosition = hits[0].pose.position;

                if (cubeInstance == null)
                {
                    cubeInstance = Instantiate(cubePrefab, hitPosition, new Quaternion());
                } else
                {
                    cubeInstance.transform.position = hitPosition;
                }
            }
        }

        if (Input.touchCount == 2)
        {
            if (this.touching == false)
            {
                this.touching = true;
                this.startDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                this.startScale = this.cubeInstance.transform.localScale;
            } else
            {
                float newDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                float percentage = newDistance / this.startDistance;

                if (percentage < 0.01f)
                {
                    percentage = 0.01f;
                }

                if (cubeInstance != null)
                {
                    this.cubeInstance.transform.localScale = this.startScale * percentage;
                }
            }
        }

        if (Input.touchCount == 0)
        {
            this.touching = false;
        }
    }
}
