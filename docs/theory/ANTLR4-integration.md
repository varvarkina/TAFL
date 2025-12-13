# ANTLR4 Integration в C#

**ANTLR4** — генератор синтаксических и лексических анализаторов. Эта инструкция показывает, как интегрировать ANTLR4 в C# проект на .NET 8 с автоматической генерацией парсера.

---

## Шаг 1: Создание проекта

### Действие:

```powershell
dotnet new console -n MyParser
cd MyParser
```

### Проверка результата:

```powershell
dir
dotnet --version
```

**Ожидаемый результат:**

- Папка содержит `MyParser.csproj` и `Program.cs`
- Выводится версия .NET 8.0 или выше

---

## Шаг 2: Добавление NuGet пакетов

### Действие:

```powershell
dotnet add package Antlr4.Runtime.Standard
dotnet add package Antlr4BuildTasks
```

### Проверка результата:

```powershell
dotnet restore
type MyParser.csproj
```

**Ожидаемый результат:**

- В консоли: `Restore completed in ...`
- В `MyParser.csproj` видны две строки:
  ```xml
  <PackageReference Include="Antlr4.Runtime.Standard" Version="..." />
  <PackageReference Include="Antlr4BuildTasks" Version="..." />
  ```

---

## Шаг 3: Создание папки и файла грамматики

### Действие:

```powershell
mkdir Grammars
```

Создай файл `Grammars/Calculator.g4` со следующим содержимым:

```antlr
grammar Calculator;

expression
    : term ((PLUS | MINUS) term)*
    ;

term
    : factor ((MUL | DIV) factor)*
    ;

factor
    : LPAREN expression RPAREN
    | NUMBER
    ;

LPAREN : '(' ;
RPAREN : ')' ;
PLUS   : '+' ;
MINUS  : '-' ;
MUL    : '*' ;
DIV    : '/' ;
NUMBER : [0-9]+ ('.' [0-9]+)? ;
WS : [ \t\r\n]+ -> skip ;
```

### Проверка результата:

```powershell
dir Grammars
type Grammars\Calculator.g4
```

**Ожидаемый результат:**

- Файл `Grammars/Calculator.g4` содержит грамматику
- Нет ошибок при просмотре файла (синтаксис читаемый)

---

## Шаг 4: Конфигурация .csproj

### Действие:

Отредактируй `MyParser.csproj`. Найди закрывающий тег `</Project>` и добавь перед ним:

```xml
<ItemGroup>
  <Antlr4 Include="Grammars/Calculator.g4">
    <Listener>false</Listener>
    <Visitor>true</Visitor>
    <Package>MyParser.Generated</Package>
  </Antlr4>
</ItemGroup>
```

Полный пример `.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Antlr4.Runtime.Standard" Version="4.13.1" />
    <PackageReference Include="Antlr4BuildTasks" Version="12.8" PrivateAssets="all" IncludeAssets="build" />
  </ItemGroup>

  <ItemGroup>
    <Antlr4 Include="Grammars/Calculator.g4">
      <Listener>false</Listener>
      <Visitor>true</Visitor>
      <Package>MyParser.Generated</Package>
    </Antlr4>
  </ItemGroup>

</Project>
```

### Проверка результата:

```powershell
type MyParser.csproj
```

**Ожидаемый результат:**

- Видны оба блока `<ItemGroup>`: один с `PackageReference`, второй с `Antlr4 Include`

---

## Шаг 5: Сборка проекта (первая генерация)

### Действие:

```powershell
dotnet clean
dotnet build
```

**Ожидаемый результат:**

- **Последняя строка в консоли:** `Build succeeded.` (Сборка успешна)
- **Нет красных ошибок** (может быть жёлтые warnings — это нормально)

Если сборка упала, ты увидишь строку вида:

```
error CS0246: Type or namespace name 'CalculatorLexer' not found
error: CS0103: The name 'AntlrInputStream' does not exist
```

## Шаг 6: Проверка генерации файлов

### Действие:

```powershell
dir obj\Debug\net8.0
```

Если папка `generated` есть:

```powershell
dir obj\Debug\net8.0\generated\
```

Если её нет, файлы могут быть прямо в `obj\Debug\net8.0`:

```powershell
dir obj\Debug\net8.0\*.cs | findstr /i "calculator"
```

### Проверка результата:

**Ожидаемый результат:**
Должны быть файлы (в одной из папок):

- `CalculatorLexer.cs`
- `CalculatorParser.cs`
- `CalculatorBaseVisitor.cs`
- `CalculatorListener.cs` (если `Listener=true`)
- `CalculatorVisitor.cs` (если `Visitor=true`)

Если файлов нет:

```powershell
dotnet clean
rm -r obj
dotnet restore
dotnet build -v:d
```

---

## Шаг 7: Написание тестового кода

### Действие:

Замени содержимое `Program.cs` на:

```csharp
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MyParser.Generated;

var input = "2 + 3 * 4";

var inputStream = new AntlrInputStream(input);
var lexer = new CalculatorLexer(inputStream);
var tokens = new CommonTokenStream(lexer);
var parser = new CalculatorParser(tokens);

IParseTree tree = parser.expression();

Console.WriteLine("=== Дерево разбора ===");
Console.WriteLine(tree.ToStringTree(parser));

Console.WriteLine("\n=== Текст из дерева ===");
Console.WriteLine(tree.GetText());
```

### Проверка результата:

Сохрани файл и проверь синтаксис:

```powershell
type Program.cs
```

**Ожидаемый результат:**

- Файл содержит `using Antlr4.Runtime;` и другие директивы
- Нет синтаксических ошибок при просмотре

---

## Шаг 8: Финальная сборка и запуск

### Действие:

```powershell
dotnet build
```

**Ожидаемый результат:**

- Сообщение: `Build succeeded.`
- Нет ошибок типа `CS0246` (type not found)

Если есть ошибка `CS0246: Не удалось найти тип ... CalculatorLexer`:

- Проверь, что `<Package>MyParser.Generated</Package>` в `.csproj`
- Проверь `using MyParser.Generated;` в `Program.cs`
- Выполни `dotnet clean` и `dotnet build` заново

---

## Шаг 9: Запуск программы

### Действие:

```powershell
dotnet run
```

### Проверка результата:

**Ожидаемый результат:**

```
=== Дерево разбора ===
(expression (term (factor 2)) + (term (factor 3) * (factor 4)))

=== Текст из дерева ===
2+3*4
```

**Что это значит:**

- Первая строка — дерево разбора в LISP-формате, показывает структуру выражения
- Вторая строка — исходный текст без пробелов
- Парсер корректно распознал приоритет: `3 * 4` вычисляется перед `2 +`

---
