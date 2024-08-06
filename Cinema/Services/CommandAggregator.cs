using System.Collections.Generic;
using System.Windows.Input;
using System;

namespace Cinema.Services
{
    public class CommandAggregator
    {
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

        public void RegisterCommand(string name, ICommand command)
        {
            if (!_commands.ContainsKey(name))
                _commands.Add(name, command);
            else
                throw new InvalidOperationException($"Команда с именем {name} уже зарегистрирована.");
        }

        public ICommand GetCommand(string name)
        {
            if (_commands.TryGetValue(name, out var command))
                return command;
            else
                throw new InvalidOperationException($"Команда с именем {name} не найдена.");
        }
    }
}