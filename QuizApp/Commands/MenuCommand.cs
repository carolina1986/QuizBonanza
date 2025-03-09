namespace QuizApp.Commands
{
    public class MenuCommand : ICommand
    {
        private readonly Dictionary<string, ICommand> _commands;

        public MenuCommand(Dictionary<string, ICommand> commands)
        {
            _commands = commands;
        }

        public string Name => "menu";
        public string Description => "To see available commands type: menu";
        public string Category => "Command for Menu"; 

        public void Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Clear();
                Console.WriteLine("===== Available Commands =====\n");

                // Gruppera kommandon efter kategori
                var groupedCommands = _commands.Values
                    .GroupBy(cmd => cmd.Category)
                    .OrderByDescending(group => group.Key); // Sortera kategorierna i omvänd alfabetiskt

                // Skriv ut varje kategori och dess kommandon
                foreach (var group in groupedCommands)
                {
                    Console.WriteLine($"\n{group.Key}:");
                    foreach (var cmd in group)
                    {
                        Console.WriteLine($"  {cmd.Name,-25} - {cmd.Description}");
                    }
                }

                Console.WriteLine("\nTo see details for a command type: <command_name>");
                Console.WriteLine("To shut down the program type: exit");
            }
            else
            {
                // Visa detaljer om ett specifikt kommando
                string commandName = args[0].ToLower();
                if (_commands.ContainsKey(commandName))
                {
                    Console.WriteLine($"\n{commandName.ToUpper()}:");
                    Console.WriteLine(_commands[commandName].Description);
                }
                else
                {
                    Console.WriteLine($"Command '{commandName}' not found. Type 'menu' to see available commands.");
                }
            }
        }
    }
}
