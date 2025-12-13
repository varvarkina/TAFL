# ANTLR4 Troubleshooting — Проблемы грамматик и их решение

## 1. Приоритет операторов (Operator Precedence)

### Проблема:
Выражение `2 + 3 * 4` может быть интерпретировано как `(2 + 3) * 4 = 20` вместо ожидаемого `2 + (3 * 4) = 14`, если приоритет операторов не определён корректно.

### Диагностика:
```csharp
// Неправильная грамматика
expression
    : expression (PLUS | MINUS) expression
    | expression (MUL | DIV) expression
    | NUMBER
    ;
```
ANTLR4 не знает, какой оператор имеет выше приоритет.

### Решение:
Использовать иерархию правил — операторы с **более высоким приоритетом** размещать на **более низких уровнях** иерархии:

```antlr
grammar Arithmetic;

// Самый низкий приоритет: + и -
expression
    : term ((PLUS | MINUS) term)*
    ;

// Средний приоритет: * и /
term
    : factor ((MUL | DIV) factor)*
    ;

// Самый высокий приоритет: числа и скобки
factor
    : LPAREN expression RPAREN
    | NUMBER
    ;

// Токены
LPAREN : '(' ;
RPAREN : ')' ;
PLUS   : '+' ;
MINUS  : '-' ;
MUL    : '*' ;
DIV    : '/' ;
NUMBER : [0-9]+ ;
WS : [ \t\r\n]+ -> skip ;
```

### Объяснение:
- `expression` (верхний уровень) обрабатывает `+` и `-`
- `term` (средний уровень) обрабатывает `*` и `/`
- `factor` (нижний уровень) обрабатывает атомарные выражения и скобки

**Результат:** `2 + 3 * 4` корректно парсится как `2 + (3 * 4)`

---

## 2. Левая и правая рекурсия (Left vs Right Recursion)

### Проблема - Правая рекурсия (НЕПРАВИЛЬНО):
```antlr
expression
    : NUMBER
    | NUMBER '+' expression    // Правая рекурсия (неправильно для ANTLR4 до обработки)
    ;
```

При левой рекурсии старые парсеры (типа ANTLR3) входили в бесконечный цикл. **ANTLR4 встроенно поддерживает левую рекурсию**, но правая рекурсия может привести к неправильной ассоциативности.

### Диагностика:
```
Error: left-recursive rule ... is not supported in this version
```
Или неправильный разбор: `1 + 2 + 3` парсится как `1 + (2 + 3)` вместо `(1 + 2) + 3`.

### Решение - Левая рекурсия (ПРАВИЛЬНО):
```antlr
// ANTLR4 может обработать левую рекурсию!
expression
    : expression '+' NUMBER
    | NUMBER
    ;
```

**ANTLR4 автоматически преобразует это во внутренний формат, который работает эффективно.**

### Альтернативный способ (без рекурсии):
```antlr
expression
    : NUMBER ('+' NUMBER)*
    ;
```

---

## 3. Левоассоциативные операции (Left Associativity)

### Проблема:
Выражение `1 - 2 - 3` должно быть парсировано как `(1 - 2) - 3 = -4`, а не как `1 - (2 - 3) = 2` (правоассоциативно).

### Диагностика:
Проверить дерево разбора (parse tree):
```
Неправильно (правоассоциативно):
    -
   / \
  1   -
     / \
    2   3

Правильно (левоассоциативно):
      -
     / \
    -   3
   / \
  1   2
```

### Решение - Явное указание в ANTLR4:
```antlr
// Способ 1: Используя левую рекурсию (автоматически левоассоциативно)
expression
    : expression op=(PLUS | MINUS) expression
    ;
```

ANTLR4 при обнаружении левой рекурсии **автоматически применяет левоассоциативность**.

Или явно:
```antlr
// Способ 2: Иерархия правил (неявная левоассоциативность)
expression
    : term ((PLUS | MINUS) term)*
    ;
```

---

## 4. Правоассоциативные операторы (Right Associativity)

### Проблема:
Оператор возведения в степень: `2 ^ 3 ^ 2` должен быть парсирован как `2 ^ (3 ^ 2) = 2 ^ 9 = 512`, а не `(2 ^ 3) ^ 2 = 8 ^ 2 = 64`.

### Диагностика:
```
Неправильно (левоассоциативно):
    ^
   / \
  ^   2
 / \
2   3

Правильно (правоассоциативно):
    ^
   / \
  2   ^
     / \
    3   2
```

### Решение:
ANTLR4 поддерживает указание ассоциативности в бинарном выражении:

```antlr
expression
    : expression op='^' expression   // Без указания — левоассоциативно
    ;

// Правильно для возведения в степень:
expression
    : base=expression op='^' exp=expression    // Нужно изменить
    ;
```

**Лучший способ — использовать правую рекурсию:**

```antlr
// Правоассоциативный оператор
power
    : base=factor ('^' exp=power)?
    ;

factor
    : NUMBER
    | '(' expression ')'
    ;

expression
    : term ((PLUS | MINUS) term)*
    ;

term
    : power ((MUL | DIV) power)*
    ;
```

Или явное указание в ANTLR4 (версия 4.7+):
```antlr
expression
    : expression op='^'<assoc=right> expression
    ;
```

---

## 5. Неоднозначность в грамматике (Grammar Ambiguity)

### Проблема 5.1: Переменная или вызов функции?

Грамматика:
```antlr
expression
    : ID                           // Переменная
    | ID '(' expression ')'        // Вызов функции
    | expression '+' expression
    ;
```

При входе `foo(1)` парсер не знает: это переменная `foo` или функция?

### Диагностика:
```
warning(125): ambiguous input ...
Multiple paths could match input
```
Или непредсказуемое поведение при разборе.

### Решение:
Переструктурировать грамматику, чтобы разделить случаи:

```antlr
expression
    : term ((PLUS | MINUS) term)*
    ;

term
    : factor ((MUL | DIV) factor)*
    ;

factor
    : primary
    | primary '(' argList? ')'     // Явный вызов функции
    ;

primary
    : ID                           // Просто переменная
    | NUMBER
    | '(' expression ')'
    ;

argList
    : expression (',' expression)*
    ;
```

**Теперь `ID` сам по себе — это переменная, `ID(...)` — вызов функции.**

### Проблема 5.2: Неоднозначность с if-else (Dangling Else)

```antlr
statement
    : 'if' expression 'then' statement
    | 'if' expression 'then' statement 'else' statement
    | otherStatement
    ;
```

Парсер не знает, к какому `if` относится `else`:
```
if (x) then if (y) then a else b
// Это:
// (a) if (x) then (if (y) then a) else b
// или
// (b) if (x) then (if (y) then a else b)
```

### Решение:

Использовать **SLL предсказание** и правила приоритета:

```antlr
statement
    : ifStatement
    | otherStatement
    ;

ifStatement
    : 'if' expression 'then' statement
    | 'if' expression 'then' statement 'else' statement
    ;

// ANTLR4 по умолчанию применяет "greedy" правило:
// 'else' привязывается к ближайшему 'if'
```

Или явно использовать `#` (labels) для ясности:

```antlr
statement
    : 'if' expression 'then' thenPart=statement ('else' elsePart=statement)?  #IfStatement
    | otherStatement  #OtherStatement
    ;
```

---

## 6. Производительность грамматики

### Проблема:
Грамматика парсит медленно, особенно на длинных выражениях.

### Диагностика:
```bash
dotnet build --verbose
# Ищи [WARN] в выводе ANTLR
```

### Решение - Избегать неоднозначности:
1. **Минимизировать количество альтернатив на уровне**
2. **Использовать более специфичные правила вместо общих**
3. **Избегать взаимной рекурсии где возможно**

Плохо:
```antlr
expr : expr '+' expr | expr '*' expr | ID | NUMBER ;
```

Хорошо:
```antlr
expr : term (('+') term)* ;
term : factor (('*') factor)* ;
factor : ID | NUMBER | '(' expr ')' ;
```

---

## 7. Кодировка файла грамматики

### Проблема:
```
Error: file not found
или
Weird character errors
```

### Решение:
Убедиться, что `.g4` файл сохранён в кодировке **UTF-8 без BOM**:
- В Visual Studio: File → Advanced Save Options → UTF-8 without signature

---

## 8. Обработка ошибок в грамматике

### Проблема:
Парсер падает при неправильном входе, нет информативных сообщений об ошибке.

### Решение - Пользовательский обработчик ошибок:

```csharp
public class CustomErrorListener : BaseErrorListener
{
    public override void SyntaxError(
        IRecognizer recognizer,
        IToken offendingSymbol,
        int line,
        int charPositionInLine,
        string msg,
        RecognitionException e)
    {
        Console.WriteLine($"Error at line {line}:{charPositionInLine} - {msg}");
    }
}

// Использование:
var lexer = new CalculatorLexer(inputStream);
var parser = new CalculatorParser(new CommonTokenStream(lexer));
parser.RemoveErrorListeners();
parser.AddErrorListener(new CustomErrorListener());
```

---

## Чек-лист для проверки грамматики

- [ ] **Приоритет:** Операторы с более высоким приоритетом на более низких уровнях?
- [ ] **Ассоциативность:** Левая рекурсия для левоассоциативных операторов?
- [ ] **Правая ассоциативность:** Правая рекурсия для операторов типа `^`, `:=`?
- [ ] **Неоднозначность:** Нет ли предупреждений при сборке?
- [ ] **Кодировка:** Файл `.g4` в UTF-8 без BOM?
- [ ] **Пробелы:** Лексер корректно пропускает пробелы (`WS -> skip`)?
- [ ] **Граница токена:** Разделены ли ID и NUMBER правильно?
- [ ] **Тестирование:** Протестированы ли граничные случаи?

---

## Полный рабочий пример грамматики выражений

```antlr
grammar Expression;

// Entry point
program
    : statement+ EOF
    ;

statement
    : expression NEWLINE
    | assignment NEWLINE
    ;

assignment
    : ID '=' expression
    ;

expression
    : term ((PLUS | MINUS) term)*
    ;

term
    : factor ((MUL | DIV) factor)*
    ;

factor
    : power
    ;

power
    : unary (POW unary)*
    ;

unary
    : (MINUS | NOT)? postfix
    ;

postfix
    : primary (LBRACKET expression RBRACKET)*
    ;

primary
    : LPAREN expression RPAREN
    | ID LPAREN argList? RPAREN   // Вызов функции
    | NUMBER
    | ID                           // Переменная
    | STRING
    ;

argList
    : expression (',' expression)*
    ;

// Токены
LPAREN   : '(' ;
RPAREN   : ')' ;
LBRACKET : '[' ;
RBRACKET : ']' ;
PLUS     : '+' ;
MINUS    : '-' ;
MUL      : '*' ;
DIV      : '/' ;
POW      : '^' ;
NOT      : '!' ;
NEWLINE  : '\r'? '\n' ;

ID
    : [a-zA-Z_][a-zA-Z0-9_]*
    ;

NUMBER
    : [0-9]+ ('.' [0-9]+)?
    ;

STRING
    : '"' (~["\\\r\n] | '\\' .)* '"'
    ;

WS
    : [ \t]+ -> skip
    ;
```

**Эта грамматика корректно обрабатывает:**
- Приоритет: `^` > `*,/` > `+,-`
- Левоассоциативность для `+,-,*,/`
- Правоассоциативность для `^`
- Вызовы функций и индексацию
- Унарные операторы
- Скобки для переопределения приоритета
