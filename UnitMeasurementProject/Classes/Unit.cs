using System;
using System.Collections.Generic;
using System.Text;

namespace UnitMeasurementProject.Classes
{
    public struct Unit
    {
        internal string Name;
        internal string Abbreviation;
        internal double Scale;
        internal UnitType Type;
        internal Localisation Localisation;
        internal Unit(UnitType type, string name, string abbreviation, Localisation localisation, double scale = 1 )
        {
            Name = name;
            Abbreviation = abbreviation;
            Scale = scale;
            Type = type;
            Localisation = localisation;
        }

        public override string ToString()
        {
            return $"{Name} ({Abbreviation})";
        }
    }

    internal enum UnitType
    {
        Weight,
        Distance,
        Volume
    }
    //Not used in the end, but usefull for sorting values depending on the location of the user
    internal enum Localisation
    {
        US,
        RestOfTheWorld
    }
}
