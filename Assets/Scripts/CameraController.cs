using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMoveSpeed = 5F;
    public MeshCollider floorCollider;
    public float floorColliderExtraMargin = 2F;
    
    void Update()
    {
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(axisX, 0, axisZ);
        movement = movement.normalized * (this.cameraMoveSpeed * Time.deltaTime);

        if (movement.magnitude > 0)
        {
            this.transform.position += movement;
            
            // Clamp the camera position within the floor collider bounds
            Vector2 xBounds = new Vector2(
                this.floorCollider.bounds.min.x - this.floorColliderExtraMargin,
                this.floorCollider.bounds.max.x + this.floorColliderExtraMargin
            );
            Vector2 zBounds = new Vector2(
                this.floorCollider.bounds.min.z - this.floorColliderExtraMargin,
                this.floorCollider.bounds.max.z + this.floorColliderExtraMargin
            );
            
            this.transform.position = new Vector3(
                Mathf.Clamp(this.transform.position.x, xBounds.x, xBounds.y),
                this.transform.position.y,
                Mathf.Clamp(this.transform.position.z, zBounds.x, zBounds.y)
            );
        }
    }
}
