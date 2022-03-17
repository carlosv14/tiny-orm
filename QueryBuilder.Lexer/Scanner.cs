using System.Collections.Generic;
using System.Text;
using QueryBuilder.Core.Enums;
using QueryBuilder.Core.Interfaces;
using QueryBuilder.Core.Models;

namespace QueryBuilder.Lexer
{
    public class Scanner : IScanner
    {
        private Input _input;
        private readonly ILogger _logger;
        private readonly Dictionary<string, TokenType> _keywords;

        public Scanner(Input input, ILogger logger)
        {
            this._input = input;
            this._logger = logger;
            this._keywords = new Dictionary<string, TokenType>
            {
                ["def"] = TokenType.DefKeyword,
                ["tables"] = TokenType.TablesKeyword,
                ["end"] = TokenType.EndKeyWord,
                ["primary"] = TokenType.PrimaryKeyword,
                ["many"] = TokenType.ManyKeyword,
                ["one"] = TokenType.OneKeyword,
                ["int"] = TokenType.IntKeyword,
                ["float"] = TokenType.FloatKeyword,
                ["string"] = TokenType.StringKeyword,
                ["bool"] = TokenType.BoolKeyword,
                ["add"] = TokenType.AddKeyword,
                ["update"] = TokenType.UpdateKeyword,
                ["delete"] = TokenType.DeleteKeyword,
                ["join"] = TokenType.JoinKeyword,
                ["where"] = TokenType.WhereKeyword,
                ["select"] = TokenType.SelectKeyword,
                ["true"] = TokenType.TrueKeyword,
                ["false"] = TokenType.FalseKeyword,
                ["as"] = TokenType.AsKeyword,
                ["relationships"] = TokenType.RelationshipsKeyword,
            };
        }

        public Token GetNextToken()
        {
            var lexeme = new StringBuilder();
            var currentChar = this.GetNextChar();
            while (currentChar != '\0')
            {
                while (char.IsWhiteSpace(currentChar) || currentChar == '\n')
                {
                    currentChar = this.GetNextChar();
                }

                if (char.IsLetter(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = this.PeekNextChar();
                    while (char.IsLetterOrDigit(currentChar))
                    {
                        currentChar = this.GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = this.PeekNextChar();
                    }

                    var tokenLexeme = lexeme.ToString();
                    return BuildToken(tokenLexeme,
                        this._keywords.ContainsKey(tokenLexeme) ? this._keywords[tokenLexeme] : TokenType.Identifier);
                }

                if (char.IsDigit(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();
                    while (char.IsDigit(currentChar))
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }

                    if (currentChar != '.')
                    {
                        return BuildToken(lexeme.ToString(), TokenType.IntConstant);
                    }

                    currentChar = GetNextChar();
                    lexeme.Append(currentChar);
                    currentChar = PeekNextChar();
                    while (char.IsDigit(currentChar))
                    {
                        currentChar = GetNextChar();
                        lexeme.Append(currentChar);
                        currentChar = PeekNextChar();
                    }

                    return BuildToken(lexeme.ToString(), TokenType.FloatConstant);
                }

                switch (currentChar)
                {
                    case '/':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Division);
                    case '<':
                        lexeme.Append(currentChar);
                        var nextChar = PeekNextChar();
                        switch (nextChar)
                        {
                            case '=':
                                GetNextChar();
                                lexeme.Append(nextChar);
                                return BuildToken(lexeme.ToString(), TokenType.LessOrEqualThan);
                            default:
                                lexeme.Append(nextChar);
                                return BuildToken(lexeme.ToString(), TokenType.LessThan);
                        }
                    case '>':
                        lexeme.Append(currentChar);
                        nextChar = this.PeekNextChar();
                        if (nextChar != '=')
                        {
                            return BuildToken(lexeme.ToString(), TokenType.GreaterThan);
                        }

                        currentChar = this.GetNextChar();
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.GreaterOrEqualThan);
                    case ':':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Colon);
                    case ';':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Semicolon);
                    case '(':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.LeftParens);
                    case ')':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.RightParens);
                    case '{':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.LeftBrace);
                    case '}':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.RightBrace);
                    case ',':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Comma);
                    case '+':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Plus);
                    case '-':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Minus);
                    case '*':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Multiplication);
                    case '=':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Equal);
                    case '[':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.LeftBracket);
                    case ']':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.RightBracket);
                    case '\'':
                        lexeme.Append(currentChar);
                        currentChar = GetNextChar();
                        while (currentChar != '\'' && currentChar != '\0')
                        {
                            lexeme.Append(currentChar);
                            currentChar = GetNextChar();
                        }

                        if (currentChar == '\'')
                        {
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.StringLiteral);
                        }

                        break;
                    case '&':
                        lexeme.Append(currentChar);
                        currentChar = GetNextChar();
                        if (currentChar == '&')
                        {
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.LogicalAnd);
                        }

                        lexeme.Clear();
                        _logger.Error(
                            $"Expected & but {currentChar} was found, line ine: {this._input.Position.Line} and column: {this._input.Position.Column}");
                        continue;
                    case '|':
                        lexeme.Append(currentChar);
                        currentChar = GetNextChar();
                        if (currentChar == '|')
                        {
                            lexeme.Append(currentChar);
                            return BuildToken(lexeme.ToString(), TokenType.LogicalOr);
                        }

                        lexeme.Clear();
                        _logger.Error(
                            $"Expected | but {currentChar} was found, line ine: {this._input.Position.Line} and column: {this._input.Position.Column}");
                        continue;
                    case '.':
                        lexeme.Append(currentChar);
                        return BuildToken(lexeme.ToString(), TokenType.Dot);
                }

                _logger.Error(
                    $"Invalid character {currentChar} in line: {this._input.Position.Line} and column: {this._input.Position.Column}");
                currentChar = this.GetNextChar();
            }

            return BuildToken("\0", TokenType.EOF);
        }

        private Token BuildToken(string lexeme, TokenType tokenType)
        {
            return new Token
            {
                Column =
                    this._input.Position.Column > 0 ? this._input.Position.Column - 1 : this._input.Position.Column,
                Line = this._input.Position.Line + 1,
                Lexeme = lexeme,
                TokenType = tokenType,
            };
        }

        private char GetNextChar()
        {
            var next = _input.NextChar();
            _input = next.Reminder;
            return next.Value;
        }

        private char PeekNextChar()
        {
            var next = _input.NextChar();
            return next.Value;
        }
    }
}