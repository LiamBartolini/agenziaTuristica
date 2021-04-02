using System;
using static System.Console;

namespace menuV1.Models
{
    static class Menu
    {
        static int _selectedIndex;
        static string[] _options;
        static string _prompt;

        //a method that seems like a constructor
        static public void Initialize(string prompt, string[] options)
        {
            _prompt = prompt;
            _options = options;
            _selectedIndex = 0;
        }

        static public int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Clear();
                DisplayOptions();

                ConsoleKeyInfo keyInfo = ReadKey(true);
                keyPressed = keyInfo.Key;

                // update selctedIndex based on pressed arrow
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    _selectedIndex--;
                    if (_selectedIndex == -1)
                        _selectedIndex = _options.Length - 1;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    _selectedIndex++;
                    if (_selectedIndex == _options.Length)
                        _selectedIndex = 0;
                }
            } while (keyPressed != ConsoleKey.Enter);

            return _selectedIndex;
        }

        static private void DisplayOptions()
        {
            WriteLine(_prompt);
            for (int i = 0; i < _options.Length; i++)
            {
                string currentOption = _options[i];
                string prefix;
                if (i == _selectedIndex)
                {
                    prefix = "*";
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = " ";
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }
                WriteLine($"{prefix} << {currentOption} >>");
            }
            ResetColor();
        }
    }
}