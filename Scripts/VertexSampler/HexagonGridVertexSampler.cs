using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGridVertexSampler : MonoBehaviour
{
    private int layer;
    private float root3;
    private List<(float, float)> offsetList;

    public HexagonGridVertexSampler()
    {
        root3 = Mathf.Sqrt(3);
        offsetList = new List<(float, float)>
        {
            (0, 2),
            (root3, 1),
            (root3, -1),
            (0, -2),
            (-root3, -1),
            (-root3, 1)
        };
    }

    private void Traverse(Queue<GridVertex> queue, List<GridVertex> list)
    {
        if(queue.Count == 0)
        {
            return;
        }
        GridVertex origin = queue.Dequeue();
        if (origin.layer >= this.layer)
        {
            return;
        }
        foreach (var offset in offsetList)
        {
            GridVertex vertex = new GridVertex(origin.x + offset.Item1, origin.y + offset.Item2, origin.layer + 1);
            if (list.Contains(vertex))
            {
                continue;
            }
            queue.Enqueue(vertex);
            list.Add(vertex);
        }
        Traverse(queue, list);
    }

    public List<GridVertex> Generate(int layer)
    {
        this.layer = layer;
        Queue<GridVertex> queue = new Queue<GridVertex>();
        queue.Enqueue(new GridVertex(0.0f, 0.0f, 0));
        List<GridVertex> list = new List<GridVertex> { new GridVertex(0.0f, 0.0f, 0) };
        Traverse(queue, list);
        return list;

    }

}
