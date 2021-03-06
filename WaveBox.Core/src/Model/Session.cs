﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Cirrious.MvvmCross.Plugins.Sqlite;
using Newtonsoft.Json;
using Ninject;
using WaveBox;
using WaveBox.Core.Extensions;
using WaveBox.Core.Model;
using WaveBox.Core.Model.Repository;
using WaveBox.Core.Static;

namespace WaveBox.Core.Model {
    public class Session {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [JsonProperty("rowId"), IgnoreWrite]
        public int RowId { get; set; }

        [JsonIgnore]
        public string SessionId { get; set; }

        [JsonProperty("userId")]
        public int? UserId { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("createTime")]
        public long? CreateTime { get; set; }

        [JsonProperty("updateTime")]
        public long? UpdateTime { get; set; }

        public bool Update() {
            this.UpdateTime = DateTime.UtcNow.ToUnixTime();

            return Injection.Kernel.Get<ISessionRepository>().UpdateSession(this);
        }

        // Remove this session by its row ID
        public bool Delete() {
            return Injection.Kernel.Get<ISessionRepository>().DeleteSessionForRowId(this.RowId);
        }

        public override string ToString() {
            return String.Format("[Session: RowId={0}, SessionId={1}, UserId={2}]", this.RowId, this.SessionId, this.UserId);
        }
    }
}
