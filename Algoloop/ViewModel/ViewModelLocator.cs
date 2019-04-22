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

/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Algoloop"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Algoloop.Service;
using GalaSoft.MvvmLight.Ioc;
using QuantConnect.Configuration;
using QuantConnect.Logging;
using QuantConnect.Util;

namespace Algoloop.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            // Register Algoloop types
            SimpleIoc.Default.Register<SettingService>();
            SimpleIoc.Default.Register<MarketService>();
            SimpleIoc.Default.Register<AccountService>();
            SimpleIoc.Default.Register<StrategyService>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MarketsViewModel>();
            SimpleIoc.Default.Register<AccountsViewModel>();
            SimpleIoc.Default.Register<StrategiesViewModel>();
            SimpleIoc.Default.Register<LogViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();

            // Register QuantConnect types
            SimpleIoc.Default.Register(
                () => Composer.Instance.GetExportedValueByTypeName<ILogHandler>(Config.Get("log-handler", "Algoloop.Lean.LogItemHandler")));
        }

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
        public MarketsViewModel MarketsViewModel => SimpleIoc.Default.GetInstance<MarketsViewModel>();
        public AccountsViewModel AccountsViewModel => SimpleIoc.Default.GetInstance<AccountsViewModel>();
        public StrategiesViewModel StrategiesViewModel => SimpleIoc.Default.GetInstance<StrategiesViewModel>();
        public LogViewModel LogViewModel => SimpleIoc.Default.GetInstance<LogViewModel>();
        public SettingsViewModel SettingsViewModel => SimpleIoc.Default.GetInstance<SettingsViewModel>();
        public AboutViewModel AboutViewModel => SimpleIoc.Default.GetInstance<AboutViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}