using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace DiscogsArtistReader
{
    public class ArtistReader
    {
        private StreamReader streamReader;
        private XmlReader xmlReader;
        private bool preparedReader;

        public ArtistReader(string filename)
        {
            this.streamReader = new StreamReader(new FileStream(filename, FileMode.Open), Encoding.UTF8);
            XmlReaderSettings xmlSettings = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Document,
                IgnoreComments = true,
                IgnoreWhitespace = true,
                CheckCharacters = false
            };

            this.xmlReader = XmlReader.Create(this.streamReader, xmlSettings);
        }

        public Artist Read()
        {
            if (!this.preparedReader)
            {
                this.PrepareReader();
            }

            if (this.xmlReader.EOF)
            {
                return null;
            }

            this.xmlReader.IsStartElement("artist");

            return this.ReadArtist();
        }

        public Artist ReadArtist()
        {
            Artist artist = new Artist();

            while (!(this.xmlReader.IsStartElement() && this.xmlReader.Name == "artist"))
                this.xmlReader.Read();
            this.xmlReader.Read();

            while (true)
            {
                if (this.xmlReader.NodeType == XmlNodeType.EndElement && this.xmlReader.Name == "artist")
                    break;

                while (!this.xmlReader.IsStartElement())
                    this.xmlReader.Read();

                if (this.xmlReader.IsStartElement())
                {

                    switch (this.xmlReader.Name)
                    {
                        case "images":
                            while (!(this.xmlReader.NodeType == XmlNodeType.EndElement && this.xmlReader.Name == "images"))
                                this.xmlReader.Read();
                            break;
                        case "id":
                            artist.ArtistId = this.xmlReader.ReadElementContentAsInt();
                            break;
                        case "name":
                            artist.Name = this.xmlReader.ReadElementContentAsString();
                            artist.IndexedName = RemoveDiacritics(artist.Name).ToLower();
                            break;
                        case "realname":
                            artist.RealName = this.xmlReader.ReadElementContentAsString();
                            break;
                        case "profile":
                            artist.Profile = this.xmlReader.ReadElementContentAsString();
                            break;
                        case "data_quality":
                            this.xmlReader.ReadElementContentAsString();// TODO
                            break;
                        case "namevariations":
                            artist.NameVariations = readStringList("namevariations", "name");
                            break;
                        case "aliases":
                            artist.Aliases = readStringList("aliases", "name");
                            break;
                        case "members":
                            artist.Members = readIntList("members", "id");
                            break;
                        case "groups":
                            artist.Groups = readStringList("groups", "name");
                            break;
                        case "urls":
                            artist.Urls = readStringList("urls", "url");
                            break;
                        default:
                            break;
                    }
                }

                if (this.xmlReader.IsStartElement() && this.xmlReader.Name == "artist")
                    break;
            }

            return artist;
        }

        public List<string> readStringList(string listTagName, string tagName)
        {
            var result = new List<String>();
            this.xmlReader.Read();

            while (!(this.xmlReader.NodeType == XmlNodeType.EndElement && this.xmlReader.Name == listTagName))
            {
                if (this.xmlReader.IsStartElement() && this.xmlReader.Name == tagName)
                    result.Add(this.xmlReader.ReadElementContentAsString());
                else
                    this.xmlReader.Read();
            }

            return result;
        }

        public List<int> readIntList(string listTagName, string tagName)
        {
            var result = new List<int>();
            this.xmlReader.Read();

            while (!(this.xmlReader.NodeType == XmlNodeType.EndElement && this.xmlReader.Name == listTagName))
            {
                if (this.xmlReader.IsStartElement() && this.xmlReader.Name == tagName)
                    result.Add(this.xmlReader.ReadElementContentAsInt());
                else
                    this.xmlReader.Read();
            }

            return result;
        }

        public double EstimatedProgress
        {
            get
            {
                if (this.streamReader == null)
                {
                    throw new InvalidOperationException();
                }
                return (double)this.streamReader.BaseStream.Position / (double)this.streamReader.BaseStream.Length;
            }
        }

        public void Dispose()
        {
            this.xmlReader.Dispose();
        }

        public IEnumerable<Artist> Enumerate()
        {
            Artist artist;
            while ((artist = this.Read()) != null)
            {
                yield return artist;
            }
        }

        private void PrepareReader()
        {
            if (!this.xmlReader.Read())
            {
                throw new EndOfStreamException();
            }

            this.xmlReader.IsStartElement("artists");
            this.preparedReader = true;
        }

        private string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
