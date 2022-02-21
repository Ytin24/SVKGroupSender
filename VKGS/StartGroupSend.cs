using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;

namespace VKGS
{
    class StartGroupSend
    {

        public StartGroupSend(ref string[] token)
        {
            this.token = token;
            GroupSend();
        }

        private string[] token;
        private string text;
        private int GetAllCountFromGroup(string token)
        {
            RestClient client = new RestClient();
            RestRequest request = new RestRequest($"https://api.vk.com/method/messages.getConversations?access_token={token}&v=5.130&count=1");
            var response = client.GetAsync(request).Result;
            var Group = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<ConversationResponse>>>(response.Content);
            return Group.Response.Count;
        }


        private void GroupSend()  // здесь очень сильно насрано, уберусь чуть позже.
        {
            try
            {
                Console.WriteLine($"Применить тестовую рассылку?\n 1- да | 0 - нет");
                byte Test = byte.Parse(Console.ReadLine());

                if (Test == 1 || token != null)
                {
                    Console.WriteLine(("Введи текст рассылки: "));
                    text = Console.ReadLine();
                    if(text == null)
                    {
                        Environment.Exit(0);
                    }
                    byte[][] photos = null;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Добавить к рассылке фото? 1- да | 0 - нет");
                    byte photoans = byte.Parse(Console.ReadLine());
                    if (photoans == 1) 
                    {
                        string user_token;
                        Console.WriteLine($"Введите токен из строки браузера");
                        Process.Start(new ProcessStartInfo { FileName = "https://oauth.vk.com/authorize?client_id=7985481&redirect_uri=https://oauth.vk.com/blank.html&scope=offline,groups,wall,photos&response_type=token&v=5.131", UseShellExecute = true });
                        user_token = Console.ReadLine();
                        photos = new Photo().photoset();
                        Console.WriteLine($"Загружаю фото на сервер вк...");
                        RestClient photocl = new RestClient();
                        RestRequest photoreq = new RestRequest($"https://api.vk.com/method/photos.getUploadServer?access_token={user_token}&album_id=1&v=5.131");
                        var resp = photocl.GetAsync(photoreq).Result;
                        var PhotoURL = JsonConvert.DeserializeObject<BasicResponse<UploadServers.UploadServerBase>>(resp.Content);

                    }
                    
                    Console.WriteLine($"Текст: {text}");
                    Thread.Sleep(1000);
                    RestClient client = new RestClient();
                    for (int n = 0; n < token.Length; n++)
                    {
                        Console.WriteLine($"Токен: {token[n]}");
                        int testint = 1;
                        token.Last();
                        var testtime = DateTime.Now.ToShortTimeString();
                        Console.WriteLine("Сейчас - " + testtime.ToString());
                        for (int a = 0; a < GetAllCountFromGroup(token[n]); a += 200)
                        {
                            RestRequest request = new RestRequest($"https://api.vk.com/method/messages.getConversations?access_token={token[n]}&v=5.130&count=200&offset={a}");
                            var response = client.GetAsync(request).Result;
                            var Group = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<ConversationResponse>>>(response.Content);
                            if (Group.Response != null)
                            {
                                for (int i = 0; i < Group.Response.Items.Count; i++)
                                {
                                    try
                                    {
                                        if (Group.Response.Items[i].Conversation.WriteSettings.Allowed && Group.Response.Items[i].Conversation.Peer.Type != "chat" && Test == 0)
                                        {
                                            request = new RestRequest($"https://api.vk.com/method/messages.send?access_token={token[n]}&peer_id={Group.Response.Items[i].Conversation.Peer.Id}&random_id=0&message={text}&v=5.130");
                                            var send = client.GetAsync(request);
                                            Console.WriteLine("Отправил сообщение пользователю {0} \t {1}", Group.Response.Items[i].Conversation.Peer.Id, testint++);
                                            Thread.Sleep(TimeSpan.FromSeconds(0.5));
                                        }
                                        else if (Group.Response.Items[i].Conversation.WriteSettings.Allowed && Group.Response.Items[i].Conversation.Peer.Type != "chat" && Test == 1)
                                        {
                                            Console.WriteLine("тест {0} {1} \t {2} \t {3}", Group.Response.Items[i].Conversation.WriteSettings.Allowed, Group.Response.Items[i].Conversation.Peer.Id, testint++, Group.Response.Items[i].Conversation.Peer.Type); //тест отправка*
                                            Thread.Sleep(TimeSpan.FromSeconds(0.5));
                                        }
                                    }
                                    catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); break; }
                                }
                            }
                            else { Console.WriteLine(Group.Error.Message); Console.ReadLine(); }
                        }
                    }
                    Console.WriteLine("Готово!  {0}", DateTime.Now.ToShortTimeString());
                    Console.ReadLine();
                    StartMenu SM = new StartMenu();
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
            catch (Exception e) { Console.WriteLine("Error: " + e.Message); }
        }
        private void AddGroupToken()
        {
            if (token != null)
            {
                if (!File.Exists(Directory.GetCurrentDirectory() + "//token.txt")) File.Create(Directory.GetCurrentDirectory() + "token.txt");
                File.WriteAllLines(Directory.GetCurrentDirectory() + "//token.txt", token);
                GroupSend();
            }
            else
            {
                Console.WriteLine(("Введи токен группы\n*он будет сохранен, чтобы его изменить удалите файл token.txt в папке с программой*\n(если их несколько, то разделять запятой)"));
                string tokenstr = Console.ReadLine();
                token = tokenstr.Split(',');
                AddGroupToken();
            }
        }

        
    }
}
