using System;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private float fov;
    private float viewDistance;
    [SerializeField] private Vector3 origin;
    private float startingAngle;
    private string playerName = "Player";
    private float lastSeenPlayer;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 120f;
        viewDistance = 25f;
        origin = Vector3.zero;
        lastSeenPlayer = 10;
    }

    private void LateUpdate()
    {
        lastSeenPlayer += Time.deltaTime;

        int rayCount = 25;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            // Player hitting


            Vector3 vertex;
            Vector3 vector = new Vector3(GetVectorFromAngle(angle).x, 0, GetVectorFromAngle(angle).y);
            RaycastHit hit;
            RaycastHit playerHit;
            Physics.Raycast(origin, vector, out hit, viewDistance, layerMask);
            Physics.Raycast(origin, vector, out playerHit, viewDistance);
            Debug.DrawRay(origin, vector * 3);
            if (hit.collider == null)
            {
                // No hit
                vertex = origin + vector * viewDistance;
            }
            else
            {
                // Hit object
                vertex = hit.point;
            }

            if (playerHit.collider != null)
            {
                if (playerHit.transform.gameObject.name == playerName)
                {
                    Debug.Log("seeing player");
                    lastSeenPlayer = 0;
                }
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    public void SetFoV(float fov)
    {
        this.fov = fov;
    }

    public void SetViewDistance(float viewDistance)
    {
        this.viewDistance = viewDistance;
    }

    public float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public void SetPlayerName(String name)
    {
        playerName = name;
    }

    public float getLastSeenPlayer()
    {
        return lastSeenPlayer;
    }
}