namespace QuizApp.Commands
{
    public class CommandHandler
    {
        // The Dictionary will use the string as the key and ICommand as the command object
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public IEnumerable<ICommand> GetAllCommands()
        {
            return _commands.Values;
        }

        public void CommandRegistration(ICommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                throw new ArgumentException("Error: command name not valid.");
            }

            _commands[command.Name.ToLower()] = command;
        }

        public void HandleCommand(string input)
        {
            string[] parts = input.Trim().Split(' '); // This will use ' ' to split the input into parts
            if (parts.Length == 0) return; // If the input is empty, dont do anything

            // This will make the first part into commandName
            string commandName = parts[0].ToLower(); 
            // This will skip the first part (commandName) and make the rest into arguments (array)
            string[] args = parts.Skip(1).ToArray();

            // Checks if the commandName exist in the dictionary
            if (_commands.TryGetValue(commandName, out var command))
            {
                _commands[commandName].Execute(args);
            }
            else // Will handle the case when the commandName is not valid
            {
                Console.WriteLine($"'{commandName}' is not a valid command");
                Console.WriteLine("(type 'menu' to see available commands)");            
            }
        }
    }
}