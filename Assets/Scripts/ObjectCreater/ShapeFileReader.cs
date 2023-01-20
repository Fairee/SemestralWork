using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using System.Collections.ObjectModel;

/// <summary>
/// PARTIALY TAKEN FROM https://github.com/CharlotteAndre/ShapeFileForUnity/tree/master/Assets/Projet
/// </summary>

public class ShapeFileReader
{
    private static byte[] intBytes = new byte[4];
    private static byte[] doubleBytes = new byte[8];
    private const int expectedFileCode = 9994;
    private bool moveToCenter = true;
    private bool convertFromWGS = true;


    public enum ShapeType {
        /// <summary>
        /// Nullshape / placeholder record.
        /// </summary>
        NullShape = 0,

        /// <summary>
        /// Point record, for defining point locations such as a city.
        /// </summary>
        //Point = 1,

        /// <summary>
        /// One or more sets of connected points. Used to represent roads,
        /// hydrography, etc.
        /// </summary>
        //PolyLine = 3,

        /// <summary>
        /// One or more sets of closed figures. Used to represent political
        /// boundaries for countries, lakes, etc.
        /// </summary>
        //Polygon = 5,

        /// <summary>
        /// A cluster of points represented by a single shape record.
        /// </summary>
        //Multipoint = 8,

        // Unsupported types:
        //PointZ = 11,
        PolyLineZ = 13
        // PolygonZ = 15,
        // MultiPointZ = 18,
        // PointM = 21,
        // PolyLineM = 23,
        // PolygonM = 25,
        // MultiPointM = 28,
        // MultiPatch = 31

    }


    private ShapeFileHeader header = new ShapeFileHeader();
    private Collection<ShapeFileRecord> records = new Collection<ShapeFileRecord>();

    #region Properties

    public ShapeFileHeader FileHeader
    {
        get { return this.header; }
    }

    public bool ConvertFromWGS
    {
        get { return this.convertFromWGS; }
        set { this.convertFromWGS = value; }
    }

    public Collection<ShapeFileRecord> Records
    {
        get { return this.records; }
    }
    #endregion Properties

    #region PublicMethods
    /// <summary>
    /// Opens a shapefile and call necessary methods to read it
    /// </summary>
    public void Read(string fileName) {
        if (string.IsNullOrEmpty(fileName)) {
            throw new ArgumentNullException("FileName");
        }

        using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            this.ReadShapeFileHeader(stream);
            this.records.Clear();
            while (true) {
                try
                {
                    this.ReadShapeFileRecord(stream);
                }
                catch (IOException){
                    break;
                }
            
            }
        }
        if (moveToCenter) {
            AdjustPosition();
        }
    }

    /// <summary>
    /// Read and process the shapefile header
    /// </summary>
    public void ReadShapeFileHeader(Stream stream) {
        this.header.FileCode = ShapeFileReader.ReadInt32_BE(stream);
        if (this.header.FileCode != ShapeFileReader.expectedFileCode)
        {
            string msg = String.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid FileCode encountered. Expecting {0}.", ShapeFileReader.expectedFileCode);
            throw new ArgumentException(msg);
        }

        // 5 unused values.
        stream.Seek(24, SeekOrigin.Begin);

        // File Length.
        this.header.FileLength = ShapeFileReader.ReadInt32_BE(stream);

        // Version.
        this.header.Version = ShapeFileReader.ReadInt32_LE(stream);

        // Shape Type.
        this.header.ShapeType = ShapeFileReader.ReadInt32_LE(stream);

        // Bounding Box.
        this.header.XMin = ShapeFileReader.ReadDouble64_LE(stream);
        this.header.YMin = ShapeFileReader.ReadDouble64_LE(stream);
        this.header.XMax = ShapeFileReader.ReadDouble64_LE(stream);
        this.header.YMax = ShapeFileReader.ReadDouble64_LE(stream);



        this.header.ZMin = ShapeFileReader.ReadDouble64_LE(stream);
        this.header.ZMax = ShapeFileReader.ReadDouble64_LE(stream);
        this.header.MMin = ShapeFileReader.ReadDouble64_LE(stream);
        this.header.MMax = ShapeFileReader.ReadDouble64_LE(stream);
    }
    /// <summary>
    /// Read the shapeFileRecord 
    /// </summary>
    public void ReadShapeFileRecord(Stream stream) {
        ShapeFileRecord record = new ShapeFileRecord();

        // Record Header.

        record.RecordNumber = ShapeFileReader.ReadInt32_BE(stream);
        record.ContentLength = ShapeFileReader.ReadInt32_BE(stream);


        // Shape Type.
        record.ShapeType = ShapeFileReader.ReadInt32_LE(stream);

        switch (record.ShapeType) {
            case (int)ShapeType.NullShape:
                break;
            case (int)ShapeType.PolyLineZ:
                ShapeFileReader.ReadPolyLineZ(stream, record);
                break;
            default:
                Debug.Log(String.Format("ShapeType {0} not supported", (int)record.ShapeType));
                break;
        }
        this.records.Add(record);
    }



    #endregion PublicMethods

    #region PrivateMethods

    /// <summary>
    /// Subtract the records mins from all the points
    /// </summary>
    private void AdjustPosition()
    {
        
        if (convertFromWGS)
        {
            double safeX = header.XMin;
            header.XMin = ExtraMath.LocationToMeters.Lat(header.YMin);
            header.YMin = ExtraMath.LocationToMeters.Lon(safeX);
            int index = 0;
            foreach (ShapeFileRecord record in records)
            {
                index++;
                safeX = record.XMin;
                record.XMin = ExtraMath.LocationToMeters.Lat(record.YMin);
                record.YMin = ExtraMath.LocationToMeters.Lon(safeX);
                for (int i = 0; i < record.Points.Count; i++)
                {
                    safeX = record.Points[i].x;
                    record.Points[i].x = ExtraMath.LocationToMeters.Lat(record.Points[i].y) - record.XMin;
                    record.Points[i].y = ExtraMath.LocationToMeters.Lon(safeX) - record.YMin;
                }
            }
        }
        else
        {
            int index = 0;
            foreach (ShapeFileRecord record in records)
            {
                index++;
                for (int i = 0; i < record.Points.Count; i++)
                {
                    record.Points[i].x = record.Points[i].x - record.XMin;
                    record.Points[i].y = record.Points[i].y - record.YMin;
                }
            }
        }
    }
    /// <summary>
    /// Read a 32Int big endian
    /// </summary>
    private static int ReadInt32_BE(Stream stream)
    {
        for (int i = 3; i >= 0; i--)
        {
            int b = stream.ReadByte();
            if (b == -1)
                throw new EndOfStreamException();
            intBytes[i] = (byte)b;

        }

        return BitConverter.ToInt32(intBytes, 0);
    }

    /// <summary>
    /// Read a 32Int little endian
    /// </summary>
    private static int ReadInt32_LE(Stream stream)
    {
        for (int i = 0; i < 4; i++)
        {
            int b = stream.ReadByte();
            if (b == -1)
                throw new EndOfStreamException();
            intBytes[i] = (byte)b;
        }

        return BitConverter.ToInt32(intBytes, 0);
    }
    /// <summary>
    /// Read a 64Double little endian
    /// </summary>
    private static double ReadDouble64_LE(Stream stream)
    {
        for (int i = 0; i < 8; i++)
        {
            int b = stream.ReadByte();
            if (b == -1)
                throw new EndOfStreamException();
            doubleBytes[i] = (byte)b;
        }

        return BitConverter.ToDouble(doubleBytes, 0);
    }
    /// <summary>
    /// Reads polyLineZ from the stream
    /// </summary>
    private static void ReadPolyLineZ(Stream stream, ShapeFileRecord record)
    {
        // current position - 4 for the header
        long pos = stream.Position - 4;
        record.XMin = ShapeFileReader.ReadDouble64_LE(stream);
        record.YMin = ShapeFileReader.ReadDouble64_LE(stream);

        record.XMax = ShapeFileReader.ReadDouble64_LE(stream);
        record.YMax = ShapeFileReader.ReadDouble64_LE(stream);

        int numParts = ShapeFileReader.ReadInt32_LE(stream);
        int numPoints = ShapeFileReader.ReadInt32_LE(stream);

        // Parts.
        for (int i = 0; i < numParts; i++)
        {
            record.Parts.Add(ShapeFileReader.ReadInt32_LE(stream));
        }
        // Points (x + y values)
        for (int i = 0; i < numPoints; i++)
        {
            Vector4D p = new Vector4D();
            p.x = (float)ShapeFileReader.ReadDouble64_LE(stream);
            p.y = (float)ShapeFileReader.ReadDouble64_LE(stream);
            p.z = 0;
            p.w = 0;
            record.Points.Add(p);
        }

        // Points Z stuff
        record.ZMin = ShapeFileReader.ReadDouble64_LE(stream);
        record.ZMax = ShapeFileReader.ReadDouble64_LE(stream);

        for (int i = 0; i < numPoints; i++)
        {
            double z = ShapeFileReader.ReadDouble64_LE(stream);
            record.Points[i].z = z;
        }

        //Check if the M values should be read too
        long pos2 = stream.Position;
        if (Math.Abs(pos - pos2) / 2 < record.ContentLength)
        {
            record.MMin = ShapeFileReader.ReadDouble64_LE(stream);
            record.MMax = ShapeFileReader.ReadDouble64_LE(stream);
            for (int i = 0; i < numPoints; i++)
            {
                double m = ShapeFileReader.ReadDouble64_LE(stream);
                record.Points[i].w = m;
            }
        }
        //check if the current stream position is correct
        long pos3 = stream.Position;
        if (pos3 != pos + record.ContentLength*2) {
            Debug.Log("The stream isn't on the expected position after reading PolyLineZ");
        }

        return;
    }


    #endregion PrivateMethods
}
