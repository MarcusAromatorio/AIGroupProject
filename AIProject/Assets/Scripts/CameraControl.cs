using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{

    public static bool zoomOnScroll = true;
    private Camera cam;

    private Vector2 prevMouse;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translate = new Vector3(0, 0, 1) * cam.orthographicSize;

        if (zoomOnScroll)
        {
            cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * -4f;
            if (cam.orthographicSize < 1f)
            {
                cam.orthographicSize = 1f;
            }
        }

        Vector2 mouse = Input.mousePosition;
        if (Input.GetMouseButton(1))
        {
            Vector2 delta = (prevMouse - mouse);
            translate = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f) * (new Vector3(delta.x, 0f, delta.y) * (cam.orthographicSize * 0.25f));

            transform.Translate(translate * Time.deltaTime, Space.World);
        }
        prevMouse = mouse;
    }
}
