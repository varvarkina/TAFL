parser grammar DeaExprParser;


options { tokenVocab=DeaExprLexer; }

// Стартовое правило для валидатора: весь ввод должен быть выражением
unit
    : expression EOF
    ;

expression
    : logicalOr
    ;

logicalOr
    : logicalAnd (OR logicalAnd)*
    ;

logicalAnd
    : equality (AND equality)*
    ;

equality
    : comparison ((EQ | NEQ) comparison)*
    ;

comparison
    : additive ((LT | LE | GT | GE) additive)*
    ;

additive
    : multiplicative ((PLUS | MINUS) multiplicative)*
    ;

multiplicative
    : power ((MUL | DIV | IDIV | MOD) power)*
    ;

// '^' правоассоциативный
power
    : unary (POW power)?
    ;

// унарные: + - !
unary
    : (PLUS | MINUS | NOT) unary
    | primary
    ;

primary
    : number
    | ID
    | functionCall
    | LPAREN expression RPAREN
    ;

functionCall
    : functionName LPAREN argumentList? RPAREN
    ;

functionName
    : ABS
    | MIN
    | MAX
    ;

argumentList
    : expression (COMMA expression)*
    ;

// Чтобы совпасть со спецификацией аналитика "number = [ + | - ] digits [ . digits ]"
number
    : (PLUS | MINUS)? NUMBER
    ;
