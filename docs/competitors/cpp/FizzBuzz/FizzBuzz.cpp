#include <iostream>
#include <string>
#include <sstream>
#include <windows.h>

int main()
{
    SetConsoleOutputCP(1251);
    std::string input;
    std::cout << "Введите целое число:" << std::endl;

    while (std::getline(std::cin, input))
    {      
        if (input.empty())
        {
            std::cout << "Ошибка: пустой ввод" << std::endl;
            std::cout << "Введите целое число:" << std::endl;
            continue;
        }

        std::stringstream ss(input);
        int num;

        if (!(ss >> num) || !(ss.eof()))
        {
            std::cout << "Ошибка: некорректный ввод. Возможно введены нечисловые символы" << std::endl;
            std::cout << "Введите целое число:" << std::endl;
            continue;
        }

        if (num % 3 == 0 && num % 5 == 0)
        {
            std::cout << "FizzBuzz" << std::endl;
        }
        else if (num % 3 == 0)
        {
            std::cout << "Fizz" << std::endl;
        }
        else if (num % 5 == 0)
        {
            std::cout << "Buzz" << std::endl;
        }
        else
        {
            std::cout << num << std::endl;
        }
        std::cout << "Введите целое число:" << std::endl;
    }

    return 0;
}