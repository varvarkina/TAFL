# Грамматика программы языка DEA

## 1. Примеры кода
### Объявление и присваивание
```
var a;
a = 10;
var b = 100;
const c = 0;
```

### Использование выражений
```
var c = a * 2;
var b = abs(c);
```

### Ввод и вывод
```
var a;
input(a);
print(a + 5);
```

### Ветвление if-else
```dea
var age;
input(age);

if (age >= 18) 
{
    print(1); 
} 
else 
{
    print(0); 
}
```

### Циклы
```dea
var i = 1; 

while (i <= 5) 
{
    print(i);
    i = i + 1;
}
```

```dea
var i;
for (i = 1 to 10)
{
    print(i);
}
```

```dea
for (i = 10 downto 1) 
{
    print(i);
}
```

### Функции

```dea
func add(a, b) 
{
    return a + b;
}

proc printResult(value) 
{
    print(value);
}
```
```
var sum = add(x, y);
printResult(sum);
```

## 2. Ключевые особенности и семантические правила языка DEA
### 2.1. Структура программы
   1. Программа находится в одном файле
   2. Программа состоит из инструкций и объявлений

### 2.2. Область действия
   1. Область действия объявленных переменных и констант ограничивается функцией
   2. Существует глобальная область видимости — для переменных и констант, объявленных на верхнем уровне
   3. Функции и процедуры объявляются в глобальной области видимости.
   4. Порядок объявления определяет порядок видимости.

### 2.3. Переменные и константы
   1. Объявление переменной обязательно перед использованием в данной области видимости. Использование необъявленной переменной считается ошибкой.
   2. Повторное объявление имени в одной и той же области считается ошибкой.
   3. Поддерживаются неизменяемые константы `const`.
      
      3.1. Значение константы должно быть задано в момент объявления.
   4. Поддерживаются изменяемые переменные `var`.

### 2.4. Ввод/вывод
Ввод-вывод организован с помощью встроенных функций: `input(x)` и `print(x)`

### 2.5. Виды инструкций
   1. Присваивание не является выражением, но является инструкцией.
   2. Отдельные инструкции объявления переменных и констант
   3. Отдельные инструкции для чтения и печати
   4. Условные инструкции
   5. Цикл while
   6. Цикл for
   7. Вызов процедуры
   8. Возврат из функции
   9. Прерывание цикла
   10. Продолжение цикла

### 2.6. Разделитель инструкций
Символ-разделитель точка с запятой `;` обязательна между отдельными инструкциями.

### 2.7. Решение проблемы висячего else
**Else всегда относится к ближайшему if без else.** 
На уровне грамматики используется подход разделения условных конструкций на matched_if_statement (с else) и unmatched_if_statement (без else или с вложенными условиями).

### 2.8. Функции
- Функция должна быть объявлена **до** её использования.
- **Запрещены рекурсивные вызовы**: функция не может вызывать саму себя.
- **Запрещена взаимная рекурсия**: функции не могут вызывать друг друга циклически.

## 2.8. Параметры и аргументы функции
- Функции и процедуры могут иметь параметры или быть без них
- Объявление параметра не содержит указания на его тип данных
- Аргументы функции вычисляются слева направо
- Порядок вычисления аргументов вызова функции важен при наличии побочных эффектов — например, из-за вызова ещё одной функции: `sum(readInt(), readInt())`.
- Количество аргументов должно соответствовать количеству параметров

## 3. Грамматика в нотации EBNF
```
(* Программа находится в одном файле *)
program = top_level_statement, { top_level_statement }, end_of_file ;

(* Верхнеуровневая инструкция *)
top_level_statement = 
      function_declaration
      | procedure_declaration
      | statement ;

(* Объявление функции *)
function_declaration = 
      "func", identifier, "(", [ parameter_list ], ")", 
      "{", { function_statement_item }, "}" ;

(* Объявление процедуры *)
procedure_declaration = 
      "proc", identifier, "(", [ parameter_list ], ")", 
      "{", { function_statement_item }, "}" ;

(* Параметры функции *)
parameter_list = identifier, { ",", identifier } ;

(* Инструкции внутри функций *)
function_statement_item =
      return_statement 
      | simple_statement, ";" 
      | compound_statement ;

(* Инструкции, включающие ';' внутри правила *)
simple_statement =
      assignment_statement
      | variable_declaration
      | constant_declaration
      | input_statement
      | print_statement 
      | procedure_call ; 

(* Инструкции, НЕ включающие ';' внутри правила *)
compound_statement =
      matched_if_statement
      | unmatched_if_statement 
      | while_statement
      | for_statement ;

statement = simple_statement, ";" | compound_statement ;      

(* Присваивание *)
assignment_statement = identifier, "=", expression ;

(* Объявление переменной *)
variable_declaration = "var", identifier, [ "=", expression ] ;

(* Объявление константы *)
constant_declaration = "const", identifier, "=", expression ;   

(* Ввод *)
input_statement = "input", "(", identifier, ")" ;

(* Вывод *)
print_statement = "print", "(", [ argument_list ], ")" ;

(* Инструкция возврата из функции или процедуры *)
return_statement = "return", [ expression ], ";" ;

(* Блок инструкций *)
block = "{", { statement }, "}" ;

(* Конструкция if с else *)
matched_if_statement =
      "if", "(", expression, ")", block, "else", block ;   
       
(* Конструкция if без else *)
unmatched_if_statement =
      "if", "(", expression, ")", block
      | "if", "(", expression, ")", matched_if_statement, "else", unmatched_if_statement ;

(* Инструкция break *)
break_statement = "break" ;

(* Инструкция continue *)
continue_statement = "continue" ;

(* Блок инструкций для циклов (может содержать break/continue) *)
loop_block = "{", { loop_statement }, "}" ;

(* Инструкции внутри циклов *)
loop_statement =
    statement
    | continue_statement, ";"
    | break_statement, ";" ;
    
(* Цикл while *)
while_statement = "while", "(", expression, ")", loop_block ;

(* Цикл for *)
for_statement = "for", "(", assignment_statement, ( "to" | "downto" ), expression, ")", loop_block ;

(* Вызов процедуры *)
procedure_call = identifier, "(", [ argument_list ], ")" ;

```