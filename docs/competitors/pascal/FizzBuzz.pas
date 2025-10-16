PROGRAM FizzBuzz(INPUT, OUTPUT);
VAR
  NumStr: STRING;
  Num, Error: INTEGER;
BEGIN
  WRITELN('Введите целое число:');
  WHILE NOT EOF DO
  BEGIN
    READLN(NumStr);

    IF Length(NumStr) = 0 
    THEN
      BEGIN
        WRITELN('Ошибка: пустой ввод');
        WRITELN('Введите целое число:');
        CONTINUE
      END;
    
    VAL(NumStr, Num, Error);
    
    IF Error <> 0 
    THEN
      BEGIN
        WRITELN('Ошибка: некорректный ввод. Возможно введены нечисловые символы');
        WRITELN('Введите целое число:');
        CONTINUE
      END;

    IF (Num MOD 3 = 0) AND (Num MOD 5 = 0) 
    THEN
      WRITELN('FizzBuzz')
    ELSE IF Num MOD 3 = 0 
    THEN
      WRITELN('Fizz')
    ELSE IF Num MOD 5 = 0 
    THEN
      WRITELN('Buzz')
    ELSE
      WRITELN(Num);
    WRITELN('Введите целое число:')
  END
END.