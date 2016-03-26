using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Table
{
    class Table
    {
        private List<Attribute> attributes;
        private object[,] data;

        private Table(List<Attribute> attributes)
        {
            this.attributes = attributes;
            this.data = new object[0, attributes.Count];
        }

        public static Table CreateTable(List<Attribute> attributes)
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(attributes != null);
            Contract.Requires(attributes.Count > 0, "You have to pass some attributes!");

            // postconditions
            Contract.Ensures(Contract.Result<Table>().attributes == attributes);

            return new Table(attributes);
        }

        public Table CloneTable()
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(this != null);

            // postconditions
            Contract.Ensures(Contract.Result<Table>().attributes == this.attributes);

            return new Table(this.attributes);
        }

        public object GetCell(int rowNumber, string attributeCaption)
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(this != null);
            Contract.Requires<ArgumentNullException>(attributeCaption != null);

            Contract.Assert(rowNumber >= 0 && rowNumber < this.GetRowsCount(), "You have to pass rowNumber that is in correct range!");
            Contract.Assert(Contract.Exists<Attribute>(this.attributes, attribute => attribute.attributeCaption == attributeCaption));

            int i;
            for (i = 0; i < this.attributes.Count; i++)
            {
                if (attributes[i].attributeCaption == attributeCaption)
                {
                    break;
                }
            }

            return this.data[rowNumber, i];
        }

        public void SetCell(int rowNumber, string attributeCaption, object data)
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(this != null);
            Contract.Requires<ArgumentNullException>(attributeCaption != null);
            Contract.Requires<ArgumentNullException>(data != null);

            Contract.Assert(rowNumber >= 0 && rowNumber < this.GetRowsCount(), "You have to pass rowNumber that is in correct range!");
            Contract.Assert(Contract.Exists<Attribute>(this.attributes, attribute => attribute.attributeCaption == attributeCaption));

            int i;
            for (i = 0; i < this.attributes.Count; i++)
            {
                if (attributes[i].attributeCaption == attributeCaption)
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

        public void AddRow(Row row)
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(this != null);
            Contract.Requires<ArgumentNullException>(row != null);

            Contract.Assert(row.rowData.Length == this.attributes.Count, "Amount of the elements in the row has to be the same as the attributes of the table!");
            Contract.Assert(this.IsRowUnique(row), "The row should be unique!");

            // postconditions
            Contract.Assert(this.GetRowsCount() == Contract.OldValue<int>(this.GetRowsCount() + 1));

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
                newData[this.data.GetLength(0), i] = row.rowData[i];
            }

            this.data = newData;
        }

        public int GetRowsCount()
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(this != null);

            return this.data.GetLength(0);
        }

        public int GetColumnsCount()
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(this != null);

            // postconditions
            Contract.Ensures(Contract.Result<int>() == this.attributes.Count);

            return this.data.GetLength(1);
        }

        public Row GetRow(int rowNumber)
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(this != null);

            Contract.Assert(rowNumber >= 0 && rowNumber < this.GetRowsCount(), "You have to pass rowNumber that is in correct range!");

            object[] data = new object[this.GetColumnsCount()];

            for (int i = 0; i < this.GetColumnsCount(); i++)
            {
                data[i] = this.GetCell(rowNumber, this.attributes[i].attributeCaption);
            }

            return new Row(data);
        }

        public Table Select(Func<Table, Condition, Table> function, Table table, Condition condition)
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(this != null);
            Contract.Requires<ArgumentNullException>(function != null);
            Contract.Requires<ArgumentNullException>(table != null);
            Contract.Requires<ArgumentNullException>(condition != null);

            return function.Invoke(table, condition);
        }

        private static bool RowsEqual(Row firstRow, Row secondRow)
        {
            for (int i = 0; i < firstRow.rowData.Length; i++)
            {
                if (firstRow.rowData[i] != secondRow.rowData[i])
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
                if (firstTable.attributes[i].attributeCaption != secondTable.attributes[i].attributeCaption)
                {
                    return false;
                }
            }

            return true;
        }

        public static Table Union(Table firstTable, Table secondTable)
        {
            // preconditions
            Contract.Requires<ArgumentNullException>(firstTable != null);
            Contract.Requires<ArgumentNullException>(secondTable != null);

            Contract.Assert(firstTable.attributes.Count == secondTable.attributes.Count);
            Contract.Assert(Table.AttributesEqual(firstTable, secondTable));

            Table returnTable = Table.CreateTable(firstTable.attributes);

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
            // preconditions
            Contract.Requires<ArgumentNullException>(firstTable != null);
            Contract.Requires<ArgumentNullException>(secondTable != null);

            Contract.Assert(firstTable.attributes.Count == secondTable.attributes.Count);
            Contract.Assert(Table.AttributesEqual(firstTable, secondTable));

            Table returnTable = Table.CreateTable(firstTable.attributes);

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
