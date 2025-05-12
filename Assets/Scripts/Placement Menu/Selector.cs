using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Selector : MonoBehaviour
{
    private Placeable selected = null;
    private GameObject preview = null;

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
    
    
    public void Select(Placeable selected)
    {
        this.selected = selected;
        if (preview)
        {
            Destroy(preview);
        }
        
        preview = Instantiate(selected.prefab, Vector3.zero, Quaternion.identity);
        var renderer = preview.GetComponent<Renderer>();
        renderer.material = previewMaterial;
        preview.layer = previewLayer;
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
                Destroy(hit.collider.gameObject);
            }
        }else if (hit.collider && hit.collider.gameObject.layer == placeLayer)
        {
            var spawnLoc = spawnDistance*hit.normal + hit.point;
            
            if(preview)
            {
                if (!preview.activeInHierarchy)
                {
                    preview.SetActive(true);
                }
                preview.transform.position = spawnLoc;
                Collider col = preview.GetComponent<Collider>();
                preview.layer = ignoreLayer;
                Collider[] results = new Collider[1];
                int hitCount = Physics.OverlapBoxNonAlloc(col.bounds.center, col.bounds.extents, results, Quaternion.identity, overlapLayerMask);
                preview.layer = previewLayer;
                if (hitCount > 0)
                {
                    preview.GetComponent<Renderer>().material = invalidPreviewMaterial;
                    return;
                }
                else
                {
                    preview.GetComponent<Renderer>().material = previewMaterial;
                }
            }
            if (Input.GetMouseButtonDown(0) && selected)
            {
                Instantiate(selected.prefab, spawnLoc, Quaternion.identity).layer = canRemoveLayer;
                Deselect();
            }
        }
        else if(preview && preview.activeInHierarchy)
        {
            preview.SetActive(false);
        }
    }

    private void Deselect()
    {
        Destroy(preview);
        preview = null;
        selected = null;
    }
}
