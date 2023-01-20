using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3D
{
    public double x, y, z;

    public Vector3D() { }

    public Vector3D(double a, double b, double c)
    {
        x = a;
        y = b;
        z = c;
    }

    public static Vector3D operator +(Vector3D A, Vector3D B)
    {
        Vector3D ret = new Vector3D();
        ret.x = A.x + B.x;
        ret.y = A.y + B.y;
        ret.z = A.z + B.z;
        return ret;
    }

    public static Vector3D operator -(Vector3D A, Vector3D B)
    {
        Vector3D ret = new Vector3D();
        ret.x = A.x - B.x;
        ret.y = A.y - B.y;
        ret.z = A.z - B.z;
        return ret;
    }


    public Vector3 ToFloat()
    {
        return new Vector3((float)x, (float)y, (float)z);
    }

    public Vector3 ToFloatWithSwap() {

        return new Vector3((float)x, (float)z, (float)y);
    }

}
