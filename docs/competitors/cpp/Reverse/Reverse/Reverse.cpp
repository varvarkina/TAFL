#include <iostream>
#include <string>
#include <windows.h>

int main()
{
    SetConsoleOutputCP(1251);
    std::string str;

    std::cout << "Введите строку: ";
    std::getline(std::cin, str);

    if (str.empty())
    {
        std::cout << "Ошибка: строка не может быть пустой" << std::endl;
        return 1;
    }

    std::string reversed(str.rbegin(), str.rend());
    std::cout << "Перевернутая строка: " << reversed << std::endl;

    return 0;
}