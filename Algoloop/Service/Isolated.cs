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

using System;

namespace Algoloop.Service
{
    public sealed class Isolated<T> : IDisposable where T : MarshalByRefObject
    {
        private AppDomain _domain;
        private T _value;

        public Isolated()
        {
            Type type = typeof(T);

            AppDomainSetup appSetup = new AppDomainSetup()
            {
                ApplicationName = type.Name,
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
                PrivateBinPath = @".",
                ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
            };

            string name = "Isolated:" + Guid.NewGuid();
            _domain = AppDomain.CreateDomain(
                name, 
                AppDomain.CurrentDomain.Evidence,
                appSetup);

            _value = (T)_domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public T Value
        {
            get
            {
                return _value;
            }
        }

        public void Dispose()
        {
            if (_domain != null)
            {
                try
                {
                    AppDomain.Unload(_domain);
                }
                catch
                {
                }

                _domain = null;
            }
        }
    }
}
