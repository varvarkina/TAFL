# TAFL

### Структура репозитория
```text
TAFL/                   
│   ├── competitors/             # Примеры кода на других языках программирования и их анализ
│   │   ├── *.md
│   │   └── ...
│   ├── specification/           # Спецификация своего языка программирования
│   │   └── *.md
│   └── theory/                  # Памятки по теории
│       └── *.md
├── src/                         
│   ├── Ast/                     # Узлы абстрактного дерева (AST)
│   ├── Execution/               # Среда выполнения и Evaluator
│   ├── Grammar/                 # ANTLR грамматика и валидатор
│   ├── Interpreter/             # Консольное приложение (Main)
│   ├── Lexer/                   # Лексический анализатор
│   └── Parser/                  # Синтаксический анализатор
├── tests/                       
│   ├── Grammar.UnitTests/       # Тесты грамматики
│   ├── Interpreter.Specs/       # Функциональные тесты (Reqnroll)
│   ├── Lexer.UnitTests/         # Тесты лексера
│   └── Parser.UnitTests/        # Тесты парсера
├── .editorconfig                
├── .gitignore                   
├── Directory.Build.props        
├── LICENSE                      
├── README.md                    
├── TAFL.sln                     
├── TODO.md   
```

## Сборка и запуск

### Сборка проекта
```bash
dotnet build
```

### Запуск тестов
```bash
dotnet test
```