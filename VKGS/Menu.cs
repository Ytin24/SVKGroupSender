using System.Diagnostics;

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
            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "VKGS 0.2";
            Console.WriteLine("Ytin24 VKGroupSender - 0.2");
            Console.WriteLine("1. Начать рассылку.");
            Console.WriteLine("2. Изменить токен(ы) вручную.");
            Console.WriteLine("3. Контакты разработчика.");
            Console.WriteLine("4. Выход.");
            int choice = AskForInt(": ");
            switch (choice)
            {
                case 1:
                    StartGroupSend SGS = new(ref token);
                    break;
                case 2:
                    TokenManual();
                    break;
                case 3:
                    Process.Start("Explorer.exe", "https://t.me/ytin24");
                    Process.Start("Explorer.exe", "https://vk.com/ytin24");
                    program.Main();
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        private int AskForInt(string question)
        {
            Console.Write(question);
            Console.ForegroundColor = ConsoleColor.Cyan;
            var answer = Console.ReadLine();
            Console.ResetColor();
            if (int.TryParse(answer, out var integer))
            {
                return integer;
            }
            Console.WriteLine("Не ввел значение! Попробуй еще раз!");
            return AskForInt(question);
        }
        private void TokenManual()
        {
            Console.WriteLine("Внесите изменения и нажмите Enter");
            Process.Start("Explorer.exe", Directory.GetCurrentDirectory() + "\\token.txt");
            Console.ReadLine();
            Console.WriteLine("Готово!");
            program.Main();
        }
    }
}
