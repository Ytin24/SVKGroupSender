using Newtonsoft.Json;
using RestSharp;

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
        private void GroupSend()
        {
            try
            {
                if (text != null && token != null)
                {
                    Console.WriteLine($"Текст: {text}");
                    Thread.Sleep(1000);
                    RestClient client = new RestClient();
                    for (int n = 0; n < token.Length; n++)
                    {
                        Console.WriteLine($"Токен: {token[n]}");
                        RestRequest request = new RestRequest($"https://api.vk.com/method/messages.getConversations?access_token={token[n]}&v=5.130&count=1");
                        var response = client.GetAsync(request).Result;
                        var Group = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<ConversationResponse>>>(response.Content);
                        int testint = 1;
                        var testtime = DateTime.Now.ToShortTimeString();
                        Console.WriteLine("Сейчас - " + testtime.ToString());
                        for (int a = 0; a < Group.Response.Count; a += 100)
                        {
                            request = new RestRequest($"https://api.vk.com/method/messages.getConversations?access_token={token[n]}&v=5.130&count=100&offset={a}");
                            response = client.GetAsync(request).Result;
                            Group = null;
                            Group = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<ConversationResponse>>>(response.Content);
                            if (Group.Response != null)
                            {

                                for (int i = 0; i < Group.Response.Items.Count; i++)
                                {

                                    try
                                    {
                                        if (Group.Response.Items[i].Conversation.WriteSettings.Allowed && Group.Response.Items[i].Conversation.Peer.Type != "chat")
                                        {
                                            request = new RestRequest($"https://api.vk.com/method/messages.send?access_token={token[n]}&peer_id={Group.Response.Items[i].Conversation.Peer.Id}&random_id=0&message={text}&v=5.130");
                                            var send = client.GetAsync(request);
                                            Console.WriteLine("Отправил сообщение пользователю {0} \t {1}", Group.Response.Items[i].Conversation.Peer.Id, testint++);
                                            //Console.WriteLine("тест {0} {1} \t {2} \t {3}", Group.Response.Items[i].Conversation.WriteSettings.Allowed, Group.Response.Items[i].Conversation.Peer.Id, testint++, Group.Response.Items[i].Conversation.Peer.Type); //тест отправка*
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
        private void ChangeTextToSend()
        {
            Console.WriteLine(("Введи текст рассылки: "));
            text = Console.ReadLine();
            if (text != null)
            {
                GroupSend();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
