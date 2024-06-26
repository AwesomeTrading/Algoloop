/*
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

using Algoloop.Wpf.ViewModels.Internal.Lean;
using QuantConnect.Logging;
using System.Diagnostics;

namespace Algoloop.Wpf.ViewModels
{
    public class LogViewModel : ViewModelBase
    {
        public LogViewModel()
        {
            if (Log.LogHandler is ILogItemHandler logService)
            {
                logService.Connect((item) => Logs.Add(item));
            }

            Debug.Assert(IsUiThread(), "Not UI thread!");
        }

        public SyncObservableCollection<LogItem> Logs { get; } = new SyncObservableCollection<LogItem>();
    }
}
