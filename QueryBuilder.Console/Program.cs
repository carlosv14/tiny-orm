using System.IO;
using QueryBuilder.Core.Enums;
using QueryBuilder.Infrastructure;
using QueryBuilder.Lexer;
using QueryBuilder.Parser;

namespace QueryBuilder.Console
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      var fileContent = File.ReadAllText("test.txt");
      var logger = new Logger();
      var scanner = new Scanner(new Input(fileContent), logger);

// var  token = scanner.GetNextToken();
//  while (token != TokenType.EOF)
//  {
//      logger.Info(token.ToString());
//      token = scanner.GetNextToken();
//  }

      var parser = new Parser.Parser(scanner, logger);
      parser.Parse();   
    }
  }
}