using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Common;

namespace TemplateNS.Core
{
    #region CompareOption
    public enum CompareOption
    {
        Equals,
        Like,
        ILike
    }
    #endregion

    #region OperationParameter
    public class OperationParameter
    {
        #region Props
        public string ColumnName { get; set; }
        public string ParameterName { get; set; }
        public object DbValue { get; set; }

        #region CompareOption
        private CompareOption p_CompareOption = CompareOption.Equals;
        public CompareOption CompareOption
        {
            get
            {
                return p_CompareOption;
            }
            set
            {
                p_CompareOption = value;
            }
        }
        #endregion

        #endregion

        #region OperationParameter
        public OperationParameter()
        {
        }
        #endregion

        #region OperationParameter
        public OperationParameter(string p_ColumnName, string p_ParameterName, string p_DbValue)
        {
            ColumnName = p_ColumnName;
            ParameterName = p_ParameterName;
            DbValue = p_DbValue;
        }
        #endregion

        #region GetDbParameter
        public DbParameter GetDbParameter()
        {
            return DataAccess.NewParameter(ParameterName, DbValue);
        }
        #endregion

        #region GetCompareOptionExtended
        public string GetCompareOptionExtended()
        {
            if (DbValue.GetType() == typeof(string))
            {
                string nValue = DbValue.ToString();
                if (nValue.Contains("!%"))
                    this.CompareOption = CompareOption.ILike;
                else if (nValue.Contains("%"))
                    this.CompareOption = CompareOption.Like;
            }
            return GetCompareOption();
        }
        #endregion

        #region GetCompareOption
        public string GetCompareOption()
        {
            switch (p_CompareOption)
            {
                case CompareOption.Equals:
                    return "=";
                case CompareOption.Like:
                    return "LIKE";
                case CompareOption.ILike:
                    return "ILIKE";
                default:
                    return "=";
            }
        }
        #endregion
    }
    #endregion

    #region OperationBase
    public abstract class OperationBase
    {
        #region Props
        protected List<OperationParameter> oprParams;
        public DbParameter[] DbParameters { get; set; }
        #endregion

        #region StripLast
        protected string StripLast(string data, string separator)
        {
            int stripLen = separator.Length;

            if (data.Length >= stripLen)
                data = data.Substring(0, data.Length - stripLen);

            return data;
        }
        #endregion

        #region OperationBase
        public OperationBase(List<OperationParameter> p_oprParams)
        {
            oprParams = p_oprParams;
        }
        #endregion

        #region Prepare
        protected abstract void Prepare();
        #endregion
    }

    #endregion

    #region UpdateOperation
    public class UpdateOperation : OperationBase
    {
        #region Props
        public string UpdateColumns { get; set; }
        public string UpdateArguments { get; set; }
        protected List<OperationParameter> oprUpdateKeys;
        #endregion

        #region UpdateOperation
        public UpdateOperation(List<OperationParameter> p_oprParams, List<OperationParameter> p_oprUpdateKeys)
            : base(p_oprParams)
        {
            oprUpdateKeys = p_oprUpdateKeys;
            Prepare();
        }
        #endregion

        #region Prepare
        protected override void Prepare()
        {
            List<DbParameter> resultParams = new List<DbParameter>();
            foreach (OperationParameter item in oprParams)
            {
                resultParams.Add(item.GetDbParameter());
                UpdateColumns += string.Format("{0}={2},",
                    item.ColumnName, item.GetCompareOption(), item.ParameterName);
            }

            foreach (OperationParameter item in oprUpdateKeys)
            {
                UpdateArguments += string.Format("{0}{1}{2},",
                    item.ColumnName, item.GetCompareOptionExtended(), item.ParameterName);

                resultParams.Add(item.GetDbParameter());
            }

            UpdateColumns = StripLast(UpdateColumns, ",");
            UpdateArguments = StripLast(UpdateArguments, ",");
            DbParameters = resultParams.ToArray();
        }
        #endregion
    }

    #endregion

    #region InsertOperation
    public class InsertOperation : OperationBase
    {
        #region Props
        public string InsertColumns { get; set; }
        public string InsertArguments { get; set; }
        #endregion

        #region InsertOperation
        public InsertOperation(List<OperationParameter> p_oprParams)
            : base(p_oprParams)
        {
            Prepare();
        }
        #endregion

        #region Prepare
        protected override void Prepare()
        {
            List<DbParameter> resultParams = new List<DbParameter>();
            foreach (OperationParameter item in oprParams)
            {
                resultParams.Add(item.GetDbParameter());
                InsertColumns += item.ColumnName + ",";
                InsertArguments += item.ParameterName + ",";
            }

            InsertColumns = StripLast(InsertColumns, ",");
            InsertArguments = StripLast(InsertArguments, ",");
            DbParameters = resultParams.ToArray();
        }
        #endregion
    }
    #endregion

    #region DeleteOperation
    public class DeleteOperation : OperationBase
    {
        #region Props
        public string DeleteArguments { get; set; }
        #endregion

        #region DeleteOperation
        public DeleteOperation(List<OperationParameter> p_oprParams)
            : base(p_oprParams)
        {
            Prepare();
        }
        #endregion

        #region Prepare
        protected override void Prepare()
        {
            List<DbParameter> resultParams = new List<DbParameter>();

            foreach (OperationParameter item in oprParams)
            {
                DeleteArguments += string.Format("{0}{1}{2},",
                    item.ColumnName, item.GetCompareOptionExtended(), item.ParameterName);

                resultParams.Add(item.GetDbParameter());
            }

            DeleteArguments = StripLast(DeleteArguments, ",");
            DbParameters = resultParams.ToArray();
        }
        #endregion
    }

    #endregion

    #region SortType
    public enum SortType
    {
        Ascending,
        Descending
    }
    #endregion

    #region SortOperation
    public class SortOperation : List<SortParameter>
    {
        public void Add(string p_ColumnName, SortType p_SortType)
        {
            this.Add(new SortParameter(p_ColumnName, p_SortType));
        }

        public override string ToString()
        {
            if (this.Count != 0)
            {
                string result = "ORDER BY ";
                foreach (SortParameter item in this)
                {
                    result += " " + item.ToString() + ",";
                }
                return result.Substring(0, result.Length - 1);
            }
            else
            {
                return "";
            }
        }
    }
    #endregion

    #region SortParameter
    public class SortParameter
    {
        public string ColumnName { get; set; }
        public SortType SortType { get; set; }

        public SortParameter(string p_ColumnName)
            : this(p_ColumnName, SortType.Ascending)
        {
        }

        public SortParameter(string p_ColumnName, SortType p_SortType)
        {
            SortType = p_SortType;
            ColumnName = p_ColumnName;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", ColumnName, SortType == SortType.Ascending ? "ASC" : "DESC");
        }
    }
    #endregion

    #region PagingOperation
    public class PagingOperation
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public PagingOperation(int p_Offset, int p_Limit)
        {
            Offset = p_Offset;
            Limit = p_Limit;
        }

        public override string ToString()
        {
            return string.Format("OFFSET {0} LIMIT {1}", this.Offset, this.Limit);
        }
    }
    #endregion
}