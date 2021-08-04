public class Polygon: MonoBehaviour
{

    private Vertex _center;
    private List<Vertex> _vertexList;

    public Vertex center
    {
        set
        {
            this._center = value;
        }
        get
        {
            return this._center;
        }
    }

    public List<Vertex> vertexList
    {
        set
        {
            this._vertexList = value;
        }
        get
        {
            return this._vertexList;
        }
    }

    public Polygon(Vertex center, List<Vertex> vertexList)
    {
        this.center = center;
        this.vertexList = vertexList;
    }
    
}