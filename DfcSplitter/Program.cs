using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
        [XmlAttribute("splitterPath")]
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
                String basePath;
                String imgsPath;
                String cockatriceFileName;
                if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), args[0])))
                {
                    // called with export root dir or images dir
                    imgsPath = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
                    if (!Path.GetFileName(imgsPath).EndsWith("-files"))
                    {
                        imgsPath = Directory.GetFiles(imgsPath, "*-files")[0];
                    }
                    var setCode = Path.GetFileName(imgsPath);
                    setCode = setCode.Substring(0, setCode.Length - 6);
                    cockatriceFileName = Path.Combine(imgsPath, $"{setCode}.xml");
                    if (!File.Exists(cockatriceFileName))
                    {
                        cockatriceFileName = Path.Combine(Directory.GetParent(imgsPath).ToString(), $"{setCode}.xml");
                    }
                }
                else if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), args[0])))
                {
                    // called with export XML (handles both of Lore Seeker exporter's XML files as well as the Custom Standard exporter's XML file)
                    cockatriceFileName = Path.Combine(Directory.GetCurrentDirectory(), args[0]);
                    var setCode = Path.GetFileNameWithoutExtension(cockatriceFileName);
                    imgsPath = Path.Combine(Directory.GetParent(cockatriceFileName).ToString(), $"{setCode}-files");
                    if (!Directory.Exists(imgsPath))
                    {
                        // called with Lore Seeker bundled-images export XML
                        imgsPath = Directory.GetParent(cockatriceFileName).ToString();
                    }
                    if (File.Exists(Path.Combine(imgsPath, $"{setCode}.xml")))
                    {
                        // prefer the XML file in images path, since it's the one with the bundled images in the Lore Seeker export
                        cockatriceFileName = Path.Combine(imgsPath, $"{setCode}.xml");
                    }
                }
                else
                {
                    // called with set code
                    basePath = Directory.GetCurrentDirectory();
                    var setCode = args[0];
                    imgsPath = Path.Combine(basePath, $"{setCode}-files");
                    cockatriceFileName = Path.Combine(imgsPath, $"{setCode}.xml");
                    if (!File.Exists(cockatriceFileName))
                    {
                        // Custom Standard exporter
                        cockatriceFileName = Path.Combine(basePath, $"{setCode}.xml");
                    }
                }

                var fullXml = File.ReadAllText(cockatriceFileName);

                Console.WriteLine("Parsing cockatrice XML");

                var sr = new StringReader(fullXml);
                XmlSerializer serializer = new XmlSerializer(typeof(Database));
                Database database = (Database)serializer.Deserialize(sr);

                var allCards = database.Cards;

                Console.WriteLine($"Found {allCards.Length} cards");

                bool foundError = false;
                foreach (Card card in allCards.Where(card => !allCards.Any(dayCard => dayCard.Related == card.Name)))
                {
                    var nightCards = allCards.Where(nightCard => nightCard.Name == card.Related).Count();
                    if (nightCards > 1)
                    {
                        Console.WriteLine($"error: {card}'s back face has the same name as another card");
                        foundError = true;
                    }
                    if (foundError)
                    {
                        Environment.Exit(1);
                    }
                }
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

                Console.WriteLine($"Found {imagePairs.Count} double-faced cards");

                var dayRect = new Rectangle(0, 0, 375, 523);
                var nightRect = new Rectangle(752 - 375, 0, 375, 523);

                foreach (var imagePair in imagePairs)
                {
                    Console.WriteLine($"Processing {imagePair.Day} // {imagePair.Night}");
                    
                    var dayFileName = imgsPath + imagePair.DayArt;
                    var nightFileName = imgsPath + imagePair.NightArt;

                    if (!File.Exists(dayFileName))
                    {
                        Console.WriteLine($"error: {dayFileName} does not exist");
                        Environment.Exit(1);
                    }

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
