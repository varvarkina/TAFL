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

    std::cout << "Перевернутая строка: ";
    for (int i = str.length() - 1; i >= 0; --i)
    {
        std::cout << str[i];
    }
    std::cout << std::endl;

    return 0;
}