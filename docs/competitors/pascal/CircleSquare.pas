PROGRAM CircleSquare(INPUT, OUTPUT);
VAR
  Radius: REAL;
  InputStr: STRING;
  Error: INTEGER;
BEGIN
  WRITE('Введите радиус круга: ');
  READLN(InputStr);
  
  IF Length(InputStr) = 0 
  THEN
    WRITELN('Ошибка: пустой ввод')
  ELSE
    BEGIN
      VAL(InputStr, Radius, Error);
      
      IF Error <> 0 
      THEN
        WRITELN('Ошибка: некорректный ввод. Возможно введены нечисловые символы')
      ELSE IF Radius <= 0 
      THEN
        WRITELN('Ошибка: радиус должен быть положительным')
      ELSE
        WRITELN('Площадь круга: ', Pi * Sqr(Radius):0:4)
    END
END.
