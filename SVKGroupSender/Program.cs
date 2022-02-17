using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;

namespace SVKGroupSender
{
    class program
    {
        public static void Main()
        {

            Interface.Menu();
        }
    }
    class Interface
    {
        static string token;
        static string text;
        //private static Dictionary<string, Data> Information = new Dictionary<string, Data>();
        public static void Menu()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "//token.txt")) token = File.ReadAllText(Directory.GetCurrentDirectory() + "//token.txt");
            Console.Clear();
            Console.Title = "SetFps SVKGroupSender Modern by Ytin24";
            Console.WriteLine("SetFps SVKGroupSender - 0.1 by Ytin24");
            Console.WriteLine("1. Начать рассылку.");
            Console.WriteLine("2. Перейти в группу setfps.");
            Console.WriteLine("3. Выход.");
            int choice = AskForInt(": ");
            switch (choice)
            {
                case 1:
                    StartGroupSend();
                    break;
                case 2:
                    Process.Start("Explorer.exe","https://vk.com/setfpsnepidor");
                    Menu();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        private static int AskForInt(string question)
        {
            int answer = 0;
            bool isInt = false;
            do
            {
                Console.Write(question);
                try
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    answer = int.Parse(Console.ReadLine());
                    Console.ResetColor();
                    isInt = true;
                }
                catch (FormatException)
                {
                    Console.ResetColor();
                    Console.WriteLine("Не ввел значение! Попробуй еще раз!");
                    Console.ReadKey();
                }
            } while (!isInt);
            return answer;
        }
        private static void ChangeTextToSend()
        {
            Console.WriteLine(("Введи текст рассылки: "));
            text = Console.ReadLine();
            if (text != null)
            {
                StartGroupSend();
            }
            else
            {
                Environment.Exit(0);
            }
        }
        private static void StartGroupSend()
        {
            //try
            {
                if (text != null && token != null)
                {
                    Console.WriteLine($"Токен: {token}");
                    Console.WriteLine($"Текст: {text}");
                    Thread.Sleep(1000);
                    RestClient client = new RestClient();
                    RestRequest request = new RestRequest($"https://api.vk.com/method/messages.getConversations?access_token={token}&v=5.130&count=1");
                    var response = client.GetAsync(request).Result;
                    var Group = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<ConversationResponse>>>(response.Content);
                    int testint = 1;
                    var testtime = DateTime.Now.ToShortTimeString();
                    Console.WriteLine("Сейчас - " + testtime.ToString());
                    for (int a = 0; a < Group.Response.Count; a+=100)
                    {
                        request = new RestRequest($"https://api.vk.com/method/messages.getConversations?access_token={token}&v=5.130&count=100&offset={a}");
                        response = client.GetAsync(request).Result;
                        Group = null;
                        Group = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<ConversationResponse>>>(response.Content);
                        if (Group.Response != null)
                        {

                            for (int i = 0; i < Group.Response.Items.Count; i++)
                            {
                                
                                try
                                {
                                    if (Group.Response.Items[i].Conversation.WriteSettings.Allowed)
                                    {
                                        request = new RestRequest($"https://api.vk.com/method/messages.send?access_token={token}&peer_id={Group.Response.Items[i].Conversation.Peer.Id}&random_id=0&message={text}&v=5.130");
                                        var send = client.GetAsync(request);
                                        Console.WriteLine("Отправил сообщение пользователю {1} \t {2}",Group.Response.Items[i].Conversation.Peer.Id, testint++);
                                        //Console.WriteLine("тест {0} {1} \t {2}", Group.Response.Items[i].Conversation.WriteSettings.Allowed, Group.Response.Items[i].Conversation.Peer.Id,testint++); //тест отправка*
                                        Thread.Sleep(TimeSpan.FromSeconds(0.5));
                                    }
                                }
                                catch (Exception ex) { break; }
                            }
                        }
                        else { Console.WriteLine(Group.Error.Message); Console.ReadLine(); }
                    }
                    Console.WriteLine("Готово!  {0}",DateTime.Now.ToShortTimeString());
                    Console.ReadLine();
                    Menu();
                }
                else if (text == null)
                {
                    ChangeTextToSend();
                }
                else if (token == null)
                {
                    AddGroupToken();
                }
                else
                {
                    Console.WriteLine("Неизвестная ошибка!");
                }
            }
           // catch (Exception e) { Console.WriteLine("Error: " + e.Message); }
        }
        private static void AddGroupToken()
        {
            if (token != null)
            {
                if (File.Exists(Directory.GetCurrentDirectory() + "//token.txt")) File.Create(Directory.GetCurrentDirectory() + "token.txt");
                File.WriteAllText(Directory.GetCurrentDirectory() + "//token.txt", token);

                StartGroupSend();
            }
            else
            {
                Console.WriteLine(("Введи токен группы *он будет сохранен, чтобы его изменить удалите файл token.txt в папке с программой*"));
                token = Console.ReadLine();
                AddGroupToken();

            }
        }
    }

}

