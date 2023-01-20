using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public class ShapeFileRecord
{

    #region Private
    private int recordNumber;
    private int contentLength;
    private int shapeType;

    private double xMin = 0;
    private double yMin = 0;
    private double zMin = 0;
    private double mMin = 0;

    private double xMax = 0;
    private double yMax = 0;
    private double zMax = 0;
    private double mMax = 0;

    private Collection<int> parts = new Collection<int>();
    private List<Vector4D> points = new List<Vector4D>();
    #endregion Private
    #region Properties
    /// <summary>
    /// Indicates the record number (or index) which starts at 1.
    /// </summary>
    public int RecordNumber
    {
        get { return this.recordNumber; }
        set { this.recordNumber = value; }
    }

    /// <summary>
    /// Specifies the length of this shape record in 16-bit words.
    /// </summary>
    public int ContentLength
    {
        get { return this.contentLength; }
        set { this.contentLength = value; }
    }

    /// <summary>
    /// Specifies the shape type for this record.
    /// </summary>
    public int ShapeType
    {
        get { return this.shapeType; }
        set { this.shapeType = value; }
    }

    /// <summary>
    /// Indicates the minimum x-position of the bounding
    /// box for the shape (expressed in degrees longitude).
    /// </summary>
    public double XMin
    {
        get { return this.xMin; }
        set { this.xMin = value; }
    }

    /// <summary>
    /// Indicates the minimum y-position of the bounding
    /// box for the shape (expressed in degrees latitude).
    /// </summary>
    public double YMin
    {
        get { return this.yMin; }
        set { this.yMin = value; }
    }

    /// <summary>
    /// Indicates the minimum z-position of the bounding
    /// box for the shape (expressed in degrees latitude).
    /// </summary>
    /// 


    public double ZMin
    {
        get { return this.zMin; }
        set { this.zMin = value; }
    }

    public double MMin
    {
        get { return this.mMin; }
        set { this.mMin = value; }
    }
    /// <summary>
    /// Indicates the maximum x-position of the bounding
    /// box for the shape (expressed in degrees longitude).
    /// </summary>
    public double XMax
    {
        get { return this.xMax; }
        set { this.xMax = value; }
    }

    /// <summary>
    /// Indicates the maximum y-position of the bounding
    /// box for the shape (expressed in degrees latitude).
    /// </summary>
    public double YMax
    {
        get { return this.yMax; }
        set { this.yMax = value; }
    }

    /// <summary>
    /// Indicates the maximum z-position of the bounding
    /// box for the shape (expressed in degrees latitude).
    /// </summary>
    public double ZMax
    {
        get { return this.zMax; }
        set { this.zMax = value; }
    }

    public double MMax
    {
        get { return this.mMax; }
        set { this.mMax = value; }
    }

    /// <summary>
    /// Indicates the number of parts for this shape.
    /// A part is a connected set of points, analogous to
    /// a PathFigure in WPF.
    /// </summary>
    public int NumberOfParts
    {
        get { return this.parts.Count; }
    }

    /// <summary>
    /// Specifies the total number of points defining
    /// this shape record.
    /// </summary>
    public int NumberOfPoints
    {
        get { return this.points.Count; }
    }

    /// <summary>
    /// A collection of indices for the points array.
    /// Each index identifies the starting point of the
    /// corresponding part (or PathFigure using WPF
    /// terminology).
    /// </summary>
    public Collection<int> Parts
    {
        get { return this.parts; }
    }

    /// <summary>
    /// A collection of all of the points defining the
    /// shape record.
    /// </summary>
    public List<Vector4D> Points
    {
        get { return this.points; }
    }
    #endregion Properties

    public ShapeFileRecord() { }


}
