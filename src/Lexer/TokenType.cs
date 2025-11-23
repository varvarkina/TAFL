namespace Lexer;

public enum TokenType
{
    // Ключевые слова

    /// <summary>
    /// Ключевое слово var
    /// </summary>
    Var,

    /// <summary>
    /// Ключевое слово const
    /// </summary>
    Const,

    /// <summary>
    /// Ключевое слово func
    /// </summary>
    Func,

    /// <summary>
    /// Ключевое слово proc
    /// </summary>
    Proc,

    /// <summary>
    /// Ключевое слово return
    /// </summary>
    Return,

    /// <summary>
    /// Ключевое слово if
    /// </summary>
    If,

    /// <summary>
    /// Ключевое слово else
    /// </summary>
    Else,

    /// <summary>
    /// Ключевое слово while
    /// </summary>
    While,

    /// <summary>
    /// Ключевое слово for
    /// </summary>
    For,

    /// <summary>
    /// Ключевое слово to
    /// </summary>
    To,

    /// <summary>
    /// Ключевое слово downto
    /// </summary>
    Downto,

    /// <summary>
    /// Ключевое слово break
    /// </summary>
    Break,

    /// <summary>
    /// Ключевое слово continue
    /// </summary>
    Continue,

    /// <summary>
    /// Ключевое слово true
    /// </summary>
    True,

    /// <summary>
    /// Ключевое слово false
    /// </summary>
    False,

    /// <summary>
    /// Ключевое слово input
    /// </summary>
    Input,

    /// <summary>
    /// Ключевое слово print
    /// </summary>
    Print,

    // Идентификаторы и литералы

    /// <summary>
    /// Идентификатор (имя переменной или функции)
    /// </summary>
    Identifier,

    /// <summary>
    /// Целочисленный литерал
    /// </summary>
    IntegerLiteral,

    /// <summary>
    /// Вещественный литерал
    /// </summary>
    FloatLiteral,

    /// <summary>
    /// Строковый литерал
    /// </summary>
    StringLiteral,

    // Операторы

    /// <summary>
    /// Оператор присваивания (=)
    /// </summary>
    Assign,

    /// <summary>
    /// Оператор неравенства (!=)
    /// </summary>
    NotEqual,

    /// <summary>
    /// Оператор равенства (==)
    /// </summary>
    Equal,

    /// <summary>
    /// Оператор меньше или равно (&lt;=)
    /// </summary>
    LessOrEqual,

    /// <summary>
    /// Оператор больше или равно (&gt;=)
    /// </summary>
    GreaterOrEqual,

    /// <summary>
    /// Оператор меньше (&lt;)
    /// </summary>
    Less,

    /// <summary>
    /// Оператор больше (&gt;)
    /// </summary>
    Greater,

    /// <summary>
    /// Оператор умножения (*)
    /// </summary>
    Multiply,

    /// <summary>
    /// Оператор деления (/)
    /// </summary>
    Divide,

    /// <summary>
    /// Оператор целочисленного деления (//)
    /// </summary>
    IntegerDivide,

    /// <summary>
    /// Оператор взятия остатка (%)
    /// </summary>
    Modulo,

    /// <summary>
    /// Оператор вычитания или унарный минус (-)
    /// </summary>
    Minus,

    /// <summary>
    /// Оператор сложения (+)
    /// </summary>
    Plus,

    /// <summary>
    /// Логическое И (&amp;&amp;)
    /// </summary>
    And,

    /// <summary>
    /// Логическое ИЛИ (||)
    /// </summary>
    Or,

    /// <summary>
    /// Логическое НЕ (!)
    /// </summary>
    Not,

    // Разделители

    /// <summary>
    /// Точка с запятой (;)
    /// </summary>
    Semicolon,

    /// <summary>
    /// Запятая (,)
    /// </summary>
    Comma,

    /// <summary>
    /// Двоеточие (:)
    /// </summary>
    Colon,

    /// <summary>
    /// Открывающая круглая скобка (()
    /// </summary>
    OpenParenthesis,

    /// <summary>
    /// Закрывающая круглая скобка ())
    /// </summary>
    CloseParenthesis,

    /// <summary>
    /// Открывающая фигурная скобка ({)
    /// </summary>
    OpenBrace,

    /// <summary>
    /// Закрывающая фигурная скобка (})
    /// </summary>
    CloseBrace,

    // Служебные

    /// <summary>
    /// Конец файла
    /// </summary>
    EndOfFile,

    /// <summary>
    /// Ошибка лексического анализа
    /// </summary>
    Error,
}
