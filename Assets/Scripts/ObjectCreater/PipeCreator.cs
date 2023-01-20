using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;

public class PipeCreator : MonoBehaviour
{
    public float thickness = 2f;
    public ShapeFileReader shapeFileReader = new ShapeFileReader();
    public DBFHandler dbfReader = new DBFHandler ();
    public string filename = "C:\\Users\\Fairee\\Desktop\\New folder\\TMISPRUBEH_L.shp";
    public bool isWGS = true;
    Collection<ShapeFileRecord> records;
    IEnumerable<Dictionary<string, object>> data;
    ShapeFileHeader header;

    Dictionary<string,string> Edata = new Dictionary<string, string>()
    {
        { "code", "CTMTP_KOD"},
        { "profile",  "PROFIL"},
        { "dateOfCreation", "DAT_VZNIK" },
        { "dateOfChange","DAT_ZMENA" },
        { "origin","PUVOD" },
        { "originNumber","PUVCIS" },
        { "year","ROK" },
        { "description","CTMTP_POPI" },
        { "shapeLenght","SHAPE_Leng" }
    };

    /// <summary>
    /// When called it creates all the Pipes based on the Records
    /// </summary>
    public void generate() {
        shapeFileReader.ConvertFromWGS = isWGS;
        shapeFileReader.Read(filename);
        dbfReader.Read(filename);
        records = shapeFileReader.Records;
        header = shapeFileReader.FileHeader;
        data = dbfReader.data;
        if (records == null) {
            Debug.Log("THERE ARE NO SHAPEFILE RECORDS");
        }
        if (data == null) {
            Debug.Log("THERE ARE NO DBF DATA");
            return;
        }
        if (records.Count == 0)
        {
            Debug.Log("There are no records to generate");
        }
        else {
            int index = 0;
            GameObject pipesHeader = GameObject.Find("pipes");
            if (pipesHeader == null)
            {
                pipesHeader = new GameObject();
                pipesHeader.name = "pipes";
            }
            foreach (Dictionary<string, object> dic in data) {
                CreateNewPipe(index++, pipesHeader.transform, dic);
            }
        }
    }


    private void SetPipeData(Dictionary<string, object> dic, Pipe pipe) {
        string a = Edata[nameof(pipe.dateOfChange)];
        pipe.code = Convert.ToInt32((float)dic[Edata[nameof(pipe.code)]]);
        pipe.dateOfChange = (DateTime)dic[Edata[nameof(pipe.dateOfChange)]];
        pipe.dateOfCreation = (DateTime)dic[Edata[nameof(pipe.dateOfCreation)]];
        pipe.description = (string)dic[Edata[nameof(pipe.description)]];
        pipe.origin = (string)dic[Edata[nameof(pipe.origin)]];
        Type v = dic[Edata[nameof(pipe.shapeLenght)]].GetType();
        pipe.shapeLenght = (float)dic[Edata[nameof(pipe.shapeLenght)]];
        pipe.profile = (float)dic[Edata[nameof(pipe.profile)]];
        pipe.originNumber = Convert.ToInt32((float)dic[Edata[nameof(pipe.originNumber)]]);
        pipe.year = Convert.ToInt32((float)dic[Edata[nameof(pipe.year)]]);
    }

    /// <summary>
    /// Creates new gameobject and sets all the necessary components
    /// </summary>
    private void CreateNewPipe(int index, Transform parent, Dictionary<string, object> dic) {
        GameObject newPipe = new GameObject();
        Pipe pipeComponent = newPipe.AddComponent<Pipe>();
        SetPipeData(dic, pipeComponent);
        pipeComponent.id = index;
        newPipe.name = index.ToString();
        newPipe.transform.position = new Vector3((float)records[index].XMin - (float)header.XMin, 0, (float)records[index].YMin - (float)header.YMin);
        GameObject head = GameObject.Find(pipeComponent.code.ToString());
        MeshFilter meshFilter = newPipe.AddComponent<MeshFilter>();
        MeshCollider meshCollider = newPipe.AddComponent<MeshCollider>();
        MeshRenderer meshRenderer = newPipe.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Diffuse"));
        makeMeshData(index, meshFilter);

        if (head == null)
        {
            head = new GameObject();
            head.transform.SetParent(parent.transform);
            head.name = pipeComponent.code.ToString();
        }
        newPipe.transform.SetParent(head.transform);
    }


    /// <summary>
    /// Finds a point to the left from given vector pendpedicular of the direction
    /// the distance of the point is set by the variable thickness which is given as public class parameter
    /// </summary>

    private Vector2 FindLeftPoint(Vector2 point, Vector2 direction) {       
        Vector2 leftnormal = new Vector2(direction.y, -direction.x);
        leftnormal.Normalize();
        Vector2 leftPoint = point + thickness * leftnormal;
        return leftPoint;
    }
    /// <summary>
    /// Finds a point to the right from given vector pendpedicular of the direction
    /// the distance of the point is set by the variable thickness which is given as public class parameter
    /// </summary>

    private Vector2 FindRightPoint(Vector2 point, Vector2 direction)
    {
        Vector2 rightnormal = new Vector2(-direction.y, direction.x);
        rightnormal.Normalize();
        Vector2 rightPoint = point + thickness * rightnormal;
        return rightPoint;
    }

    /// <summary>
    /// Finds a line from point and a direction
    /// returns a Vector(a,b,c) ... ax+by+c=0
    /// </summary>
    private Vector3 FindLine(Vector2 point, Vector2 direction) {      
        Vector2 normal = new Vector2(direction.y, -direction.x);
        float C = -normal.x * point.x - normal.y * point.y;
        Vector3 line = new Vector3(normal.x, normal.y, C);
        return line;
    }

    /// <summary>
    /// Finds intersection between two lines
    /// lines are given as A vector (a,b,c) ... ax+by+c=0
    /// </summary>
    private Vector2 FindIntersection(Vector3 lineA, Vector3 lineB) {

        //check if line A is parallel with y-axis
        if (lineA.x == 0)
        {
            if (lineA.y == 0)
            {
                return new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            }
            float y = -lineA.z / lineA.y;
            float x = 0;
            //check if line B is parallel with y-axis (if yes there is no intersection)
            if (lineB.x == 0)
            {
                return new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            }
            else
            {
                x = (-lineB.y * y - lineB.z) / lineB.x;
            }
            return new Vector2(x, y);
        }
        //check if line B is parallel with y-axis
        else if (lineB.x == 0) {
            if (lineB.y == 0)
            {
                return new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            }
            float y = -lineB.z / lineB.y;
            float x = 0;
            //check if line A is parallel with y-axis (if yes there is no intersection)
            if (lineA.x == 0)
            {
                return new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            }
            else
            {
                x = (-lineA.y * y - lineA.z) / lineA.x;
            }
            return new Vector2(x, y);
        }        
        //check if the lines are parallel
        else if (lineA.x*lineB.y/lineB.x - lineA.y == 0)
        {
            return new Vector2(float.NegativeInfinity, float.NegativeInfinity);
        }
        else
        {
            float y = (-lineA.z + lineA.x * lineB.z / lineB.x) / (lineA.y - lineA.x * lineB.y / lineB.x);
            float x = (-lineA.y * y - lineA.z) / lineA.x;
            Vector2 point = new Vector2(x, y);
            return point;
        }
    }

    /// <summary>
    /// Based on the points given by the data creates a plane mesh
    /// </summary>
    private void makeMeshData(int index, MeshFilter filter)
    {
        List<Vector4D> pointsD = records[index].Points;
        List<Vector4> points = new List<Vector4>();
        foreach (Vector4D v in pointsD) {
            points.Add(new Vector4((float)(v.x), (float)(v.y), (float)(v.z), (float)(v.w)));
        }
        int pointCount = records[index].NumberOfPoints;

        //TEST
        /*points.Clear();
        points.Add(new Vector4(0, 0, 0, 0));
        points.Add(new Vector4(1, 0.9f, 0, 0));
        points.Add(new Vector4(1.8f, 2f, 0, 0));
        pointCount = 3;*/


        int numberOfVertices = pointCount * 2;
        int numberOfTriangles = 2 * (pointCount - 1);
        List<Vector3> vertices = new List<Vector3>();
        //Vector3[] vertices = new Vector3[numberOfVertices];
        int[] triangles = new int[numberOfTriangles * 3];
        Vector3[] normals = new Vector3[numberOfTriangles];

        if (pointCount < 2) {
            return;
        }

        //Find the first two points there as there is no necesity to compute intersection of lines for them
        Vector2 direction = points[1] - points[0];
        Vector2 left = FindLeftPoint(points[0], direction);
        Vector2 right = FindRightPoint(points[0], direction);
        vertices.Add(new Vector3(left.x, 0, left.y));
        vertices.Add(new Vector3(right.x, 0, right.y));



        //FIND THE POINTS
        for (int i = 0; i < pointCount - 1; i++)
        {
            if (i + 2 < pointCount)
            {
                Vector2 directionNear = points[i + 1] - points[i];
                Vector2 directionFar = points[i + 2] - points[i + 1];
                //Find the two points on the left
                Vector2 leftNear = FindLeftPoint(points[i], directionNear);
                Vector2 leftFar = FindLeftPoint(points[i + 2], directionFar);
                //Find lines going through the points
                Vector3 leftlineNear = FindLine(leftNear, directionNear);
                Vector3 leftlineFar = FindLine(leftFar, directionFar);
                //Find the intersection of these lines and thus get the left point
                Vector2 leftIntersection = FindIntersection(leftlineNear, leftlineFar);

                //leftIntersection is negative infinity when the two lines are parallel
                if (leftIntersection.x == float.NegativeInfinity)
                {
                    direction = points[i+1] - points[i];
                    left = FindLeftPoint(points[i+1], direction);
                    right = FindRightPoint(points[i+1], direction);
                    vertices.Add(new Vector3(left.x, 0, left.y));
                    vertices.Add(new Vector3(right.x, 0, right.y));
                }
                else
                {
                    //Find the two points on the right
                    Vector2 rightNear = FindRightPoint(points[i], directionNear);
                    Vector2 rightFar = FindRightPoint(points[i + 2], directionFar);
                    //Find lines going through the points
                    Vector3 rightlineNear = FindLine(rightNear, directionNear);
                    Vector3 rightlineFar = FindLine(rightFar, directionFar);
                    //Find the intersection of these lines and thus get the right point
                    Vector2 rightIntersection = FindIntersection(rightlineNear, rightlineFar);

                    //rightIntersection is negativeinfinity when the two lines are parallel
                    //SHOULDNT BE NECESSARY AS IF THE LINES ARE PARALLEL THERE WILL BE NO LEFT INTERSECTION
                    if (rightIntersection.x == float.NegativeInfinity)
                    {
                        direction = points[i+1] - points[i];
                        left = FindLeftPoint(points[i + 1], direction);
                        right = FindRightPoint(points[i + 1], direction);
                        vertices.Add(new Vector3(left.x, 0, left.y));
                        vertices.Add(new Vector3(right.x, 0, right.y));
                    }


                    vertices.Add( new Vector3(leftIntersection.x, 0, leftIntersection.y));
                    vertices.Add( new Vector3(rightIntersection.x, 0, rightIntersection.y));
                }
            }
            else
            {
                //find the last two points 
                direction = points[i + 1] - points[i];
                Vector2 leftFar = FindLeftPoint(points[i + 1], direction);
                Vector2 rightFar = FindRightPoint(points[i + 1], direction);
                vertices.Add( new Vector3(leftFar.x,0,leftFar.y));
                vertices.Add( new Vector3(rightFar.x, 0, rightFar.y));
            }
        }

        //SET TRIANGLES
        for (int i = 0; i < pointCount - 1; i++)
        {
            triangles[i * 6]=(i * 2);
            triangles[i * 6 + 1]=(i * 2 + 1);
            triangles[i * 6 + 2]=(i * 2 + 2);
            triangles[i * 6 + 3]=(i * 2 + 2);
            triangles[i * 6 + 4]=(i * 2 + 1);
            triangles[i * 6 + 5]=(i * 2 + 3);
        }

        //Set normals
        for (int i = 0; i < numberOfTriangles; i++) {
            normals[i] = Vector3.up;
        }
        Mesh m = new Mesh();
        m.vertices = vertices.ToArray();
        m.triangles = triangles;
        m.name = "Pipe" + index.ToString();
        filter.mesh = m;


    }
}
