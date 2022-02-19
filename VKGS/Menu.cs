using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKGS
{
    class StartMenu
    {
        private string[] token;
        
        public StartMenu()
        {
            Menu();
        }
        
        private void Menu()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "//token.txt")) token = File.ReadAllLines(Directory.GetCurrentDirectory() + "//token.txt");
            Console.Clear();
            Console.Title = "VKGS 0.2";
            Console.WriteLine("Ytin24 VKGroupSender - 0.2");
            Console.WriteLine("1. Начать рассылку.");
            Console.WriteLine("2. Контакты разработчика");
            Console.WriteLine("3. Выход.");
            int choice = AskForInt(": ");
            switch (choice)
            {
                case 1:
                    StartGroupSend SGS = new(ref token);
                    break;
                case 2:
                    Process.Start("Explorer.exe", "https://t.me/ytin24");
                    Process.Start("Explorer.exe", "https://vk.com/ytin24");
                    Menu();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        private int AskForInt(string question)
        {
            Console.Write(question);
            Console.ForegroundColor = ConsoleColor.Green;
            var answer = Console.ReadLine();
            Console.ResetColor();
            if (int.TryParse(answer, out var integer))
            {
                return integer;
            }
            Console.WriteLine("Не ввел значение! Попробуй еще раз!");
            return AskForInt(question);
        }
    }
}
