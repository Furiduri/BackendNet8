namespace GCatcode.Utils.ChangeHistory
{
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Helper to save Changes History.
    /// </summary>
    /// <typeparam name="T">Class to save changes.</typeparam>
    public class ChangeHistoryHelper<T>
    {
        /// <summary>
        /// Gets or sets MSSQL Helper.
        /// </summary>
        private readonly string ConnectionString;

        protected ChangeHistoryDTO changeHistoryReverse;
        private string ReverseData { get; set; }

        /// <summary>
        /// Gets or sets List to Changes to save.
        /// </summary>
        private List<ChangeHistoryData<T>> ListChanges { get; set; }

        /// <summary>
        /// Gets or sets Remove data list for Type T.
        /// </summary>
        private List<T> RemovesList { get; set; }

        /// <summary>
        /// Gets or sets Add data list for Type T.
        /// </summary>
        private List<T> AddList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeHistoryHelper{T}"/> class.
        /// </summary>
        /// <param name="context">Entity database context.</param>
        /// <param name="changeType">Change Type ID.</param>
        /// <param name="userId">User Id.</param>
        public ChangeHistoryHelper(string connectionString, ChangeHistoryType changeType, int userId)
        {
            ConnectionString = connectionString;
            ChangeType = changeType;
            UserId = userId;
            ListChanges = new List<ChangeHistoryData<T>>();
            AddList = new List<T>();
            RemovesList = new List<T>();
        }

        /// <summary>
        /// Gets or sets change Type.
        /// </summary>
        protected ChangeHistoryType ChangeType { get; set; }

        /// <summary>
        /// Gets or sets User Id.
        /// </summary>
        protected int UserId { get; set; }

        protected int ReverseId { get; private set; }
        protected string RefReverse { get; private set; }
        protected string ReverseMethod { get; private set; }

        protected void AddToChangeHistory(ChangeHistoryData<T> data)
        {
            if (!ListChanges.Contains(data))
                ListChanges.Add(data);
        }

        protected void RemoveToChangeHistory(ChangeHistoryData<T> data)
        {
            if (ListChanges.Contains(data))
                ListChanges.Remove(data);
        }

        /// <summary>
        /// Add item to list History Add.
        /// </summary>
        /// <param name="item"></param>
        public void AddToAddHistory(T item)
        {
            if (!AddList.Contains(item))
                AddList.Add(item);
        }

        /// <summary>
        /// Remove item to list History Add.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveToAddHistory(T item)
        {
            if (AddList.Contains(item))
                AddList.Remove(item);
        }

        /// <summary>
        /// Add item to list History Removed.
        /// </summary>
        /// <param name="item"></param>
        public void AddToRemoveHistory(T item)
        {
            if (!RemovesList.Contains(item))
                RemovesList.Add(item);
        }

        /// <summary>
        /// Remove item to list History Removed.
        /// </summary>
        /// <param name="item"></param>
        public void RemoveToRemoveHistory(T item)
        {
            if (RemovesList.Contains(item))
                RemovesList.Remove(item);
        }

        /// <summary>
        /// Save Change History to Add or Remove rows.
        /// </summary>
        /// <param name="observations">Other mensajes text.</param>
        public List<ChangeHistoryDTO> SaveAddOrRemove(string observations = "")
        {
            try
            {
                List<ChangeHistoryDTO> listRes = new List<ChangeHistoryDTO>();
                JsonSerializerSettings jsonSetings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                };

                string changedDataRemove = JsonConvert.SerializeObject(this.RemovesList, jsonSetings);
                string changedDataAdd = JsonConvert.SerializeObject(this.AddList, jsonSetings);
                var resAdd = Save(changedDataAdd, $"[Add] {observations}");
                if (resAdd != null)
                    listRes.Add(resAdd);
                var resRemove = Save(changedDataRemove, $"[Remove] {observations}");
                if (resRemove != null)
                    listRes.Add(resRemove);
                return listRes;
            }
            catch
            {
                throw;
            }
        }

        private ChangeHistoryDTO Save(string changedData, string observations)
        {
            try
            {
                if (changedData.Length > 3)
                {
                    var changeHistory = new ChangeHistoryDTO
                    {
                        ChangeTypeId = (int)this.ChangeType,
                        UserModifying = this.UserId,
                        ChangedData = changedData,
                        Observations = observations,
                        DateTime = System.DateTime.UtcNow
                    };
                    using (var db = new SqlConnection(ConnectionString))
                    {
                        return db.Query<ChangeHistoryDTO>(@"INSERT INTO [dbo].[ChangeHistory]
                                   ([ChangeTypeId]
                                   ,[DateTime]
                                   ,[UserModifying]
                                   ,[ChangedData]
                                   ,[Observations])
                             VALUES
                                   (@ChangeTypeId
                                   , @DateTime
                                   , @UserModifying
                                   , @ChangedData
                                   , @Observations)

                            SELECT * FROM [ChangeHistory] WHERE ChangeHistoryId = @@IDENTITY",
                            changeHistory)
                            .FirstOrDefault();
                    }
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save Change History.
        /// </summary>
        /// <param name="method">Name Method to use.</param>
        protected List<ChangeHistoryDTO> SaveHistoryChanges(string method)
        {
            try
            {
                List<ChangeHistoryDTO> listRes = new List<ChangeHistoryDTO>();
                foreach (var item in ListChanges)
                {
                    string observation = string.Empty;
                    string changedData = this.GetDifferences(item);

                    if (changedData.Length > 3)
                    {
                        switch (this.ChangeType)
                        {
                            default:
                                observation = $"[System] {method}";
                                break;
                        }

                        listRes.Add(Save(changedData, observation));
                    }
                }
                return listRes;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get diference.
        /// </summary>
        /// <returns>Json string with the dierences.</returns>
        private string GetDifferences(ChangeHistoryData<T> data)
        {
            JsonSerializerSettings jsonSetings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };
            JObject obj1 = JObject.Parse(JsonConvert.SerializeObject(data.UpdatedData, jsonSetings));
            JObject obj2 = JObject.Parse(JsonConvert.SerializeObject(data.OriginalData, jsonSetings));
            JObject res = JsonMethods.GetJsonDifference(obj1, obj2);
            return res.ToString(Formatting.None);
        }

        protected T GetReverseChanges(List<string> listChanges, T baseData)
        {
            if (baseData == null)
                throw new Exception("Not object to recovery.");

            JsonMergeSettings settings = new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Concat, PropertyNameComparison = StringComparison.OrdinalIgnoreCase };
            JObject json = listChanges.Aggregate(new JObject(), (j, s) => { j.Merge(JObject.Parse(s), settings); return j; });
            JObject jsonBase = new JObject();
            T jsonObject = json.ToObject<T>();

            bool isValidObject = false;
            foreach (PropertyInfo propertyInfo in baseData.GetType().GetProperties())
            {
                if (json.ContainsKey(propertyInfo.Name))
                {
                    var valueBase = propertyInfo.GetValue(baseData);
                    var jsonValue = propertyInfo.GetValue(jsonObject);
                    if (valueBase.Equals(jsonValue))
                        continue;

                    string basePropertyInfo = JsonConvert.SerializeObject(valueBase);
                    jsonBase.Merge(JObject.Parse($@"{{""{propertyInfo.Name}"": {basePropertyInfo}}}"));
                    propertyInfo.SetValue(baseData, jsonValue);
                    isValidObject = true;
                }
            }

            if (!isValidObject)
                throw new InvalidConstraintException($"Not data to recovery.");

            this.ReverseData = JsonConvert.SerializeObject(jsonBase);
            return baseData;
        }

        public List<string> GetListToChangesReverse(int changeHistoryId)
        {
            using (var conn = new SqlConnection())
            {
                changeHistoryReverse = conn.Query<ChangeHistoryDTO>(
                       @"SELECT * FROM ChangeHistory WHERE ChangeHistoryId = @ChangeHistoryId",
                       new { ChangeHistoryId = changeHistoryId }).FirstOrDefault();
            }

            if (ChangeType == ChangeHistoryType.System)
            {
                return new List<string> { changeHistoryReverse.ChangedData };
            }

            Match match = Regex.Match(changeHistoryReverse.Observations, "[[][A-Za-z]+=[1-9][0-9]+[]]");
            if (!match.Success)
                throw new ArgumentException($"This register observation note, not is valid format for recovery information. Value: {this.changeHistoryReverse.Observations}");

            RefReverse = match.Value.Replace("[", string.Empty).Replace("]", string.Empty);
            ReverseId = Convert.ToInt32(RefReverse.Split('=')[1]);

            using (var conn = new SqlConnection(ConnectionString))
            {
                return conn.Query<string>(@"SELECT ch.ChangedData
                            FROM ChangeHistory ch
                        WHERE ch.Observations LIKE '%'+@refId+'%'
                        AND ch.ChangeTypeId = @changeTypeId
                        AND ch.ChangeHistoryId >= @changeHistoryid
                        ORDER BY ch.[DateTime] DESC",
                            SQLUtils.GetSqlParameters(new
                            {
                                changeHistoryid = changeHistoryReverse.ChangeHistoryId,
                                changeTypeId = (int)this.ChangeType,
                                refId = RefReverse
                            })
                    ).ToList();
            }
        }

        protected List<ChangeHistoryDTO> SaveReverseChanges()
        {
            if (ChangeType == ChangeHistoryType.System)
            {
                throw new InvalidOperationException("Not is possible save reverse changes for System type.");
            }

            List<ChangeHistoryDTO> res = new List<ChangeHistoryDTO>();
            if (RefReverse == null)
                throw new ApplicationException("LLama primero el methodo GetReverseChanges(), antes que SaveReverseChanges()");
            res.Add(Save(ReverseData, $"[{RefReverse}] Reverse to {changeHistoryReverse.ChangeHistoryId}"));
            return res;
        }

        public List<T> GetReverseAddOrRemove(int changeHistoryId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                changeHistoryReverse = conn.Query<ChangeHistoryDTO>(
                       @"SELECT * FROM ChangeHistory WHERE ChangeHistoryId = @ChangeHistoryId",
                       SQLUtils.GetSqlParameters(new { ChangeHistoryId = changeHistoryId })).FirstOrDefault();
            }

            List<T> reverse = new List<T>();
            reverse.AddRange(JsonConvert.DeserializeObject<List<T>>(changeHistoryReverse.ChangedData));

            Match match = Regex.Match(changeHistoryReverse.Observations, "[[][A-Za-z]+[]]");
            if (!match.Success)
                throw new ArgumentException($"This register observation note, not is valid format for recovery information. Value: {changeHistoryReverse.Observations}");
            ReverseMethod = match.Value.Replace("[", string.Empty).Replace("]", string.Empty);
            switch (ReverseMethod)
            {
                case "Remove":
                    AddList = reverse;
                    break;

                case "Add":
                    RemovesList = reverse;
                    break;
            }
            return reverse;
        }
    }
}