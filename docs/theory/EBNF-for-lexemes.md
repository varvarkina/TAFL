# EBNF Грамматика лексем языка Extended Pascal

**Язык:** Extended Pascal (ISO/IEC 10206:1990)

**Охватываемые типы лексем:**
- Идентификаторы (identifiers)
- Литералы целых чисел (integer literals)
- Литералы чисел с плавающей точкой (real/floating-point literals)
- Литералы строк (string literals)

---

```ebnf
letter =
    "A" | "B" | "C" | "D" | "E" | "F" | "G" | "H" | "I" | "J"
  | "K" | "L" | "M" | "N" | "O" | "P" | "Q" | "R" | "S" | "T"
  | "U" | "V" | "W" | "X" | "Y" | "Z"
  | "a" | "b" | "c" | "d" | "e" | "f" | "g" | "h" | "i" | "j"
  | "k" | "l" | "m" | "n" | "o" | "p" | "q" | "r" | "s" | "t"
  | "u" | "v" | "w" | "x" | "y" | "z"
  ;

digit = "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" ;

sign = "+" | "-" ;
underscore = "_" ;

(* ========== IDENTIFIER ========== *)
identifier =
    letter,
    {
        letter
      | digit
      | ( underscore , ( letter | digit ) )
    }
  ;

(* ========== INTEGER ========== *)

digit_sequence = digit, { digit } ;
unsigned_integer = digit_sequence ;
signed_integer = [ sign ], unsigned_integer ;

based_digit =
    "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"
  | "A" | "B" | "C" | "D" | "E" | "F" | "G" | "H" | "I" | "J"
  | "K" | "L" | "M" | "N" | "O" | "P" | "Q" | "R" | "S" | "T"
  | "U" | "V" | "W" | "X" | "Y" | "Z"
  | "a" | "b" | "c" | "d" | "e" | "f" | "g" | "h" | "i" | "j"
  | "k" | "l" | "m" | "n" | "o" | "p" | "q" | "r" | "s" | "t"
  | "u" | "v" | "w" | "x" | "y" | "z"
  ;

based_integer = [ sign ], digit_sequence, "#", based_digit, { based_digit } ;
integer_literal = signed_integer (* стандарт ISO *)
                | based_integer; (* расширение диалекта Pascal например FreePascal *)

(* ========== REAL ========== *)

exponent_part = ("e" | "E"), [ sign ], digit_sequence ;

unsigned_real =
      digit_sequence, ".", digit_sequence, [ exponent_part ]
    | digit_sequence, exponent_part
  ;

signed_real = [ sign ], unsigned_real ;
real_literal = signed_real ;

(* ========== STRING ========== *)

apostrophe = "'" ;
apostrophe_pair = "''" ;

printable_not_apostrophe =
    " " | "!" | '"' | "#" | "$" | "%" | "&"
  | "(" | ")" | "*" | "+" | "," | "-" | "." | "/"
  | "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"
  | ":" | ";" | "<" | "=" | ">" | "?" | "@"
  | "A" | "B" | "C" | "D" | "E" | "F" | "G" | "H" | "I" | "J"
  | "K" | "L" | "M" | "N" | "O" | "P" | "Q" | "R" | "S" | "T"
  | "U" | "V" | "W" | "X" | "Y" | "Z"
  | "a" | "b" | "c" | "d" | "e" | "f" | "g" | "h" | "i" | "j"
  | "k" | "l" | "m" | "n" | "o" | "p" | "q" | "r" | "s" | "t"
  | "u" | "v" | "w" | "x" | "y" | "z"
  | "[" | "\\" | "]" | "^" | "_" | "`" | "{" | "|" | "}" | "~"
  ;

string_element = apostrophe_pair | printable_not_apostrophe ;
character_string = apostrophe, { string_element }, apostrophe ;
string_literal = character_string ;

(* ========== UNION + ROOT ========== *)

number_literal = integer_literal | real_literal ;
lexeme = identifier | number_literal | string_literal ;

```