using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Minecraft_Username_To_Skin_PNG
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Enter Minecraft Username:");
                Console.WriteLine("-----------------------------------------");
                string username = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Username Cannot Be Empty.");
                    Console.WriteLine("------------------------------------------- ");
                    return;
                }
                
                string skinUrl = $"https://minotar.net/skin/{username}.png";
                
                using var client = new HttpClient();
                var response = await client.GetAsync(skinUrl);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("-----------------------------------------------------------------------------------------------");
                    Console.WriteLine($"Error: Minecraft Username '{username}' Not Found Or Invalid.");
                    Console.WriteLine("-----------------------------------------------------------------------------------------------");
                    return;
                }
                
                string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string skinFolder = Path.Combine(documentsFolder, "MinecraftSkins");
                Directory.CreateDirectory(skinFolder);
                string skinPath = Path.Combine(skinFolder, $"{username}.png");

                using var stream = await response.Content.ReadAsStreamAsync();
                using var fileStream = new FileStream(skinPath, FileMode.Create);

                await stream.CopyToAsync(fileStream);

                Console.WriteLine("-----------------------------------------------------------------------------------------------");
                Console.WriteLine($"Skin For '{username}' Downloaded Successfully At: {skinPath}");
                Console.WriteLine("-----------------------------------------------------------------------------------------------");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine($"Error Downloading Skin: {ex.Message}");
                Console.WriteLine("------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine($"An Error Occurred: {ex.Message}");
                Console.WriteLine("---------------------------------------------------");
            }
        }
    }
}
