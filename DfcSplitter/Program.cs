using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DfcSplitter
{
    [Serializable]
    public class Card
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("related")]
        public string Related { get; set; }

        [XmlElement("set")]
        public Set Set { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    [Serializable]
    public class Set
    {
        [XmlAttribute("picURL")]
        public string PicUrl { get; set; }

        public override string ToString()
        {
            return PicUrl;
        }
    }

    [Serializable]
    [XmlRoot("cockatrice_carddatabase")]
    public class Database
    {
        [XmlArray("cards")]
        [XmlArrayItem("card", typeof(Card))]
        public Card[] Cards { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var basePath = Directory.GetCurrentDirectory();
                var setCode = args[0];
                var cockatriceFileName = $"{basePath}\\{setCode}.xml";
                var imgsPath = $"{basePath}\\{setCode}-files";

                var fullXml = File.ReadAllText(cockatriceFileName);

                Console.WriteLine("Parsing cockatrice XML");

                var sr = new StringReader(fullXml);
                XmlSerializer serializer = new XmlSerializer(typeof(Database));
                Database database = (Database)serializer.Deserialize(sr);

                var allCards = database.Cards;

                Console.WriteLine($"Found {allCards.Length} cards");

                var imagePairs = allCards
                .Where(card => !allCards.Any(dayCard => dayCard.Related == card.Name))
                .Select(card => new
                {
                    DayCard = card,
                    NightCard = allCards.SingleOrDefault(nightCard => nightCard.Name == card.Related)
                })
                .Where(pair => pair.NightCard != null)
                .Select(pair => new
                {
                    Day = pair.DayCard.Name,
                    DayArt = pair.DayCard.Set.PicUrl,
                    Night = pair.NightCard?.Name,
                    NightArt = pair.NightCard.Set.PicUrl
                }).ToList();
                sr.Close();

                Console.WriteLine($"Found {imagePairs.Count} cards");

                var dayRect = new Rectangle(0, 0, 375, 523);
                var nightRect = new Rectangle(752 - 375, 0, 375, 523);

                foreach (var imagePair in imagePairs)
                {
                    Console.WriteLine($"Processing {imagePair.Day} // {imagePair.Night}");
                    
                    var dayFileName = imgsPath + imagePair.DayArt;
                    var nightFileName = imgsPath + imagePair.NightArt;

                    using (Bitmap sourceImage = Image.FromFile(dayFileName) as Bitmap)
                    {

                        // Do not rewrite the images if they have already been rewritten
                        if (imagePair.Night != null && sourceImage.Width == 752)
                        {
                            Bitmap dayImage = new Bitmap(dayRect.Width, dayRect.Height);
                            Bitmap nightImage = new Bitmap(dayRect.Width, dayRect.Height);

                            using (Graphics g = Graphics.FromImage(dayImage))
                            {
                                g.DrawImage(sourceImage, dayRect, dayRect, GraphicsUnit.Pixel);
                            }

                            using (Graphics g = Graphics.FromImage(nightImage))
                            {
                                g.DrawImage(sourceImage, dayRect, nightRect, GraphicsUnit.Pixel);
                            }

                            sourceImage.Dispose();

                            dayImage.Save(dayFileName);
                            nightImage.Save(nightFileName);
                        }
                    }
                }

                Console.WriteLine("Done!");
            }
            catch (Exception e) when (!Debugger.IsAttached)
            {
                Console.WriteLine(e);
            }
        }
    }
}
