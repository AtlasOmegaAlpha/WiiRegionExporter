using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiiRegionExporter
{
    class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Subregion> subregions;

        public Region(int id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.subregions = new List<Subregion>();
        }

        public override bool Equals(object? obj)
        {
            if (obj is Region other)
                return this.Id == other.Id && this.Name == other.Name;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Id, this.Name);
        }
    }
}
