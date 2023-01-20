using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeFileHeader
{

    public string boundBox;

    #region Private
    private int code;
    private int length;
    private int version;
    private int shapeType;

    private double xMin = 0;
    private double yMin = 0;
    private double zMin = 0;
    private double mMin = 0;

    private double xMax = 0;
    private double yMax = 0;
    private double zMax = 0;
    private double mMax = 0;

    #endregion Private

    public ShapeFileHeader() {}

    public override string ToString()
    {
        string s = string.Format("This shape file has: Code: {0}, Lenght: {1}, Version: {2}, ShapeType: {3}, xMin: {4}, xMax: {5}, yMin: {6}, yMax: {7}" +
            " zMin: {8}, zMax: {9}, mMin: {10}, mMax: {11}", code, length, version, shapeType, xMin, xMax, yMin, yMax, zMin, zMax, mMin, mMax);
        return base.ToString();
    }

    #region Get/Set

    public int FileCode {
        get { return code; }
        set { code = value; }
    }

    public int FileLength
    {
        get { return length; }
        set { length = value; }
    }

    public int Version
    {
        get { return version; }
        set { version = value; }
    }

    public int ShapeType
    {
        get { return shapeType; }
        set { shapeType = value; }
    }

    public double XMin
    {
        get { return xMin; }
        set { xMin = value; }
    }

    public double YMin
    {
        get { return yMin; }
        set { yMin = value; }
    }

    public double ZMin
    {
        get { return zMin; }
        set { zMin = value; }
    }

    public double MMin
    {
        get { return mMin; }
        set { mMin = value; }
    }

    public double XMax
    {
        get { return xMax; }
        set { xMax = value; }
    }

    public double YMax
    {
        get { return yMax; }
        set { yMax = value; }
    }

    public double ZMax
    {
        get { return zMax; }
        set { zMax = value; }
    }

    public double MMax
    {
        get { return mMax; }
        set { mMax = value; }
    }

    #endregion Get/Set
}
