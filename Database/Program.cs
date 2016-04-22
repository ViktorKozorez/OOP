using System;
using System.Collections.Generic;

namespace Table
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database("db");

            MyQLGrammar grammar = new MyQLGrammar();

            Table table = Table.CreateTable("workers", new List<Attribute> { new Attribute("Name"), new Attribute("Surname"), new Attribute("Age") });

            table.AddRow(new Row(new object[] { "John", "Jackson", "30" }));
            table.AddRow(new Row(new object[] { "Jack", "Johnson", "20" }));

            db.AddTable(table);
            db.DropTable("fsgfds");
            //db.DropTable("workers");

            Table table2 = Table.CreateTable("workers2", new List<Attribute> { new Attribute("Name2"), new Attribute("Surname2"), new Attribute("Age2") });
            table2.AddRow(new Row(new object[] { "John2", "Jackson2", "302" }));
            table2.AddRow(new Row(new object[] { "Jack2", "Johnson2", "202" }));
            db.AddTable(table2);
            //Console.WriteLine(Table.RowsEqual(table.GetRow(0), table.GetRow(1)));

            Condition condition = new Condition("Name = John");

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
            Console.WriteLine(Environment.CurrentDirectory);
            db.SaveDB();
            db = null;
            //Database db2 = new Database("db");
            Database db2 = Database.LoadDB("db");
            //Console.ReadKey();
        }
    }
}
