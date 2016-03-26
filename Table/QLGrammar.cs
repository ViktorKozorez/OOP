namespace Table
{
    abstract class QLGrammar
    {
        public abstract Table Where(Table table, Condition condition);
    }

    class MyQLGrammar : QLGrammar
    {
        public override Table Where(Table table, Condition condition)
        {
            Table returnTable = table.CloneTable();

            string[] conditionSplit = condition.conditionText.Split(' ');

            if (conditionSplit[1] == "=")
            {
                for (int i = 0; i < table.GetRowsCount(); i++)
                {
                    if (table.GetCell(i, conditionSplit[0]).ToString() == conditionSplit[2])
                    {
                        returnTable.AddRow(table.GetRow(i));
                    }
                }
            }

            return returnTable;
        }
    }
}
