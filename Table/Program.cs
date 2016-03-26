using System;
using System.Collections.Generic;

namespace Table
{
    class Program
    {
        static void Main(string[] args)
        {
            MyQLGrammar grammar = new MyQLGrammar();

            Table table = Table.CreateTable(new List<Attribute> { new Attribute("first"), new Attribute("second"), new Attribute("third") });

            table.AddRow(new Row(new object[] { "sad", "fsd", "kgt" }));
            table.AddRow(new Row(new object[] { "2", "3", "4" }));

            //Console.WriteLine(Table.RowsEqual(table.GetRow(0), table.GetRow(1)));

            Condition condition = new Condition("first = sad");

            Console.WriteLine(table.GetRowsCount() + " " + table.GetColumnsCount());

            //Console.WriteLine(table.GetCell(0, "first"));
            //Console.WriteLine(table.GetCell(0, "second"));
            //Console.WriteLine(table.GetCell(0, "third"));
            //Console.WriteLine(table.GetCell(1, "first"));
            //Console.WriteLine(table.GetCell(1, "second"));
            //Console.WriteLine(table.GetCell(1, "third"));
            
            table = table.Select(grammar.Where, table, condition);
            //table = Table.Union(table, table);
            //table = Table.Intersection(table, table);

            //Table newtable = Table.CreateTable(new List<Attribute> { new Attribute("first"), new Attribute("second"), new Attribute("third") });

            //table.AddRow(new Row(new object[] { "sa", "fs", "kg" }));
            //table.AddRow(new Row(new object[] { "21", "31", "41" }));
            
            //table = Table.Intersection(table, newtable);
            //Table newtable = Table.CreateTable(new List<Attribute> { });
            Console.WriteLine(table.GetRowsCount() + " " + table.GetColumnsCount());
            
            //Console.ReadKey();
        }
    }
}
