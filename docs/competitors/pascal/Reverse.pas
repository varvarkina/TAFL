PROGRAM Reverse(INPUT, OUTPUT);
VAR
  S: STRING;
BEGIN
  WRITE('Введите строку: ');
  READLN(S);
  
  IF Length(S) = 0 THEN
  BEGIN
    WRITELN('Ошибка: строка не может быть пустой');
    EXIT
  END;
  
  WRITE('Перевернутая строка: ');
  FOR VAR I := Length(S) DOWNTO 1 DO
    WRITE(S[I]);
  WRITELN
END.