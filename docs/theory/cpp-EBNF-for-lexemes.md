# EBNF для лексем C++

**Язык:** C++ (современная подсмножество; ориентир — синтаксис лексем, совместимый с C++11/C++14/C++17 и новее)

**Описанные лексемы:**

* идентификатор (identifier)
* литералы целых чисел (integer number)
* литералы чисел с плавающей точкой (real number / floating literal)
* строковые литералы (string literal) — обычные и raw-строки

---

## Условные соглашения EBNF

В этой записи используются стандартные расширения EBNF:

* `::=` — определение нетерминала
* `|` — альтернатива (или)
* `[...]` — опциональная часть (0 или 1 раз)
* `{...}` — повторение (0 или более раз)
* `(...)` — группировка
* Терминалы заключены в одинарные кавычки, например `'a'`, или в двойные кавычки для наглядности; при необходимости используется описание класса символов.
* Символ `'` (апостроф, одиночная кавычка) влево не экранируется внутри терминала — при необходимости используется двойные кавычки.

> Примечание: реальная спецификация C++ гораздо сложнее (поддержка юникод-идентификаторов, подробные правила суффиксов и пр.). Здесь приведено компактное и практическое EBNF-описание, пригодное для учебных лексеров и большинства реализаций, с учётом современных расширений (разделители цифр `'`).

---

## Грамматика лексем (EBNF)

```ebnf
(* --- Идентификатор --- *)
identifier ::= identifier-start { identifier-part } ;
identifier-start ::= letter | '_' ;
identifier-part  ::= letter | digit | '_' ;

letter ::= 'A'..'Z' | 'a'..'z' ;
digit  ::= '0'..'9' ;

(* --- Разделитель цифр (C++14 и новее) --- *)
sep ::= "'" ; (* одинарная кавычка между цифрами *)

digit-seq ::= digit { [sep] digit } ;
hex-digit   ::= '0'..'9' | 'A'..'F' | 'a'..'f' ;
hex-seq     ::= hex-digit { [sep] hex-digit } ;
bin-digit   ::= '0' | '1' ;
bin-seq     ::= bin-digit { [sep] bin-digit } ;
oct-digit   ::= '0'..'7' ;
oct-seq     ::= oct-digit { [sep] oct-digit } ;

(* --- Суффиксы целых чисел --- *)
integer-suffix ::= [ unsigned-suffix ] [ long-suffix ] ;
unsigned-suffix ::= 'u' | 'U' ;
long-suffix ::= 'l' | 'L' | 'll' | 'LL' ;

(* Популярный упрощённый вариант, допускающий сочетания в любом порядке: *)
integer-suffix-alt ::= { 'u' | 'U' } { 'l' | 'L' } [ 'l' | 'L' ] ;

(* --- Литералы целых чисел --- *)
integer-literal ::= decimal-literal | octal-literal | hex-literal | binary-literal ;

decimal-literal ::= nonzero-digit { [sep] digit } [ integer-suffix ]
                   | '0' [ integer-suffix ] ;
nonzero-digit ::= '1'..'9' ;

octal-literal ::= '0' { [sep] oct-digit } [ integer-suffix ] ;

hex-literal ::= ( '0x' | '0X' ) hex-seq [ integer-suffix ] ;

binary-literal ::= ( '0b' | '0B' ) bin-seq [ integer-suffix ] ;

(* --- Суффиксы для вещественных литералов --- *)
floating-suffix ::= 'f' | 'F' | 'l' | 'L' ;

(* --- Экспонента --- *)
exponent-part ::= ( 'e' | 'E' | 'p' | 'P' ) [ '+' | '-' ] digit-seq ;
(* примечание: 'p'/'P' используется для hex floating-point literals (C++17) *)

(* --- Вещественные литералы (floating literals) --- *)
floating-literal ::= fractional-constant [ exponent-part ] [ floating-suffix ]
                   | digit-seq exponent-part [ floating-suffix ]
                   | hex-floating-literal ;

fractional-constant ::= digit-seq '.' [ digit-seq ]
                      | '.' digit-seq ;

hex-floating-literal ::= ( '0x' | '0X' ) hex-seq [ '.' [ hex-seq ] ] [ exponent-part ] [ floating-suffix ]
                       | ( '0x' | '0X' ) '.' hex-seq [ exponent-part ] [ floating-suffix ] ;

(* --- Строковые литералы --- *)
string-literal ::= ordinary-string-literal | raw-string-literal ;

ordinary-string-literal ::= '"' { string-chunk } '"' [ string-suffix ] ;
string-chunk ::= { string-char } ;
string-char ::= any-char-except-quote-or-backslash | escape-sequence ;

escape-sequence ::= '\\' ( simple-escape | octal-escape | hex-escape | universal-escape ) ;
simple-escape ::= 'a' | 'b' | 'f' | 'n' | 'r' | 't' | 'v' | '\\' | '\'' | '"' | '?' ;

octal-escape ::= oct-digit { oct-digit } ;
hex-escape ::= 'x' hex-seq ;
universal-escape ::= 'u' hex-digit hex-digit hex-digit hex-digit
                    | 'U' hex-digit hex-digit hex-digit hex-digit hex-digit hex-digit hex-digit hex-digit ;

string-suffix ::= /* для обычных литералов в C++ — суффиксы типа u8, u, U, L */
                  [ 'u8' | 'u' | 'U' | 'L' ] ;

(* --- Raw string literal --- *)
raw-string-literal ::= 'R' '"' raw-delim '(' raw-chars ')' raw-delim '"' ;
raw-delim ::= { raw-char } ;
raw-char ::= any-character-except-space-paren-backslash-quote ;
raw-chars ::= { any-character } ;

(* Ограничения на разделитель в raw-строке: в стандарте длина разделителя ограничена (обычно <= 16),
   но здесь даётся свободная форма; при реализации лексера стоит реализовать проверку длины и допустимых символов. *)

```

---

## Пояснения и рекомендации для реализации

1. **Юникод и универсальные имена.** В стандарте C++ идентификаторы могут содержать символы из набора универсальных имён и многих классов Unicode. Для большинства учебных и практических лексеров достаточно поддержать ASCII-идентификаторы (`A-Z`, `a-z`, `_`, `0-9`) и при необходимости расширить правило `letter`/`identifier-part` до Unicode-категорий.

2. **Разделитель цифр `'`.** В EBNF введён термин `sep` как одиночная кавычка между цифрами. При реальном лексическом анализе нужно запретить разделитель в начале или конце числа и два разделителя подряд.

3. **Суффиксы.** Суффиксы целых и вещественных литералов в стандарте допускают сложные комбинации (`u`, `U`, `l`, `L`, `ll`, `LL`, `f`, `F`, `L`). В грамматике дана облегчённая, но практичная их модель; при строгой совместимости следует реализовать перечисление допустимых комбинаций и порядок.

4. **Raw-строки.** Синтаксис raw-литералов в стандарте: `R"delim(chars)delim"` — где `delim` — ограничитель длиной до 16 символов, не содержащий пробелов, скобок или кавычек. При реализации нужно проверять совпадение ограничителя в конце.

5. **Hex floating-point.** Для hex-floating использован схожий с стандартом подход: префикс `0x`/`0X`, шестнадцатеричная мантисса и экспонента с `p`/`P`.

6. **Точность EBNF и практичность.** Данный EBNF предназначен для учебных и большинства практических лексеров. Для полностью корректной реализации строго по стандарту C++ следует свериться с последним текстом стандарта (ISO C++17/C++20/C++23) и учесть дополнительные детали (конкатенация строковых литералов, префиксированные строковые литералы, грамматика char-литералов и пр.).

---

Если нужно, могу:

* добавить полную EBNF-грамматику синтаксиса (высокоуровневую) для подмножества C++ (функции, объявления, выражения),
* расширить поддержку Unicode-идентификаторов и точные правила суффиксов согласно конкретной версии стандарта (укажите, какую),
* подготовить тестовый набор лексем и небольшую реализацию лексера на выбранном языке (например, Python или Rust).

```
```
