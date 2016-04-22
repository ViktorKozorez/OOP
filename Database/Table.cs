using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Table
{
    class Table
    {
        public string caption { get; private set; }
        public List<Attribute> attributes { get; private set; }
        private object[,] data;

        private Table(string caption, List<Attribute> attributes)
        {
            this.caption = caption;
            this.attributes = attributes;
            this.data = new object[0, attributes.Count];
        }

        public static Table CreateTable(string caption, List<Attribute> attributes)
        {
            return new Table(caption, attributes);
        }

        public Table CloneTable()
        {
            return new Table(this.caption, this.attributes);
        }

        public object GetCell(int rowNumber, string attributeCaption)
        {
            int i;
            for (i = 0; i < this.attributes.Count; i++)
            {
                if (attributes[i].caption == attributeCaption)
                {
                    break;
                }
            }

            return this.data[rowNumber, i];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetCell(int rowNumber, string attributeCaption, object data)
        {
            int i;
            for (i = 0; i < this.attributes.Count; i++)
            {
                if (attributes[i].caption == attributeCaption)
                {
                    break;
                }
            }

            this.data[rowNumber, i] = data;
        }

        private bool IsRowUnique(Row row)
        {
            for (int i = 0; i < this.GetRowsCount(); i++)
            {
                if (Table.RowsEqual(this.GetRow(i), row))
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddRow(Row row)
        {
            object[,] newData = new object[this.data.GetLength(0) + 1, this.data.GetLength(1)];

            for (int i = 0; i < this.data.GetLength(0); i++)
            {
                for (int j = 0; j < this.data.GetLength(1); j++)
                {
                    newData[i, j] = this.data[i, j];
                }
            }

            for (int i = 0; i < this.attributes.Count; i++)
            {
                newData[this.data.GetLength(0), i] = row.data[i];
            }

            this.data = newData;
        }

        public int GetRowsCount()
        {
            return this.data.GetLength(0);
        }

        public int GetColumnsCount()
        {
            return this.data.GetLength(1);
        }

        public Row GetRow(int rowNumber)
        {
            object[] data = new object[this.GetColumnsCount()];

            for (int i = 0; i < this.GetColumnsCount(); i++)
            {
                data[i] = this.GetCell(rowNumber, this.attributes[i].caption);
            }

            return new Row(data);
        }

        public Table Select(Func<Table, Condition, Table> function, Table table, Condition condition)
        {
            return function.Invoke(table, condition);
        }

        private static bool RowsEqual(Row firstRow, Row secondRow)
        {
            for (int i = 0; i < firstRow.data.Length; i++)
            {
                if (firstRow.data[i] != secondRow.data[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AttributesEqual(Table firstTable, Table secondTable)
        {
            for (int i = 0; i < firstTable.attributes.Count; i++)
            {
                if (firstTable.attributes[i].caption != secondTable.attributes[i].caption)
                {
                    return false;
                }
            }

            return true;
        }

        public static Table Union(Table firstTable, Table secondTable)
        {
            Table returnTable = Table.CreateTable(firstTable.caption, firstTable.attributes);

            for (int i = 0; i < firstTable.GetRowsCount(); i++)
            {
                returnTable.AddRow(firstTable.GetRow(i));
            }

            bool isUnique;

            for (int i = 0; i < secondTable.GetRowsCount(); i++)
            {
                isUnique = true;

                for (int j = 0; i < firstTable.GetRowsCount(); j++)
                {
                    if (Table.RowsEqual(firstTable.GetRow(j), secondTable.GetRow(i)))
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique)
                {
                    returnTable.AddRow(secondTable.GetRow(i));
                }
            }

            return returnTable;
        }

        public static Table Intersection(Table firstTable, Table secondTable)
        {
            Table returnTable = Table.CreateTable(firstTable.caption, firstTable.attributes);

            for (int i = 0; i < firstTable.GetRowsCount(); i++)
            {
                for (int j = 0; i < secondTable.GetRowsCount(); j++)
                {
                    if (Table.RowsEqual(firstTable.GetRow(i), secondTable.GetRow(j)))
                    {
                        returnTable.AddRow(firstTable.GetRow(i));
                        break;
                    }
                }
            }

            return returnTable;
        }
    }
}
