using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DiscogsArtistReader
{
    public class Artist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string IndexedName { get; set; }
        public string RealName { get; set; }
        public string Profile { get; set; }

        [NotMapped]
        public List<int> Members
        {
            get
            {
                return InternalMembers.Split(';').Select(x => int.Parse(x)).ToList();
            }
            set
            {
                this.InternalMembers = string.Join(";", value.Select(x => x.ToString()).ToArray());
            }
        }
        public string InternalMembers { get; set; }

        [NotMapped]
        public List<string> NameVariations
        {
            get
            {
                return InternalNameVariations.Split(';').ToList();
            }
            set
            {
                this.InternalNameVariations = string.Join(";", value);
            }
        }
        public string InternalNameVariations { get; set; }

        [NotMapped]
        public List<string> Aliases
        {
            get
            {
                return InternalAliases.Split(';').ToList();
            }
            set
            {
                this.InternalAliases = string.Join(";", value);
            }
        }
        public string InternalAliases { get; set; }

        [NotMapped]
        public List<string> Urls
        {
            get
            {
                return InternalUrls.Split(';').ToList();
            }
            set
            {
                this.InternalUrls = string.Join(";", value);
            }
        }
        public string InternalUrls { get; set; }

        [NotMapped]
        public List<string> Groups
        {
            get
            {
                return InternalGroups.Split(';').ToList();
            }
            set
            {
                this.InternalGroups = string.Join(";", value);
            }
        }

        public string InternalGroups { get; set; }

        public bool Active { get; set; }
        public DataQuality DataQuality { get; set; }
    }
}
