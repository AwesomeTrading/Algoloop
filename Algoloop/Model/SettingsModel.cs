﻿/*
 * Copyright 2018 Capnode AB
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Algoloop.ViewSupport;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Algoloop.Model
{
    [Serializable]
    [DataContract]
    public class SettingsModel : ModelBase
    {
        [Category("API")]
        [DisplayName("API access token")]
        [Description("Your unique access token to the API.")]
        [Browsable(true)]
        [ReadOnly(false)]
        [DataMember]
        public string ApiToken { get; set; }

        [Category("API")]
        [DisplayName("API user id")]
        [Description("Your user id for the API.")]
        [Browsable(true)]
        [ReadOnly(false)]
        [DataMember]
        public string ApiUser { get; set; }

        [Category("API")]
        [DisplayName("API data download")]
        [Description("Download missing data from QuantConnect.")]
        [Browsable(true)]
        [ReadOnly(false)]
        [DataMember]
        public bool ApiDownload { get; set; }

        [Category("API")]
        [DisplayName("Desktop port")]
        [Description("Desktop API port number.")]
        [Browsable(true)]
        [ReadOnly(false)]
        [DataMember]
        public int DesktopPort { get; set; }

        [DisplayName("Data folder")]
        [Description("Folder for market data.")]
        [Editor(typeof(FolderEditor), typeof(FolderEditor))]
        [Browsable(true)]
        [ReadOnly(false)]
        [DataMember]
        public string DataFolder { get; set; } = @"..\..\..\Data";

        [DisplayName("Max backtests")]
        [Description("Largest number of simultaneous ongoing backtest execution.")]
        [Browsable(true)]
        [ReadOnly(false)]
        [DataMember]
        public int MaxBacktests { get; set; } = Environment.ProcessorCount;

        internal void Copy(SettingsModel oldSettings)
        {
            ApiToken = oldSettings.ApiToken;
            ApiUser = oldSettings.ApiUser;
            ApiDownload = oldSettings.ApiDownload;
            DesktopPort = oldSettings.DesktopPort;
            DataFolder = oldSettings.DataFolder;
            MaxBacktests = oldSettings.MaxBacktests;
        }
    }
}
