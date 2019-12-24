using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;
    private bool follow = true;

    private void LateUpdate()
    {
        if (target == null || !follow)
            return;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position + offset, 14 * Time.deltaTime);
    }

    public IEnumerator Shake(float magnitude, float duration)
    {
        float elapsed = 0;
        follow = false;
        float z = transform.position.z;
        while (elapsed <= duration && !target.GetComponent<PlayerController>().dead)
        {
            float x = target.transform.position.x + Random.Range(-1f, 1f) * magnitude;
            float y = target.transform.position.y + Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(x, y, z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        follow = true;
    }
}
