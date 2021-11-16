using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonGraphVertexSampler
{
    private int hexTraverseLayer;
    private float root3;
    private List<(float, float)> offsetList;

    public HexagonGraphVertexSampler()
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

    private void Traverse(Queue<GraphVertex> queue, List<GraphVertex> list)
    {
        if(queue.Count == 0)
        {
            return;
        }
        GraphVertex origin = queue.Dequeue();
        if (origin.hexTraverseLayer >= this.hexTraverseLayer)
        {
            return;
        }
        foreach (var offset in offsetList)
        {
            GraphVertex vertex = new GraphVertex(origin.x + offset.Item1, origin.y + offset.Item2);
            vertex.hexTraverseLayer = origin.hexTraverseLayer + 1;
            if (list.Contains(vertex))
            {
                continue;
            }
            queue.Enqueue(vertex);
            list.Add(vertex);
        }
        Traverse(queue, list);
    }

    public List<GraphVertex> Generate(int hexTraverseLayer)
    {
        this.hexTraverseLayer = hexTraverseLayer;
        Queue<GraphVertex> queue = new Queue<GraphVertex>();
        queue.Enqueue(new GraphVertex(0.0f, 0.0f, 0));
        List<GraphVertex> list = new List<GraphVertex> { new GraphVertex(0.0f, 0.0f, 0) };
        Traverse(queue, list);
        return CommonUtils.Deduplicate(list);

    }
}
