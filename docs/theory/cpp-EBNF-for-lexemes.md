# EBNF for C++ lexemes

**Язык:** C++ (описание лексем для лабораторной работы)

**Описываемые лексемы:** `identifier`, `integer number`, `real number`, `string literal` (ordinary и raw).

(* Диалект: ISO/IEC 14977. Терминалы в двойных кавычках. Комментарии — в скобках (* ... *) *)

---
```EBNF
space = ' ' ;
horizontal_tab = '\t' ;
vertical_tab = '\v' ;
form_feed = '\f' ;
newline = '\n' ;
whitespace = space | horizontal_tab | vertical_tab | form_feed | newline ;

letter = 'A' | 'B' | 'C' | 'D' | 'E' | 'F' | 'G' | 'H' | 'I' | 'J' | 'K'
       | 'L' | 'M' | 'N' | 'O' | 'P' | 'Q' | 'R' | 'S' | 'T' | 'U' | 'V'
       | 'W' | 'X' | 'Y' | 'Z'
       | 'a' | 'b' | 'c' | 'd' | 'e' | 'f' | 'g' | 'h' | 'i' | 'j' | 'k'
       | 'l' | 'm' | 'n' | 'o' | 'p' | 'q' | 'r' | 's' | 't' | 'u' | 'v'
       | 'w' | 'x' | 'y' | 'z' ;

digit = '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' ;

punct = '{' | '}' | '[' | ']' | '#' | '(' | ')' | '<' | '>' | '%' | ':'
      | ';' | '.' | '?' | '*' | '+' | '-' | '/' | '^' | '&' | '|' | '~'
      | '!' | '=' | ',' | '$' | '@' | '`' | "'" ;

BACKSLASH = '\\' ;
DQUOTE = '"' ;

basic_graphic = letter | digit | '_' | punct ; // видимые символы
basic_source_character = whitespace | basic_graphic | BACKSLASH | DQUOTE ; // все символы

identifier = identifier_start , { identifier_part } ;
identifier_start = letter | '_' ;
identifier_part  = letter | digit | '_' ;

zero = '0' ;
nonzero_digit = '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' ;

decimal_literal = nonzero_digit , { digit } ; //десятичные числа

octal_digit = '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' ; //какие числа в этой системе, снизу то, как пишется для компилятора

octal_literal = '0' , octal_digit , { octal_digit } ; //восьмеричные числа (начинаются с 0)

hex_digit = digit | 'a' | 'b' | 'c' | 'd' | 'e' | 'f'
            | 'A' | 'B' | 'C' | 'D' | 'E' | 'F' ;

hex_literal = ( '0x' | '0X' ) , hex_digit , { hex_digit } ; //шестнадцатеричные числа (0x или 0X).

binary_digit = '0' | '1' ;
binary_literal = ( '0b' | '0B' ) , binary_digit , { binary_digit } ; //бинарные числа (0b или 0B).

integer_literal = decimal_literal | zero | octal_literal | hex_literal | binary_literal ;

integer_suffix = ( 'u' | 'U' ) , [ 'l' | 'L' | 'll' | 'LL' ]
               | ( 'l' | 'L' | 'll' | 'LL' ) , [ 'u' | 'U' ] ;

digit_seq = digit , { digit } ;

fractional_constant = digit_seq , '.' , { digit } //десятичная дробь с точкой
                    | '.' , digit_seq ;

exponent_part = ( 'e' | 'E' ) , [ '+' | '-' ] , digit_seq ;

floating_suffix = 'f' | 'F' | 'l' | 'L' ;

decimal_floating_literal = fractional_constant , [ exponent_part ] , [ floating_suffix ] //десятичные вещественные числа с возможной экспонентой и суффиксом (f или l).
                         | digit_seq , exponent_part , [ floating_suffix ] ;

hex_digit_seq = hex_digit , { hex_digit } ;

hex_fraction = hex_digit_seq , '.' , { hex_digit }
             | '.' , hex_digit_seq
             | hex_digit_seq ;

hex_exponent = ( 'p' | 'P' ) , [ '+' | '-' ] , digit_seq ;

hex_floating_literal = ( '0x' | '0X' ) , hex_fraction , hex_exponent , [ floating_suffix ] ; //числа с плавающей точкой в шестнадцатеричной форме (например, 0x1.2p3).

real_literal = decimal_floating_literal //вещественные
             | hex_floating_literal ;

simple_escape = BACKSLASH , ( "'" | DQUOTE | "?" | BACKSLASH | "a" | "b" | "f"
              | "n" | "r" | "t" | "v" ) ;

octal_escape = BACKSLASH , octal_digit , [ octal_digit , [ octal_digit ] ] ;

hex_escape = BACKSLASH , 'x' , hex_digit , { hex_digit } ;

universal_escape = BACKSLASH , 'u' , hex_digit , hex_digit , hex_digit , hex_digit
                 | BACKSLASH , 'U' , hex_digit , hex_digit , hex_digit , hex_digit ,
                 hex_digit , hex_digit , hex_digit , hex_digit ;

escape_sequence = simple_escape | octal_escape | hex_escape | universal_escape ;

non_quote_non_backslash = letter | digit | "_" | punct                  // любые символы, кроме кавычки и обратного слэша.
                        | space | horizontal_tab | vertical_tab | form_feed ;

string_char = non_quote_non_backslash | escape_sequence ; //символ строки, может быть escape-последовательностью

ordinary_string_literal = DQUOTE , { string_char } , DQUOTE ; // обычная строка в кавычках

raw_delim_char = letter | digit | '_' | '#' | '$' | '%' | '&'
               | '+' | '-' | '.' | ':' | ';' | '<' | '=' | '>' ;

raw_delim = { raw_delim_char } ;

raw_chars = { basic_source_character } ;

raw_string_literal = 'R' , DQUOTE , raw_delim , '(' , raw_chars , ')' , raw_delim , DQUOTE ;

string_literal = ordinary_string_literal | raw_string_literal ;

