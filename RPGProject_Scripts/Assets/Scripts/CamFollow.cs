using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform player; // Posição atual do player a ser referenciada na Unity
    public Vector3 _offset; // Posição da camera a ser configurada manualmente na Unity

    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 10f;

    public float pitch = 2f; // velocidade que a camera vai olhar pro player

    private float currentZoom = 10f;

    // Update is called once per frame
    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    private void LateUpdate()
    {
        transform.position = player.position - _offset * currentZoom;
        transform.LookAt(player.position + Vector3.up * pitch);
    }
}
