using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCollision : MonoBehaviour
{
    #region Unity Methods
    public Vector3 boxSize = new Vector3(3, 2, 2);

    #endregion Unity Methods

    #region Unity Methods
    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }

    #endregion Unity Methods

    #region Helper Methods
    public Collider[] CheckOverlapBox(LayerMask layerMask)
    {
        return Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation, layerMask);
    }

    #endregion Helper Methods
}
