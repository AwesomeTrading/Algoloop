﻿/*
 * Copyright 2019 Capnode AB
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

using Algoloop.Model;
using QuantConnect;
using QuantConnect.Configuration;
using QuantConnect.ToolBox.FxcmDownloader;
using System;
using System.Collections.Generic;

namespace Algoloop.Provider
{
    class Fxcm : IProvider
    {
        public void Download(MarketModel model, SettingsModel settings, IList<string> symbols)
        {
            Config.Set("map-file-provider", "QuantConnect.Data.Auxiliary.LocalDiskMapFileProvider");
            Config.Set("data-directory", settings.DataFolder);
            switch (model.Access)
            {
                case MarketModel.AccessType.Demo:
                    Config.Set("fxcm-terminal", "Demo");
                    break;

                case MarketModel.AccessType.Real:
                    Config.Set("fxcm-terminal", "Real");
                    break;
            }

            Config.Set("fxcm-user-name", model.Login);
            Config.Set("fxcm-password", model.Password);

            string resolution = model.Resolution.Equals(Resolution.Tick) ? "all" : model.Resolution.ToString();
            DateTime fromDate = model.FromDate.Date;
            if (fromDate < DateTime.Today)
            {
                FxcmDownloaderProgram.FxcmDownloader(symbols, resolution, fromDate, fromDate);
                model.FromDate = fromDate.AddDays(1);
            }

            model.Active = model.FromDate < DateTime.Today;
        }

        public IEnumerable<SymbolModel> GetAllSymbols(MarketModel market)
        {
            throw new NotImplementedException();
        }
    }
}
