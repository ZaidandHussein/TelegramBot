using System;
using Telegram.Bot;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    class Program
    {
        
        static string token = "";
        static Telegram.Bot.TelegramBotClient bot = new TelegramBotClient(token);
        static void  Main(string[] args)
        {
            HttpClient clint = new HttpClient();
            int offset = 0;
            while(true)
            {
                Telegram.Bot.Types.Update[] updates = bot.GetUpdatesAsync(offset).Result;
                try
                {
                    foreach (var update in updates)
                    {

                        offset = update.Id + 1;
                        if (update.Message != null)
                        {
                            long id = update.Message.From.Id;
                            string txt = update.Message.Text;
                            string username = update.Message.From.Username;
                            string firstname = update.Message.From.FirstName;
                            string lastname = update.Message.From.LastName;

                            if (txt == "/start")
                            {
                                bot.SendTextMessageAsync(id, "You are welcome to know the weather please write the name of the city");
                                Console.WriteLine(id);
                                Console.WriteLine(txt);
                                Console.WriteLine(username);
                                Console.WriteLine(firstname);
                                Console.WriteLine(lastname);
                            }
                            else
                            {
                                var x = "http://api.openweathermap.org/data/2.5/weather?q=";
                                var responseTask = clint.GetAsync(x + txt + "&units=metric&APPID=");
                                responseTask.Wait();

                                if (responseTask.IsCompleted)
                                {
                                    var result = responseTask.Result;
                                    if (result.IsSuccessStatusCode)
                                    {

                                        var massage = result.Content.ReadAsStringAsync();

                                        massage.Wait();
                                        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(massage.Result);
                                       

                                        bot.SendTextMessageAsync(id, "temperature.value: " + Convert.ToString( myDeserializedClass.main.temp ));
                                        bot.SendTextMessageAsync(id, "temperature.min: "+ Convert.ToString(myDeserializedClass.main.temp_min ));
                                        bot.SendTextMessageAsync(id, "temperature.max: " + Convert.ToString(myDeserializedClass.main.temp_max ));
                                      
                                       
                                    }
                                }


                            }
                        }

                    }
                }

                catch (Exception)
                {


                }
                
            }
        }
    }
}
