lexer grammar DeaExprLexer;

// пробелы/переводы строк
WS              : [ \t\r\n]+ -> skip ;

// комментарии
LINE_COMMENT    : '#' ~[\r\n]* -> skip ;
BLOCK_COMMENT   : '/*' .*? '*/' -> skip ;

// разделители
LPAREN          : '(' ;
RPAREN          : ')' ;
COMMA           : ',' ;

// операторы (сначала длинные)
OR              : '||' ;
AND             : '&&' ;

EQ              : '==' ;
NEQ             : '!=' ;

LE              : '<=' ;
GE              : '>=' ;
LT              : '<' ;
GT              : '>' ;

IDIV            : '//' ;
DIV             : '/' ;
MUL             : '*' ;
MOD             : '%' ;

POW             : '^' ;

PLUS            : '+' ;
MINUS           : '-' ;
NOT             : '!' ;

// встроенные функции (регистронезависимо)
ABS             : [aA][bB][sS] ;
MIN             : [mM][iI][nN] ;
MAX             : [mM][aA][xX] ;

// числа: без знака (знак обрабатывается правилом unary/number в парсере)
fragment DIGIT  : [0-9] ;
fragment INT0   : '0' ;
fragment INTNZ  : [1-9] DIGIT* ;

fragment INT    : INT0 | INTNZ ;
fragment FLOAT  : (INT0 | INTNZ) '.' DIGIT+ ;

NUMBER          : FLOAT | INT ;

// идентификатор (после ключевых слов/функций)
ID              : [A-Za-z] [A-Za-z0-9]* ;
