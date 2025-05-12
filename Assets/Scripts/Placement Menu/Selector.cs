using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Selector : MonoBehaviour
{
    private Placeable selected = null;
    private GameObject preview = null;
    private GameObject measurement = null;
    private Collider previewCollider = null;
    private Collider measureCollider = null;
    private Vector3 spawnLoc;

    public int previewLayer;
    public int placeLayer;
    public int canRemoveLayer;
    public int ignoreLayer;
    public Material previewMaterial;
    public Material invalidPreviewMaterial;
    public LayerMask selectionLayerMask;
    public LayerMask overlapLayerMask;
    public float selectionDistance = Mathf.Infinity;
    public float spawnDistance = 2f;
    public float smallOffset = 0.005f;

    private void UpdateMaterial(Transform go, Material mat)
    {
        go.GetComponent<Renderer>().material = mat;
        foreach (Transform child in go)
        {
            child.GetComponent<Renderer>().material = mat;
        }
    }   
    
    private void UpdateLayer(Transform go, int newLayer)
    {
        go.gameObject.layer = newLayer;
        foreach (Transform child in go.transform)
        {
            child.gameObject.layer = newLayer;
        }
    }
    
    public void Select(Placeable selected)
    {
        this.selected = selected;
        if (preview)
        {
            Destroy(preview);
        }
        
        preview = Instantiate(selected.prefab, Vector3.zero, Quaternion.identity);
        measurement = Instantiate(selected.prefab, Vector3.zero, Quaternion.identity);
        previewCollider = preview.GetComponent<Collider>();
        measureCollider = measurement.GetComponent<Collider>();
        // var renderer = preview.GetComponent<Renderer>();
        // renderer.material = previewMaterial;
        UpdateMaterial(preview.transform, previewMaterial);
        // preview.layer = previewLayer;
        UpdateLayer(preview.transform, previewLayer);
        UpdateLayer(measurement.transform, previewLayer);
        measurement.SetActive(false);
        preview.SetActive(false);
    }

    private void Update()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, selectionDistance, selectionLayerMask);
        if (Input.GetMouseButtonDown(1))
        {
            if (preview)
            {
                Deselect();
            }
            else if(hit.collider && hit.collider.gameObject.layer == canRemoveLayer)
            {
                Destroy(hit.collider.transform.root.gameObject);
            }
            else
            {
                Debug.Log($"hit: {hit.collider.gameObject.name}");
            }
        }else if (hit.collider && hit.collider.gameObject.layer == placeLayer)
        {
            
            Vector3 itemCenterOffset = hit.collider.bounds.center - hit.collider.transform.position;
            float itemExtent = Vector3.Dot(hit.collider.bounds.extents, hit.normal);
            spawnLoc = hit.point + spawnDistance * hit.normal;
            
            Vector3 flatForward = new Vector3(hit.normal.x, 0, hit.normal.z).normalized;
            if (flatForward.sqrMagnitude < Mathf.Epsilon) flatForward = Vector3.forward;
            
            Quaternion spawnRot = Quaternion.LookRotation(flatForward, Vector3.up);            
            if(preview)
            {
                if (!preview.activeInHierarchy)
                {
                    preview.SetActive(true);
                }
                preview.transform.rotation = spawnRot;
                // preview.transform.position = spawnLoc;
                measurement.transform.rotation = spawnRot;
                measurement.transform.position = spawnLoc;
                measurement.SetActive(true);
                spawnLoc += hit.point - measureCollider.ClosestPointOnBounds(hit.point) + hit.normal*smallOffset; //TODO: use a third object for the raycast test and toggle active everytime
                measurement.SetActive(false);
                preview.transform.position = spawnLoc;
                // Debug.Log($"closest on bounds: {previewCollider.ClosestPointOnBounds(hit.point)}. spawnLoc: {spawnLoc}. hitpoint: {hit.point}");

                // Collider col = preview.GetComponent<Collider>();
                // preview.layer = ignoreLayer;
                UpdateLayer(preview.transform, ignoreLayer);
                Collider[] results = new Collider[1];
                int hitCount = Physics.OverlapBoxNonAlloc(previewCollider.bounds.center, previewCollider.bounds.extents, results, Quaternion.identity, overlapLayerMask);
                // preview.layer = previewLayer;
                UpdateLayer(preview.transform, previewLayer);
                if (hitCount > 0)
                {
                    // preview.GetComponent<Renderer>().material = invalidPreviewMaterial;
                    UpdateMaterial(preview.transform, invalidPreviewMaterial);
                    return;
                }
                else
                {
                    // preview.GetComponent<Renderer>().material = previewMaterial;
                    UpdateMaterial(preview.transform, previewMaterial);
                }
            }
            if (Input.GetMouseButtonDown(0) && preview)
            {
                var spawned = Instantiate(selected.prefab, spawnLoc, spawnRot);
                spawned.gameObject.layer = canRemoveLayer;
                foreach (Transform child in spawned.transform)
                {
                    child.gameObject.layer = canRemoveLayer;
                }
                Deselect();
            }
        }
        else if(preview && preview.activeInHierarchy)
        {
            Debug.Log(hit.collider.gameObject.name);
            preview.SetActive(false);
        }
    }


    private void Deselect()
    {
        Destroy(preview);
        Destroy(measurement);
        preview = null;
        measurement = null;
        previewCollider = null;
        measureCollider = null;
        selected = null;
    }
}
