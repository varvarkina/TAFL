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

    std::cout << "Введите радиус круга: ";
    std::getline(std::cin, input);

    if (input.empty())
    {
        std::cout << "Ошибка: пустой ввод" << std::endl;
        return 1;
    }

    std::stringstream ss(input);
    if (!(ss >> radius) || !(ss.eof()))
    {
        std::cout << "Ошибка: некорректный ввод. Возможно введены нечисловые символы" << std::endl;
        return 1;
    }

    if (radius <= 0)
    {
        std::cout << "Ошибка: радиус должен быть положительным" << std::endl;
        return 1;
    }

    double area = std::numbers::pi * radius * radius;
    std::cout.setf(std::ios::fixed);
    std::cout.precision(4);
    std::cout << "Площадь круга = " << area << std::endl;

    return 0;
}