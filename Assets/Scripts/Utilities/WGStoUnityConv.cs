using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtraMath
{
    public struct LocationToMeters
    {
        public static double Lon(double lon)
        {
            var x = lon * 2 * Math.PI * 6378137 / 2 / 180;
            return x;
        }

        public static double Lat(double lat)
        {
            var y = Math.Log(Math.Tan((90 + lat) * Math.PI / 360)) / (Math.PI / 180);
            y = y * 2 * Math.PI * 6378137 / 2 / 180;
            return y;
        }
    }
}
