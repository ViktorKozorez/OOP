using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Table
{
    class Database
    {
        private static string currentDirectory = Environment.CurrentDirectory;

        private static string[] tableSeparator = { " \n\n" };
        private static string[] lineSeparator = { " \n" };

        private string caption;
        private List<Table> tables = new List<Table>();

        public Database(string caption)
        {
            this.caption = caption;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddTable(Table table)
        {
            this.tables.Add(table);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DropTable(string tableCaption)
        {
            Table chosenTable = this.tables.Find(table => table.caption == tableCaption);

            this.tables.Remove(chosenTable);
        }

        public void SaveDB()
        {
            string path = System.IO.Path.Combine(currentDirectory, this.caption);

            string text = String.Empty;

            foreach (Table table in this.tables)
            {
                text += table.caption + " \n";

                foreach (Attribute attribute in table.attributes)
                {
                    text += attribute.caption + " ";
                }

                text += "\n";

                for (int i = 0; i < table.GetRowsCount(); i++)
                {
                    for (int j = 0; j < table.GetColumnsCount(); j++)
                    {
                        text += table.GetCell(i, table.attributes[j].caption) + " ";
                    }

                    text += "\n";
                }

                text += "\n";
            }

            File.WriteAllText(path, text);
        }

        public static Database LoadDB(string caption)
        {
            string path = System.IO.Path.Combine(currentDirectory, caption);

            string text = File.ReadAllText(path);

            string[] tables = text.Split(tableSeparator, StringSplitOptions.RemoveEmptyEntries);

            Database db = new Database(caption);

            foreach (string table in tables)
            {
                string[] lines = table.Split(lineSeparator, StringSplitOptions.RemoveEmptyEntries);

                string[] attributesCaptions = lines[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                List<Attribute> attributesList = new List<Attribute>();

                foreach (string attributeCaption in attributesCaptions)
                {
                    attributesList.Add(new Attribute(attributeCaption));
                }

                Table dbTable = Table.CreateTable(lines[0], attributesList);

                for (int i = 2; i < lines.Length; i++)
                {
                    string[] cells = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    object[] data = new object[dbTable.attributes.Count];

                    for (int j = 0; j < cells.Length; j++)
                    {
                        data[j] = cells[j];
                    }

                    Row row = new Row(data);

                    dbTable.AddRow(row);
                }

                db.AddTable(dbTable);
            }

            return db;
        }
    }
}