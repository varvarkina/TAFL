#include <iostream>
#include <string>
#include <sstream>
#include <numbers>
#include <windows.h>

int main()
{
    SetConsoleOutputCP(1251);

    std::string input;
    double radius;

    std::cout << "������� ������ �����: ";
    std::getline(std::cin, input);

    if (input.empty())
    {
        std::cout << "������: ������ ����" << std::endl;
        return 1;
    }

    std::stringstream ss(input);
    if (!(ss >> radius) || !(ss.eof()))
    {
        std::cout << "������: ������������ ����. �������� ������� ���������� �������" << std::endl;
        return 1;
    }

    if (radius <= 0)
    {
        std::cout << "������: ������ ������ ���� �������������" << std::endl;
        return 1;
    }

    double area = std::numbers::pi * radius * radius;
    std::cout.setf(std::ios::fixed);
    std::cout.precision(4);
    std::cout << "������� ����� = " << area << std::endl;

    return 0;
}