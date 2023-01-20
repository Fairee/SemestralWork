using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using System.Collections.ObjectModel;

public class DBFHandler
{
    public IEnumerable<Dictionary<string, object>> data;
    #region PublicMethods
    /// <summary>
    /// Opens a shapefile and call necessary methods to read it
    /// </summary>
    public void Read(string fileName)
    {
        fileName = fileName.Replace(".shp", ".dbf");

        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException("FileName");
        }

        using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            DBFReader reader = new DBFReader(stream, Encoding.UTF8);
             data = reader.ReadToDictionary();
        }
    }
    #endregion //PublicMethods
}
