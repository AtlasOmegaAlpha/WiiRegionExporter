using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiiRegionExporter
{
    class Subregion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public Subregion(int id, string name, float latitude, float longitude)
        {
            this.Id = id;
            this.Name = name;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Subregion other)
                return this.Id == other.Id;
            return false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
