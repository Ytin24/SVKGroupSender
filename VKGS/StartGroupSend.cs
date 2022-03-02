using Newtonsoft.Json;
using RestSharp;

namespace VKGS
{
    class StartGroupSend
    {

        public StartGroupSend(ref string[] token)
        {
            this.token = token;
            StartSend();
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
        public void StartSend()
        {

            bool TestSend = true;
            bool whilee = true;
            while (whilee)
            {
                Console.WriteLine($"Применить тестовую рассылку?\n 1 - да | 0 - нет");
                int a = int.Parse(Console.ReadLine());
                switch (a)
                {
                    case 1:
                        TestSend = true;
                        whilee = false;
                        break;
                    case 0:
                        TestSend = false;
                        whilee = false;
                        break;
                }
            }
            switch (TestSend)
            {
                case true:
                    Send(TestSend);
                    break;
                case false:
                    Send(TestSend);
                    break;
            }


        }
        private void Send(bool TS)
        {
            RestClient client = new();
            RestRequest request = new();
            if (token != null) AddGroupToken();
            while (true)
            {
                Console.WriteLine("Введи текст рассылки: ");
                text = Console.ReadLine();
                if (text != null)
                {
                    break;
                }
            }
            int count = 1;
            try
            {
                for (int n = 0; n < token.Length; n++)
                {
                    Console.WriteLine($"Токен: {token[n]}");
                    request = new RestRequest($"https://api.vk.com/method/messages.getConversations?access_token={token[n]}&v=5.130&count=1");
                    var responsee = client.GetAsync(request).Result;
                    var GroupCount = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<ConversationResponse>>>(responsee.Content);
                    for (int a = 0; a < GroupCount.Response.Count; a += 200)
                    {
                        request = new RestRequest($"https://api.vk.com/method/messages.getConversations?access_token={token[n]}&v=5.130&count=200&offset={a}");
                        var response = client.GetAsync(request).Result;
                        var Group = JsonConvert.DeserializeObject<BasicResponse<ItemsResponse<ConversationResponse>>>(response.Content);
                        if (Group.Response != null)
                        {
                            for (int i = 0; i < Group.Response.Items.Count; i++)
                            {
                                try
                                {
                                    if (Group.Response.Items[i].Conversation.WriteSettings.Allowed && Group.Response.Items[i].Conversation.Peer.Type != "chat" && TS == false)
                                    {
                                        request = new RestRequest($"https://api.vk.com/method/messages.send?access_token={token[n]}&peer_id={Group.Response.Items[i].Conversation.Peer.Id}&random_id=0&message={text}&v=5.130");
                                        var send = client.GetAsync(request);
                                        Console.WriteLine("Отправил сообщение пользователю {0} \t {1}", Group.Response.Items[i].Conversation.Peer.Id, count++);
                                        Thread.Sleep(TimeSpan.FromSeconds(0.5));
                                    }
                                    else if (Group.Response.Items[i].Conversation.WriteSettings.Allowed && Group.Response.Items[i].Conversation.Peer.Type != "chat" && TS == true)
                                    {
                                        Console.WriteLine("тест {0} {1} \t {2} \t {3}", Group.Response.Items[i].Conversation.WriteSettings.Allowed, Group.Response.Items[i].Conversation.Peer.Id, count++, Group.Response.Items[i].Conversation.Peer.Type); //тест отправка*
                                    }
                                }
                                catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); break; }
                            }
                        }
                        else if (token == null)
                        {
                            AddGroupToken();
                        }
                    }
                }
            }
            catch (Exception) { };
        }

        private void AddGroupToken()
        {
            if (token != null)
            {
                if (!File.Exists(Directory.GetCurrentDirectory() + "//token.txt")) File.Create(Directory.GetCurrentDirectory() + "//token.txt");
                File.WriteAllLines(Directory.GetCurrentDirectory() + "//token.txt", token);
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
