using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ShelfStocking : MonoBehaviour
{
    public Transform[] stockSpawnPoints;
    public GameObject[] stockItems;

    private void Start()
    {
        if (stockSpawnPoints == null || stockSpawnPoints.Length == 0)
            return;
        for (int i = 0; i < stockSpawnPoints.Length; i++)
        {
            if (stockItems == null || i >= stockItems.Length || stockItems[i] == null)
                continue;
            GameObject item = Instantiate(stockItems[i], stockSpawnPoints[i].position, stockSpawnPoints[i].rotation);
            item.transform.SetParent(stockSpawnPoints[i]);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (stockSpawnPoints == null || stockSpawnPoints.Length == 0)
            return;
        for (int i = 0; i < stockSpawnPoints.Length; i++)
        {
            Handles.color = (stockItems == null || i >= stockItems.Length || stockItems[i] == null) ? Color.white : Color.green;
            Handles.Label(stockSpawnPoints[i].position, i.ToString());
            Handles.ArrowHandleCap(0, stockSpawnPoints[i].position, stockSpawnPoints[i].rotation, 1, EventType.Repaint);
            if(Handles.color == Color.green)
                Handles.Label(stockSpawnPoints[i].position + Vector3.up * 0.2f, stockItems[i].name);
        }
    }
}
