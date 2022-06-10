using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackObject : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject objectPrefab;
    private GameObject objectInstance = null;

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnChanged;
    }
    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged += OnChanged;
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage newImage in eventArgs.added)
        {
            if (this.objectInstance != null)
            {
                Destroy(objectInstance);
                this.objectInstance = null;
            }
            this.objectInstance = Instantiate(this.objectPrefab, newImage.transform.position, new Quaternion());
        }

        foreach (ARTrackedImage updatedImage in eventArgs.updated)
        {
            this.objectInstance.transform.position = updatedImage.transform.position;

            Quaternion q = new Quaternion();
            q.eulerAngles = new Vector3(0, updatedImage.transform.rotation.eulerAngles.y, 0);
            this.objectInstance.transform.rotation = q;
        }

        foreach (ARTrackedImage removedImage in eventArgs.removed)
        {
            if (this.objectInstance != null)
            {
                Destroy(this.objectInstance);
                this.objectInstance = null;
            }
        }
    }
}
