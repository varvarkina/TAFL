#include <iostream>
#include <string>
#include <windows.h>

int main()
{
    SetConsoleOutputCP(1251);
    std::string str;

    std::cout << "������� ������: ";
    std::getline(std::cin, str);

    if (str.empty())
    {
        std::cout << "������: ������ �� ����� ���� ������" << std::endl;
        return 1;
    }

    std::cout << "������������ ������: ";
    for (int i = str.length() - 1; i >= 0; --i)
    {
        std::cout << str[i];
    }
    std::cout << std::endl;

    return 0;
}