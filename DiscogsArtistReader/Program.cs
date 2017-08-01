using System;

namespace DiscogsArtistReader
{
    class Program
    {
        public static void Main(string[] args)
        {
            //xml path
            ArtistReader artistReader = new ArtistReader("C:\\Users\\pierr\\Desktop\\discogs_20170601_artists.xml");

            foreach (Artist a in artistReader.Enumerate())
            {
                ;
                // Treatment
            }

            Console.WriteLine("DONE !!");
        }
    }
}