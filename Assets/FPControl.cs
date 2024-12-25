using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class FPControl : MonoBehaviour
{
    CharacterController characterController;
    public Transform camTransform;
    public Transform targetTransform;
    public float moveSpeed;
    public float rotSpeed;
    float rotVertical;

    public Transform firePoint;
    public GameObject BulletPrefab;
    public LayerMask hitLayers;
    public float minHitDistance;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CursorLockMode cursorLockMode = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        camTransform.position = targetTransform.position;
        camTransform.rotation = targetTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float horizon = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        var moveDirection = transform.right * horizon + transform.forward * vertical;
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        camTransform.position = targetTransform.position;

        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        rotVertical = rotVertical - (mouseY * rotSpeed *  Time.deltaTime);
        if (rotVertical > 50f)
            rotVertical = 50f;
        else if (rotVertical < -40f)
            rotVertical = -40f;

        transform.Rotate(0, mouseX, 0);
        targetTransform.forward = transform.forward;
        targetTransform.Rotate(rotVertical, 0, 0, Space.Self);
        
        camTransform.forward = targetTransform.forward;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 targetPos;
            Vector3 finalPos = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(camTransform.position, camTransform.forward,  out hit, 100f, hitLayers))
            {
                var hitDistance = hit.distance;
                if (hitDistance < minHitDistance)
                    hitDistance = minHitDistance;
                targetPos = camTransform.position + camTransform.forward * hitDistance;
            }
            else
                targetPos = camTransform.position + camTransform.forward * 100f;

            Bullet bullet = Instantiate(BulletPrefab).GetComponent<Bullet>();
            bullet.Shot(firePoint.position, targetPos);
        }
    }
}
