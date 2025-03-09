namespace QuizApp.Commands
{
    public interface ICommand
    {
        string? Name { get; } // The commands name
        string? Description { get; } // What it does
        string? Category { get; }
        // Method to execute the command with the given argument
        void Execute(string[] args); // Whith 'string[] args' we can take in any amount of arguments 
    }
}