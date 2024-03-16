using System.Windows.Input;

namespace AuthDesk.Models
{
    public class SplitButtonCommandItem
    {
        public string Name { get; }
        public ICommand Command { get; }

        public SplitButtonCommandItem(string name, ICommand command)
        {
            Name = name;
            Command = command;
        }
    }
}
