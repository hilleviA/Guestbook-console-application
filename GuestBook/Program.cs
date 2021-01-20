using System;
using static System.Console; //Gör så att Console inte behöver skrivas ut 
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GuestBook
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sparar filens sökväg
            string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\GuestBookPosts.json";
            //Deklarerar en tom vaiabel så länge för att undvika fel i koden
            string json = "";

            //Kontrollerar så filen existerar
            if (File.Exists(filePath))
            {
                //Läser ut innehåll 
                json = File.ReadAllText(filePath);
            }
            else
            {
                WriteLine("Kunde inte läsa in inlägg.  \n");
                ReadLine();
            }
            
            //Deserialiserar json filen till en lista med objekt  
            List<Post> post = JsonConvert.DeserializeObject<List<Post>>(json);

            //Loop som kör programmet om och om igen 
            while (true)
            {
                ResetColor();
                CursorVisible = false;
                WriteLine("*** H-I-L-L-E-V-I-S G-Ä-S-T-B-O-K ***\n");
                WriteLine("Vad vill du göra? \n");
                WriteLine("1. Skriv nytt inlägg");
                WriteLine("2. Radera inlägg");
                WriteLine("x. Avsluta \n");
                WriteLine("---------------------------------");

                //Loopar igenom listan med inlägg och skriver ut tillsammans med sitt index
                int index = 0;
                foreach (var allPosts in post)
                {
                    WriteLine($"({index}) { allPosts.Message} \nSkrivet av: { allPosts.Sender}");
                    WriteLine("---------------------------------\n");
                    index++;
                }

                //Läser in inmatning från användaren och konverterar till versal
                string input = ReadLine().ToUpper();

                //Jämför inmatning
                switch (input)
                {
                    case "1":
                        CursorVisible = true;
                        //Varible to end loop
                        bool checkLength;
                        do
                        {
                            ResetColor();
                            //Sparar inmatad data i variabler
                            WriteLine("\nSkriv meddelande i gästboken");
                            string messageInput = ReadLine();
                            WriteLine("\nAnge avsändare");
                            string senderInput = ReadLine();

                            //Kontrollerar så att fälten inte är tomma 
                            if (messageInput.Length > 0 && senderInput.Length > 0)
                            {
                                //Adderar nytt inlägg
                                post.Add(new Post { Message = messageInput, Sender = senderInput });

                                //Serialiserar listan tillbaka till json igen
                                json = JsonConvert.SerializeObject(post, Formatting.Indented);
                                //Sparar listan 
                                File.WriteAllText(filePath, json);

                                //Ändrar variablen till true för att hoppa ut loopen
                                checkLength = true;
                            }
                            else
                            {
                                ForegroundColor = ConsoleColor.Red;
                                WriteLine("Inga tomma fält får förekomma. Vänligen fyll i både meddelande och avsändare \n");
                                checkLength = false;
                            }
                        }
                        while (checkLength == false);
                        Clear();
                        ForegroundColor = ConsoleColor.Green;
                        WriteLine("INLÄGGET HAR POSTATS");
                        break;
                    case "2":
                        CursorVisible = true;
                        WriteLine("Ange index att radera: ");
                        //Konverterar den inmatade strängen till int och kontrollerar så att den matchar de index som finns
                        string inputIndex = ReadLine();
                        int removeIndex;
                        bool success = Int32.TryParse(inputIndex, out removeIndex);

                        if (success && (removeIndex < post.Count))
                        {
                            //Tar bort inmatat index
                            post.RemoveAt(removeIndex);
                            //Serialiserar listan tillbaka till json igen
                            json = JsonConvert.SerializeObject(post, Formatting.Indented);
                            //Sparar listan 
                            File.WriteAllText(filePath, json);
                            Clear();
                            ForegroundColor = ConsoleColor.Green;
                            WriteLine("INLÄGGET HAR TAGITS BORT  \n");
                        }
                        else
                        {
                            Clear();
                            ForegroundColor = ConsoleColor.Red;
                            WriteLine("FELAKTIGT INDEX ANGIVET  \n");
                        }
                        break;
                    case "X":
                        Environment.Exit(0);
                        break;
                    default:
                        Clear();
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("OGILTIG INMATNING, FÖRSÖK IGEN \n");
                        break;
                }
            }

        }
    }

}
